using System;

namespace Auto_Warehouse
{
	public class Bufer
	{
		public Bufer ()
		{
		}

		private string password;
		public string Password
		{
			get{return password;}
			set{ password = value;}
		}

		private string serverName;
		public string  ServerName
		{
			get{return serverName;}
			set{ serverName = value;}
		}

	}
}

