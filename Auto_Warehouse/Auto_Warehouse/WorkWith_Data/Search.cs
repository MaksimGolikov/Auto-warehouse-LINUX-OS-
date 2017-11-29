using System;
using System.Collections.Generic;



namespace Auto_Warehouse
{
	public class Serach
	{
		string pathToDB;
		Bufer bufer;


		public Serach (string pathTO_DB,Bufer buf)
		{
			pathToDB = pathTO_DB;
			bufer = buf;
		}


		public List<StructOfProduct> Finde_ByName(string name)
		{
			List<StructOfProduct> filtredData = new List<StructOfProduct> ();

			ReadDB readDB = new ReadDB (pathToDB,bufer);

			filtredData = readDB.Search(name,ParametrFiltr.ProductName);

			return filtredData;
		}

		public List<StructOfProduct> Finde_ByNumber(string number)
		{
			List<StructOfProduct> filtredData = new List<StructOfProduct> ();

			ReadDB readDB = new ReadDB (pathToDB,bufer);

			filtredData = readDB.Search(number,ParametrFiltr.Code);

			return filtredData;
		}

		public List<StructOfProduct> Finde_ByParametr(string parametr, string valueParam)
		{
			List<StructOfProduct> filtredData = new List<StructOfProduct> ();

			ReadDB readDB = new ReadDB (pathToDB,bufer);

			List<StructOfProduct> data = readDB.Search(parametr,ParametrFiltr.Parametr);


			foreach (StructOfProduct product in data) 
			{
				if (product.ValueParametr==valueParam)
					filtredData.Add (product);
			}


			return filtredData;
		}



	}
}