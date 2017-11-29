using System;
using Npgsql;
using NpgsqlTypes;
using System.Data;


namespace Auto_Warehouse
{
	public class WriteTo_DB
	{
		NpgsqlConnection connection;
		NpgsqlCommand command;

		ReadDB readDB;


		public WriteTo_DB (string pathToDb,Bufer buf)
		{
			ConnectTo (pathToDb,buf);

			readDB = new ReadDB (pathToDb,buf);
		}

		private void ConnectTo(string path,Bufer buf)
		{
			connection = new NpgsqlConnection ("Server="+buf.ServerName+";Port=5432; User Id=postgres; Password="+buf.Password+"; Database="+path+";");
			command =connection.CreateCommand();
		}



		public bool UpdateCount_ExistPruduct(StructDataDeal deal)
		{
			bool resalst = true;

			try
			{
				int newCount=Convert.ToInt32(deal.Count);

				command.Connection=connection;

				command.CommandText="UPDATE product_t SET count_value='"+newCount+"' WHERE id_product="+deal.ID;
				command.CommandType=CommandType.Text;
				connection.Open();


				command.ExecuteNonQuery();
			}
			catch 
			{
				resalst = false;
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return resalst;
		}
		public bool Write_newProduct(StructOfProduct product)
		{
			bool resalt = true;

			try
			{

				// WHITOUT COUNT BECAUSE COUNT 0 BY START CONFIG IN DB
				command.CommandType=CommandType.Text;
				command.CommandText ="INSERT INTO product_t(code ,id_producer ,name_product , id_type_counter ,price_for_one ,parametr ,parametr_value ,count_value )" +
					" VALUES('"+product.Number+"','"+product.Id_producer+"','"+product.Name+"','"+product.Id_typeCount+"','"+product.PriceForOne+"','"+product.Parametr+"','"+product.ValueParametr+"','"+0+"')";

				connection.Open();

				command.ExecuteNonQuery();

			}
			catch (Exception)
			{
				resalt = false;
			}

			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}

			return resalt;
		}
		public bool Write_ToChangeTab(StructDataDeal deal)
		{
			bool resalt = true;

			int id_productName = readDB.GetID_BYNumber (deal.Number);
			int id_typeCount = readDB.Get_TypeOfCountID_ByName (deal.TypeCount);

			try
			{

				command.CommandType=CommandType.Text;
				command.CommandText ="INSERT INTO change_t(id_product_name,count_value,id_type_count,price,data_wrote,name_document,number_document,data_document,wherehouser)" +
					" VALUES('"+id_productName+"','"+deal.Count+"','"+id_typeCount+"','"+deal.Price+"','"+deal.DateWrote+"','"+deal.NameDoc+"','"+deal.NumberDoc+"','"+deal.DateDocCreated+"','"+deal.Warhouser+"')";

				connection.Open();

				command.ExecuteNonQuery();

			}
			catch (Exception)
			{
				resalt = false;
			}

			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}

			return resalt;
		}


		public bool DeleteProduct(int ID)
		{
			bool resalt = true;

			try
			{
				command.CommandType=CommandType.Text;
				command.CommandText ="DELETE FROM product_t WHERE id_product="+ID;

				connection.Open();

				command.ExecuteNonQuery();

			}
			catch (Exception)
			{
				resalt = false;
			}

			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}

			return resalt;
		}


		public int Get_Or_Set_IdOfProducer(string producer)
		{
			int id_Producer=readDB.Get_ProducerID_ByName(producer);

			if (id_Producer == 0) 
			{
				try
				{
					connection.Open();
					command.Connection = connection;
					command.CommandType=CommandType.Text;

					command.CommandText = "INSERT INTO producer_t (producer_name) VALUES('"+producer+"')";

					command.ExecuteNonQuery();

					id_Producer = readDB.Get_ProducerID_ByName (producer);
				}
				catch (Exception)
				{					
				}

				finally
				{
					if (connection != null)
					{
						connection.Close();
					}
				}
			}



			return id_Producer;
		}
		public int Get_Or_Set_IdOfTypeCount(string type)
		{
			int id_typeCount = readDB.Get_TypeOfCountID_ByName (type);

			if (id_typeCount == 0) 
			{
				try
				{
					connection.Open();
					command.Connection = connection;
					command.CommandType=CommandType.Text;

					command.CommandText = "INSERT INTO type_counting_t(type_counting_name) VALUES('"+type+"')";

					command.ExecuteNonQuery();

					id_typeCount =  readDB.Get_TypeOfCountID_ByName (type);
				}
				catch (Exception)
				{					
				}

				finally
				{
					if (connection != null)
					{
						connection.Close();
					}
				}
			}



			return id_typeCount;
		}





	}
}

