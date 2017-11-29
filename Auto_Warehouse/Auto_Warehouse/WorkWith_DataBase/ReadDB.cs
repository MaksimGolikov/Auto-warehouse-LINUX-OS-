using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Data.Common;
using Npgsql;
using NpgsqlTypes;

namespace Auto_Warehouse
{
	public class ReadDB
	{

		NpgsqlConnection connection;
		NpgsqlCommand command;



		public ReadDB (string pathTo_db,Bufer buf)
		{
			ConnectTo(pathTo_db,buf);
		}

		private void ConnectTo(string path,Bufer buf)
		{
			connection = new NpgsqlConnection ("Server="+buf.ServerName+";Port=5432; User Id=postgres; Password="+buf.Password+"; Database="+path+";");
			command =connection.CreateCommand();
		}



		public List<StructOfProduct> Get_All_Products()
		{
			List<StructOfProduct> products=new List<StructOfProduct>();
			List<int> all_Id=Get_AllId_();


			for(int i=0;i<all_Id.Count;i++)
			{
				products.Add (Get_Product(all_Id[i]));
			}


			return products;
		}
		public List<string> Get_TypeOfCount_All()
		{
			List<string> type = new List<string>();

			try
			{				
				command.CommandText="SELECT * FROM type_counting_t";
				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{			
					type.Add(reader["type_counting_name"].ToString());
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return type;
		}
		public List<string> Get_Producer_All()
		{
			List<string> producer = new List<string>();

			try
			{				
				command.CommandText="SELECT * FROM producer_t";
				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{			
					producer.Add(reader["producer_name"].ToString());
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return producer;
		}
		public int GetID_BYNumber(string name)
		{
			int id = 0;

			try
			{				
				command.CommandText="SELECT * FROM product_t";
				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{	
					string filtr=reader["code"].ToString();

					if(filtr==name)
					{
						id=Convert.ToInt32(reader["id_product"].ToString());  
						break;
					}
				}
			}
			catch
			{
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}

			return id;

		}

		public int GetCount_BYiD(int number)
		{
			int count = 0;

			try
			{				
				command.CommandText="SELECT * FROM product_t";
				command.CommandType = CommandType.Text;

				connection.Open();


				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{	
					int filtr=Convert.ToInt32(reader["id_product"].ToString());

					if(filtr==number)
					{
						count=Convert.ToInt32(reader["count_value"].ToString());  
						break;
					}
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}

			return count;

		}
		public int GetPriceForOne_BYiD(int number)
		{
			int price = 0;

			try
			{				
				command.CommandText="SELECT * FROM product_t";
				command.CommandType = CommandType.Text;

				connection.Open();


				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{	
					int filtr=Convert.ToInt32(reader["id_product"].ToString());

					if(filtr==number)
					{
						price=Convert.ToInt32(reader["price_for_one"].ToString());					
						break;
					}
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}

			return price;

		}
		public int GetTypeID_BYTypeName(string name)
		{
			int id = 0;

			try
			{				
				command.CommandText="SELECT * FROM type_counting_t";
				command.CommandType = CommandType.Text;

				connection.Open();


				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{	
					string filtr=reader["type_counting_name"].ToString();
					if(filtr==name)
					{
						id=Convert.ToInt32(reader["id_type_counting"].ToString());  
						break;
					}
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}

			return id;

		}
		public List<StructDataDeal> Get_All_Deal()
		{
			List<StructDataDeal> All_deals=new List<StructDataDeal>();
			List<int> all_Id=GetCountOfDeal();


			for (int i = 0; i < all_Id.Count; i++)
			{
				All_deals.Add (Get_Deal( all_Id[i]));
			}


			return All_deals;
		}

		public List<StructOfProduct> Search(string filtr, ParametrFiltr param)
		{
			List<StructOfProduct> products = new List<StructOfProduct> ();

			try
			{	
				switch(param)
				{
				case ParametrFiltr.Code:
					command.CommandText="SELECT * FROM product_t WHERE code LIKE '%"+filtr+"%'";
					break;

				case ParametrFiltr.ProductName:
					command.CommandText="SELECT * FROM product_t WHERE name_product LIKE '%"+filtr+"%'";
					break;

				case ParametrFiltr.Parametr:
					command.CommandText="SELECT * FROM product_t WHERE parametr LIKE '%"+filtr+"%'";
					break;
				default:
					command.CommandText="SELECT * FROM product_t";
					break;
				}

				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{		
					StructOfProduct product=new StructOfProduct();

					product.ID=Convert.ToInt32(reader["id_product"].ToString());
					product.Number=reader["code"].ToString();									
					product.Name=reader["name_product"].ToString();

					product.Id_producer=Convert.ToInt32(reader["id_producer"].ToString());
					product.Id_typeCount=Convert.ToInt32(reader["id_type_counter"].ToString());

					product.PriceForOne=reader["price_for_one"].ToString();
					product.Parametr=reader["parametr"].ToString();
					product.ValueParametr=reader["parametr_value"].ToString();
					product.Count=Convert.ToInt32(reader["count_value"].ToString());

					products.Add(product);
				}
			}

			catch
			{
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}

			foreach (StructOfProduct product in products) 
			{
				product.Produser = Get_ProducerByID (product.Id_producer);
				product.TypeCount = Get_TypeOfCount_ByID (product.Id_typeCount);
			}



			return products;
		}

		public int Get_ProducerID_ByName(string producer)
		{
			int producerId=0;


			try
			{				
				command.CommandText="SELECT * FROM producer_t";
				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{						
					string filtr=reader["producer_name"].ToString();
					if(filtr==producer)
					{
						producerId=Convert.ToInt32(reader["id_producer"].ToString());
						break;
					}
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}

			return producerId;
		}
		public int Get_TypeOfCountID_ByName(string typeCounter)
		{
			int typeId = 0;


			try
			{				
				command.CommandText="SELECT * FROM type_counting_t";
				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{			
					string filtr=reader["type_counting_name"].ToString();
					if(filtr==typeCounter)
					{
						typeId=Convert.ToInt32(reader["id_type_counting"].ToString());
					}
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return typeId;
		}

		// PRODUCT TABLE
		private StructOfProduct Get_Product(int id_labe)
		{
			StructOfProduct product=new StructOfProduct();
			int id_1=0;
			int id_2=0;


			try
			{				
				command.CommandText="SELECT * FROM product_t WHERE id_product="+id_labe+"";
				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{	
					product.ID=Convert.ToInt32(reader["id_product"].ToString());
					product.Number=reader["code"].ToString();									
					product.Name=reader["name_product"].ToString();

					id_1=Convert.ToInt32(reader["id_producer"].ToString());
					id_2=Convert.ToInt32(reader["id_type_counter"].ToString());

					product.PriceForOne=reader["price_for_one"].ToString();
					product.Parametr=reader["parametr"].ToString();
					product.ValueParametr=reader["parametr_value"].ToString();
					product.Count=Convert.ToInt32(reader["count_value"].ToString());

				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}

			product.Produser=Get_ProducerByID(id_1);
			product.TypeCount=Get_TypeOfCount_ByID(id_2);

			return product;
		}
		private string Get_ProducerByID(int id_producer)
		{
			string producer = "";


			try
			{				
				command.CommandText="SELECT * FROM producer_t WHERE id_producer="+id_producer+"";
				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{			
					producer=reader["producer_name"].ToString();
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}

			return producer;
		}
		private string Get_TypeOfCount_ByID(int id_typeCounter)
		{
			string type = "";


			try
			{				
				command.CommandText="SELECT * FROM type_counting_t WHERE id_type_counting="+id_typeCounter+"";
				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{	
					type=reader["type_counting_name"].ToString();
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return type;
		}
		private List<int> Get_AllId_()
		{
			List<int> all_id = new List<int> ();

			try
			{		

				command.CommandText="SELECT * FROM product_t";
				command.CommandType = CommandType.Text;


				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{			
					all_id.Add(Convert.ToInt32(reader["id_product"].ToString()));
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return all_id;
		}




		//CHANGE TABLE
		private StructDataDeal Get_Deal(int id)
		{
			StructDataDeal deal = new StructDataDeal();
			int id_product=0;
			int id_2=0;
			int id_=0;

			try
			{				
				command.CommandText="SELECT *FROM change_t WHERE id_change="+id+"";
				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{	

					id_product=Convert.ToInt32(reader["id_product_name"].ToString());

					deal.Count=Convert.ToInt32(reader["count_value"].ToString());

					id_2=Convert.ToInt32(reader["id_type_count"].ToString());


					deal.Price=reader["price"].ToString();
					deal.DateWrote=reader["data_wrote"].ToString();
					deal.NameDoc=reader["name_document"].ToString();
					deal.NumberDoc=reader["number_document"].ToString();
					deal.DateDocCreated=reader["data_document"].ToString();
					deal.Warhouser=reader["wherehouser"].ToString();

				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}


			try
			{	


				command.CommandText="SELECT * FROM product_t";
				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{	
					id_=Convert.ToInt32(reader["id_product"]);

					if(id_== id_product)
					{
						deal.Number=reader["code"].ToString();									
						deal.Name=reader["name_product"].ToString();
						break;
					}
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}


			deal.TypeCount=Get_TypeOfCount_ByID(id_2);

			return deal;
		}
		private List<int> GetCountOfDeal()
		{			
			List<int> all_id = new List<int> ();

			try
			{				
				command.CommandText="SELECT * FROM change_t";
				command.CommandType = CommandType.Text;

				connection.Open();

				NpgsqlDataReader reader=command.ExecuteReader();

				while (reader.Read())
				{			
					all_id.Add(Convert.ToInt32(reader["id_change"].ToString()));
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return all_id;

		}

	}
}
