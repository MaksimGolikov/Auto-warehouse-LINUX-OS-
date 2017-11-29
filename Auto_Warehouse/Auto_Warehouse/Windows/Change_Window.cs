using System;
using Gtk;
using System.Collections.Generic;



namespace Auto_Warehouse
{
	public partial class Change_Window : Gtk.Window
	{

		List<StructOfProduct> _AllProduct;
		List<StructDataDeal> _DataForInsert;
		List<StructDataDeal> data_ForDB;
		StructOfProduct _SelectProduct;
		ReadDB readDB;
		WriteTo_DB write;
		string nameDB;
		Bufer buf;

		//Code, NAME, PRUDUCER, TYPECount, COUNT
		ListStore mainModel = new ListStore (typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
		// CODE, NAME, TYPECount,COUNT, Price
		ListStore docModel = new ListStore (typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));






		public Change_Window (List<StructOfProduct> allProducts, StructOfProduct selectProduct,string nameDataBase,Bufer bufer) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();

			_AllProduct = allProducts;
			_SelectProduct = selectProduct;

			buf = bufer;

			Initialize (nameDataBase);

			SetGotStructOfPruduct (selectProduct);
		}



		private void Initialize(string nameDatBas)
		{
			this.nameDB = nameDatBas;

			readDB = new ReadDB (nameDB,buf);
			write = new WriteTo_DB (nameDB,buf);

			_DataForInsert = new List<StructDataDeal> ();
			data_ForDB = new List<StructDataDeal> ();

			TopLineOf_InsertTable ();
			TopLineOf_MainTable ();


			SetData_ToMainTab (_AllProduct);


			tb_TodeyDate_ForInsert.Text = DateTime.Now.ToString ("d");

			List<string> ParametrsForSearchList = new List<string> ();

			ParametrsForSearchList.Add ("По Номеру");
			ParametrsForSearchList.Add ("По Названию");

			Set_ListOfSearchParams (ParametrsForSearchList);
		}
		private void Set_ListOfSearchParams(List<string> ParametrsForSearchList)
		{
			foreach (string parametr in ParametrsForSearchList) 
			{
				cb_ParamForSearch.AppendText (parametr);
			}
		}
		private void SearchData(string textForSerch,string bywhatSearch)
		{
			Serach serch = new Serach (nameDB,buf);
			Validation validation = new Validation ();


			switch(bywhatSearch)
			{
			case "По Номеру":

				if (!validation.IsEmpty (textForSerch))
				{
					Clear_main_Data ();
					SetData_ToMainTab (serch.Finde_ByNumber(textForSerch));		
				}

				break;
			case "По Названию":

				if (!validation.IsEmpty (textForSerch))
				{
					Clear_main_Data ();
					SetData_ToMainTab (serch.Finde_ByName (textForSerch));	
				}

				break;

			default:

				break;
			}

		}





		//ALL DATA LIST
		private void TopLineOf_MainTable()
		{


			treeview_AllProduct.Model = mainModel;

			treeview_AllProduct.AppendColumn ("  Номер   ",new CellRendererText(),"text",0);
			treeview_AllProduct.AppendColumn ("  Название   ",new CellRendererText(),"text",1);
			treeview_AllProduct.AppendColumn ("  Производитель   ",new CellRendererText(),"text",2);
			treeview_AllProduct.AppendColumn ("  Еденица измерения   ",new CellRendererText(),"text",3);
			treeview_AllProduct.AppendColumn ("  Количество   ",new CellRendererText(),"text",4);
		}
		private void Clear_main_Data()
		{		
			mainModel.Clear ();
		}

		// DOC DATA LIST
		private void TopLineOf_InsertTable()
		{

			treeview_InsertList.Model = docModel;

			treeview_InsertList.AppendColumn ("  Номер   ",new CellRendererText(),"text",0);
			treeview_InsertList.AppendColumn ("  Название   ",new CellRendererText(),"text",1);		
			treeview_InsertList.AppendColumn ("  Еденица измерения   ",new CellRendererText(),"text",2);
			treeview_InsertList.AppendColumn ("  Количество   ",new CellRendererText(),"text",3);
			treeview_InsertList.AppendColumn ("  Цена   ",new CellRendererText(),"text",4);
		}
		private void Clear_Doc_Data()
		{		
			docModel.Clear ();
		}


