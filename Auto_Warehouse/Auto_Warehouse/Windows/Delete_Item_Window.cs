using System;
using System.Collections.Generic;
using Gtk;


namespace Auto_Warehouse
{
	public partial class Delete_Item_Window : Gtk.Window
	{

		StructOfProduct _SelectProduct;
		List<StructOfProduct> _ProductsList;
		List<StructOfProduct> _DataForDelete;

		ListStore model = new ListStore (typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
		ListStore _ForDelete = new ListStore (typeof (string), typeof(string),typeof(string),typeof(string));

		WriteTo_DB write;
		ReadDB read;
		string nameDB;
		Bufer buf;


		public Delete_Item_Window (List<StructOfProduct> productsList,string nameDataBase,Bufer bufer) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();

			_ProductsList = productsList;
			_DataForDelete = new List<StructOfProduct> ();

			nameDB = nameDataBase;

			buf = bufer;
			Initialize (productsList);
		}




		private void Initialize(List<StructOfProduct> _ProductsList)
		{
			write = new WriteTo_DB (nameDB,buf);
			read = new ReadDB (nameDB,buf);

			Clear_CurrentData ();
			GetDataFromDB (_ProductsList);
			TopLineOfTable ();

			TopLineOfDelteTab ();
		}

		private void TopLineOfTable()
		{
			treeview_AllProduct.Model = model;

			treeview_AllProduct.AppendColumn ("  Номер   ",new CellRendererText(),"text",0);
			treeview_AllProduct.AppendColumn ("  Название   ",new CellRendererText(),"text",1);
			treeview_AllProduct.AppendColumn ("  Производитель   ",new CellRendererText(),"text",2);
			treeview_AllProduct.AppendColumn ("  Праметр   ",new CellRendererText(),"text",3);
			treeview_AllProduct.AppendColumn ("  Значение   ",new CellRendererText(),"text",4);
			treeview_AllProduct.AppendColumn ("  Еденица измерения   ",new CellRendererText(),"text",5);
			treeview_AllProduct.AppendColumn ("  Количество   ",new CellRendererText(),"text",6);

		}

		private void Clear_CurrentData()
		{		
			model.Clear ();
		}

		private void GetDataFromDB(List<StructOfProduct> productsList)
		{

			for (int i = 0; i < productsList.Count; i++)
			{
				model.AppendValues (productsList[i].Number, productsList[i].Name, productsList[i].Produser,productsList[i].Parametr,
					productsList[i].ValueParametr, productsList[i].TypeCount,productsList[i].Count.ToString());
			} 
		}





		private void TopLineOfDelteTab()
		{			
			treeview_DeleteList.Model =_ForDelete;

			treeview_DeleteList.AppendColumn ("  Номер   ",new CellRendererText(),"text",0);
			treeview_DeleteList.AppendColumn ("  Название   ",new CellRendererText(),"text",1);
			treeview_DeleteList.AppendColumn ("  Еденица измерения ",new CellRendererText(),"text",2);
			treeview_DeleteList.AppendColumn ("  Количество ",new CellRendererText(),"text",3);
		}

		private void SetDataForDelete(StructOfProduct productsList)
		{		
			Validation validation = new Validation ();

			if (!validation.ExistInList (_DataForDelete, productsList)) 
			{
				AddNewItemToDeletelist (productsList);
			} 
			else
				MessageBox.Show ("Товар с таким нонером уже есть в списке");
		} 

		private void AddNewItemToDeletelist(StructOfProduct productsList)
		{
			_DataForDelete.Add (productsList);	
			_ForDelete.Clear ();

			for (int i = 0; i < _DataForDelete.Count; i++)
			{
				_ForDelete.AppendValues (_DataForDelete [i].Number, _DataForDelete [i].Name, _DataForDelete [i].TypeCount, _DataForDelete [i].Count.ToString ());
			}
		}

		private void RefrashMainList()
		{
			_ProductsList = read.Get_All_Products ();

			GetDataFromDB (_ProductsList);
		}



//ACTIONS

		//BUTTONS
		protected void OnBnClearDeleteListClicked (object sender, EventArgs e)
		{
			_DataForDelete = new List<StructOfProduct> ();

			_ForDelete.Clear ();
		}

		protected void OnBnDeleteItemFromListClicked (object sender, EventArgs e)
		{
			if (_DataForDelete.Count != 0)
			{	
				bool resalt = true;

				foreach (StructOfProduct product in _DataForDelete) 
				{					
					resalt=write.DeleteProduct (product.ID);
				}	


				if (resalt)
					MessageBox.Show ("Успешно");
				else
					MessageBox.Show (" Не успешно");


				model.Clear ();
				RefrashMainList ();
				_ForDelete.Clear ();
			}
		}
		//TREE VIEW

		protected void OnTreeviewDeleteListRowActivated (object o, RowActivatedArgs args)
		{
			TreePath[] f= treeview_DeleteList.Selection.GetSelectedRows();
			_DataForDelete.RemoveAt (f [0].Indices[0]);

			_ForDelete.Clear ();

			for (int i = 0; i < _DataForDelete.Count; i++)
			{
				_ForDelete.AppendValues (_DataForDelete [i].Number, _DataForDelete [i].Name, _DataForDelete [i].TypeCount, _DataForDelete [i].Count.ToString ());
			}
		}


		protected void OnTreeviewAllProductCursorChanged (object sender, EventArgs e)
		{
			TreePath[] f= treeview_AllProduct.Selection.GetSelectedRows();

			SetDataForDelete (_ProductsList[f [0].Indices[0]]);
		}





























	}
}

