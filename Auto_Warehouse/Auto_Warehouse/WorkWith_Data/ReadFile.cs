using System;
using System.IO;

namespace Auto_Warehouse
{
	public class ReadFile
	{
		public ReadFile ()
		{
		}


		public string ReadFromFile(string nameFile)
		{
			string text ="Welcome in this programm";//= File.ReadAllText(nameFile + ".txt", System.Text.Encoding.Default);
			//StreamReader read = new StreamReader (nameFile + ".txt", System.Text.Encoding.UTF8);

			//string text = read.ReadToEnd ();

			return text;
		}

	}
}