		private void SetData_ToMainTab(List<StructOfProduct> _ProductsList)
		{
			for (int i = 0; i < _ProductsList.Count; i++)
			{
				mainModel.AppendValues (_ProductsList[i].Number, _ProductsList[i].Name, _ProductsList[i].Produser, _ProductsList[i].TypeCount,_ProductsList[i].Count.ToString());
			}
		}
		private void AddSelectProduct(StructDataDeal deal)
		{
			docModel.AppendValues (deal.Number,deal.Name,deal.TypeCount,deal.Count.ToString(),deal.Price);
		}



		private void SetGotStructOfPruduct(StructOfProduct product)
		{
			try
			{				
				tb_Name_ForInsert.Text = product.Name;
				tb_Code_ForInsert.Text = product.Number;
			}
			catch
			{
			}
		}


		private void Refresh_DataForInsert()
		{
			Clear_Doc_Data ();

			for (int i = 0; i < _DataForInsert.Count; i++)
			{
				AddSelectProduct (_DataForInsert[i]);
			}

		}
		private void Clear_DataForInsert()
		{
			for (int i = 0; i < _DataForInsert.Count; i++)
			{
				_DataForInsert.RemoveAt(0);
			}
		}

		private void Refresh_MainList()
		{			
			mainModel.Clear ();

			_AllProduct = readDB.Get_All_Products ();

			//Add EMPTY LINE
			mainModel.AppendValues();

			SetData_ToMainTab (_AllProduct);
		}

		private void Clear_AllProduct()
		{
			for (int i = 0; i < _AllProduct.Count; i++)
			{
				_AllProduct.RemoveAt(0);
			}
		}





		private void UpdateDB(bool AddData)
		{	
			bool resalt = true;

			if (AddData) 
			{
				foreach (StructDataDeal deal in _DataForInsert)
				{
					int id_product = readDB.GetID_BYNumber (deal.Number);
					int countForInsert = readDB.GetCount_BYiD (id_product) + Convert.ToInt32 (deal.Count);

					deal.ID = id_product;
					deal.Count = Convert.ToInt32(countForInsert.ToString ());
					data_ForDB.Add (deal);
				}
			} 
			else
			{
				foreach (StructDataDeal deal in _DataForInsert)
				{
					int id_product = readDB.GetID_BYNumber (deal.Number);
					int countForInsert = readDB.GetCount_BYiD (id_product) - Convert.ToInt32 (deal.Count);

					deal.ID = id_product;
					deal.Count = Convert.ToInt32(countForInsert.ToString ());
					data_ForDB.Add (deal);
				}
			}



			foreach(StructDataDeal deal in data_ForDB)
			{				
				resalt=	write.UpdateCount_ExistPruduct (deal);
				resalt = write.Write_ToChangeTab (deal);
			}

			if(resalt)
				MessageBox.Show ("Отчет сформирован");							
		}


//ACTIONS

		protected void OnCalendarDaySelectedDoubleClick (object sender, EventArgs e)
		{
			tb_DateCreateDoc_ForInsert.Text=calendar.GetDate().ToString("d");
		}
	//TREE VIEW
		protected void OnTreeviewAllProductRowActivated (object o, RowActivatedArgs args)
		{
			Auto_Warehouse.Parametr_Window win = new Parametr_Window (_SelectProduct);
		}

		protected void OnTreeviewAllProductCursorChanged (object sender, EventArgs e)
		{
			TreePath[] f=  treeview_AllProduct.Selection.GetSelectedRows();

			_SelectProduct = _AllProduct [f [0].Indices [0]];
			SetGotStructOfPruduct (_AllProduct[f [0].Indices[0]]);
		}

