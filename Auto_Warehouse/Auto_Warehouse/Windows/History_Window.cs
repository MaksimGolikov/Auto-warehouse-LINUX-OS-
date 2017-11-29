using System;
using Gtk;
using System.Collections.Generic;

namespace Auto_Warehouse
{
	public partial class History_Window : Gtk.Window
	{
		ListStore model = new ListStore (typeof(string),typeof(string),typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
		ReadDB readDB;
		Bufer buf;

		public History_Window (string nameDB,Bufer bufer) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();

			buf = bufer;
			Initialize (nameDB);
			ReadDataAboutDeal ();
		}




		private void Initialize(string nameDB)
		{
			readDB = new ReadDB (nameDB,buf);


			treeview_History.Model = model;

			treeview_History.AppendColumn ("     Номер      ",new CellRendererText(),"text",0);
			treeview_History.AppendColumn ("     Название      ",new CellRendererText(),"text",1);
			treeview_History.AppendColumn ("     Количество      ",new CellRendererText(),"text",2);
			treeview_History.AppendColumn ("  Еденица измерения   ",new CellRendererText(),"text",3);
			treeview_History.AppendColumn ("     Сумма      ",new CellRendererText(),"text",4);
			treeview_History.AppendColumn ("     Дата      ",new CellRendererText(),"text",5);
			treeview_History.AppendColumn ("     Тип докуммента      ",new CellRendererText(),"text",6);
			treeview_History.AppendColumn ("     Номер домекумента      ",new CellRendererText(),"text",7);
			treeview_History.AppendColumn ("     Инвентор      ",new CellRendererText(),"text",8);

		}


		private void ReadDataAboutDeal()
		{
			List<StructDataDeal> deals = readDB.Get_All_Deal ();


			for (int i = 0; i < deals.Count; i++)
			{
				model.AppendValues (deals[i].Number,deals[i].Name,deals[i].Count,deals[i].TypeCount,
					deals[i].Price,deals[i].DateWrote,deals[i].NameDoc,deals[i].NumberDoc,deals[i].Warhouser);
			}


		}



	}
}

