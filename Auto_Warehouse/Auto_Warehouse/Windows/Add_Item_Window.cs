using System;
using System.Collections.Generic;


namespace Auto_Warehouse
{
	public partial class Add_Item_Window : Gtk.Window
	{
		List<StructOfProduct> _ExistProduct;
		string nameDB;
		Bufer buf;


		public Add_Item_Window (List<StructOfProduct> exist_products,string nameDatabase,Bufer bufer) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();

			_ExistProduct = exist_products;

			nameDB = nameDatabase;
			buf = bufer;
			Initialize ();
		}


		private void Initialize()
		{
			ReadDB readDB = new ReadDB (nameDB,buf);

			List<string> AllTypeCount = readDB.Get_TypeOfCount_All ();
			List<string> AllProducer = readDB.Get_Producer_All ();

			for (int i = 0; i < AllTypeCount.Count; i++) 
			{
				cb_TypeCounting.AppendText (AllTypeCount[i]);
			}

			for (int i = 0; i < AllProducer.Count; i++) 
			{
				cb_Producer.AppendText (AllProducer[i]);
			}

		}




		//ACTIONS

		protected void OnBnAddItemClicked (object sender, EventArgs e)
		{
			Validation validation=new Validation();

			if(!validation.IsEmpty(tb_CodeProduct.Text) && !validation.IsEmpty(tb_CodeProduct.Text) && !validation.IsEmpty(tb_Producer.Text) && 
				!validation.IsEmpty(tb_TypeCounting.Text) && !validation.IsEmpty(tb_PriceForOne.Text)&& !validation.IsEmpty(tb_ParametrProduct.Text)&& 
				!validation.IsEmpty(tb_ParametrValue.Text))
			{
				if (!validation.ExistInList (_ExistProduct, tb_CodeProduct.Text)) 
				{
					WriteTo_DB writer = new WriteTo_DB (nameDB,buf);

					StructOfProduct product = new StructOfProduct ();

					product.Number = tb_CodeProduct.Text;
					product.Name = tb_NameProduct.Text;
					product.Id_producer=writer.Get_Or_Set_IdOfProducer(tb_Producer.Text);
					product.Id_typeCount=writer.Get_Or_Set_IdOfTypeCount(tb_TypeCounting.Text);
					product.Parametr=tb_ParametrProduct.Text;
					product.ValueParametr = tb_ParametrValue.Text;
					product.PriceForOne = tb_PriceForOne.Text;

					if(writer.Write_newProduct(product))
						MessageBox.Show ("Товар успешно добавлен");
					else
						MessageBox.Show ("повторите попытку");
				} 
				else
					MessageBox.Show ("Товар с таким номером уже есть");
			}
			else
				MessageBox.Show ("Не все поля заполнены. Заполните и повторите попытку");
		}

		//COMBO BOX
		protected void OnCbProducerChanged (object sender, EventArgs e)
		{
			tb_Producer.Text = cb_Producer.ActiveText;
		}

		protected void OnCbTypeCountingChanged (object sender, EventArgs e)
		{
			tb_TypeCounting.Text = cb_TypeCounting.ActiveText;
		}






























	}
}

