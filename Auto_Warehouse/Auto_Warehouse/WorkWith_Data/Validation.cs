using System;
using System.Collections.Generic;

namespace Auto_Warehouse
{
	public class Validation
	{
		
		public Validation ()
		{
			
		}



		public bool IsEmpty(string line)		
		{			
		   if (line=="")
					return true;
			
			return false;
		}



		public bool ExistInList(List<StructOfProduct> productsList, StructOfProduct lineForInsert)
		{
			bool ansver = false;

			foreach (StructOfProduct product in productsList) 
			{
				if (product.Number == lineForInsert.Number)
					ansver = true;
			}

			return ansver;
		}
		public bool ExistInList(List<StructDataDeal> productsList, StructDataDeal lineForInsert)
		{
			bool ansver = false;

			foreach (StructDataDeal deal in productsList) 
			{
				if (deal.Number == lineForInsert.Number)
					ansver = true;
			}

			return ansver;
		}
		public bool ExistInList(List<StructOfProduct> productsList, string lineForInsert)
		{
			bool ansver = false;

			foreach (StructOfProduct product in productsList) 
			{
				if (product.Number == lineForInsert) 
				{
					ansver = true;
					break;
				}
			}

			return ansver;
		}


		public bool IsApprove(int exist,int forDel)
		{
			bool ansver = true;

			if (exist < forDel)
				ansver = false;

			return ansver;
		}



	}
}

