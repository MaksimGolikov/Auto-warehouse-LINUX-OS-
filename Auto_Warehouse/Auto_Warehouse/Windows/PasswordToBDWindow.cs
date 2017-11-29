using System;

namespace Auto_Warehouse
{
	public partial class PasswordToBDWindow : Gtk.Window
	{

		Bufer buf;

		public PasswordToBDWindow (Bufer b) :
			base (Gtk.WindowType.Toplevel)
		{

			this.Build ();


			buf = b;
		}

		protected void OnBnEnterClicked (object sender, EventArgs e)
		{
			if (tb_password.Text != "" && tb_ServerName.Text!="") 
			{
				buf.Password = tb_password.Text;
				buf.ServerName = tb_ServerName.Text;

				this.Destroy ();
			}
		}

	   

	}
}

