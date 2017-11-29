using System;

namespace Auto_Warehouse
{
	public partial class Parametr_Window : Gtk.Window
	{
		public Parametr_Window (StructOfProduct person) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();

			Initialize (person);
		}


		private void Initialize(StructOfProduct person)
		{						
			tb_Code.Text = person.Number;
			tb_Name.Text = person.Name;
			tb_Producer.Text = person.Produser;
			tb_Parametr.Text = person.Parametr;
			tb_ParametrValue.Text = person.ValueParametr;
			tb_TypeCount.Text = person.TypeCount;
			tb_Count.Text = person.Count.ToString ();
			tb_PriceForOne.Text = person.PriceForOne;
		}



	}
}

