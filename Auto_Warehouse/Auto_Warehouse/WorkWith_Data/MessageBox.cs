using System;
using Gtk;

namespace Auto_Warehouse
{
	public static class MessageBox
	{
		public static void Show (Gtk.Window parrent_window, DialogFlags flag, MessageType msgtype,
			ButtonsType btntype,string msg)
		{
			MessageDialog md = new MessageDialog (parrent_window,flag,msgtype,btntype,msg);
			md.Run ();
			md.Destroy ();
		}

		public static void Show(string msg)
		{
			MessageDialog md = new MessageDialog (null,DialogFlags.Modal,MessageType.Info,ButtonsType.Ok,msg);
			md.Run ();
			md.Destroy ();
		}
	}
}