		protected void OnTreeviewInsertListRowActivated (object o, RowActivatedArgs args)
		{
			TreePath[] f= treeview_InsertList.Selection.GetSelectedRows();
			_DataForInsert.RemoveAt (f [0].Indices[0]);

			Refresh_DataForInsert ();
		}

//BUTTONS

		protected void OnBnSearchClicked (object sender, EventArgs e)
		{
			SearchData (tb_ValueForSearch.Text,cb_ParamForSearch.ActiveText);
		}
		protected void OnBnGetAllProductClicked (object sender, EventArgs e)
		{
			Clear_main_Data ();

			SetData_ToMainTab (_AllProduct);
		}


		protected void OnBnAddingDataFromInsertListClicked (object sender, EventArgs e)
		{
			if (_DataForInsert.Count != 0)
			{
				UpdateDB (true);

				Clear_DataForInsert ();

				Refresh_DataForInsert ();
				Refresh_MainList ();
			}
			else
				MessageBox.Show (" Список пуст. Добавте продукт");
		}

		protected void OnBnMInesDataFromInsertListClicked (object sender, EventArgs e)
		{
			Validation validation = new Validation ();

			if(_DataForInsert.Count!=0)
			{
				bool goNext = true;
				string numberWithErrore = "";

				foreach(StructDataDeal deal in _DataForInsert)
				{
					int countForDeal = Convert.ToInt32 (deal.Count);
					int countExist = readDB.GetCount_BYiD(readDB.GetID_BYNumber(deal.Number));

					if (!validation.IsApprove (countExist, countForDeal)) 
					{
						goNext = false;
						numberWithErrore = deal.Number;
						break;
					}
				}

				if (goNext) 
				{					
					UpdateDB (false);

					Clear_DataForInsert ();

					Refresh_DataForInsert ();
					Refresh_MainList ();
				}
				else
					MessageBox.Show ("Недостаточное количество для проведения данной операции. Удалите продукт из списка или измените количество. Продукт с номером "+numberWithErrore);
		}
			else
				MessageBox.Show (" Список пуст. Добавте продукт");

		}


		protected void OnBnAddToInsertListClicked (object sender, EventArgs e)
		{
			Validation validation = new Validation ();

			if(!validation.IsEmpty(tb_Name_ForInsert.Text) && !validation.IsEmpty(tb_Code_ForInsert.Text) && 
				!validation.IsEmpty(tb_DocName_ForInsert.Text) && !validation.IsEmpty(tb_DocNumber_ForInsert.Text) && 
				!validation.IsEmpty(tb_DateCreateDoc_ForInsert.Text) && !validation.IsEmpty(tb_Count_ForInsert.Text) && 
				!validation.IsEmpty(tb_TodeyDate_ForInsert.Text) && !validation.IsEmpty(tb_Warehouser_ForInsert.Text))
			{
				StructDataDeal deal = new StructDataDeal ();
				StructDataDeal deal_new = new StructDataDeal ();

				deal.Name = tb_Name_ForInsert.Text;
				deal.Number = tb_Code_ForInsert.Text;
				deal.NameDoc = tb_DocName_ForInsert.Text;
				deal.NumberDoc = tb_DocNumber_ForInsert.Text;
				deal.DateDocCreated = tb_DateCreateDoc_ForInsert.Text;
				deal.Count = Convert.ToInt32(tb_Count_ForInsert.Text);
				deal.DateWrote = tb_TodeyDate_ForInsert.Text;
				deal.TypeCount = _SelectProduct.TypeCount;
				deal.Warhouser = tb_Warehouser_ForInsert.Text;


				int price=Convert.ToInt32 (deal.Count) * Convert.ToInt32(_SelectProduct.PriceForOne);

				deal.Price = Convert.ToString(price);

				if (!validation.ExistInList (_DataForInsert, deal))
				{
					_DataForInsert.Add (deal);

					Refresh_DataForInsert ();
				}
				else
					MessageBox.Show ("Такая запись уже есть");
			}
			else
				MessageBox.Show ("Параметры не заданы или заданы не правильно. ");
		}






































	}
}

