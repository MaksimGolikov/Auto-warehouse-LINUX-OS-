using System;

namespace Auto_Warehouse
{
	public partial class Help_Window : Gtk.Window
	{
		public Help_Window () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();

			ReadhelpFile ();
		}


		private void ReadhelpFile()
		{
			ReadFile read = new ReadFile ();

			textview_Help.Buffer.Text = read.ReadFromFile ("help");
		}
	}
}

