using System;
using Gtk;

namespace MSSQL_Assistant_for_Linux
{
	public class Programm
	{
		public static void Main()
		{
			Application.Init ();
			var programm = new MsSqlAssistant ("MSSQL Assistant for Linux");
			programm.DeleteEvent += OnDelete;

			programm.ShowAll ();

			programm.DeleteEvent += OnDelete;

			Application.Run ();
		}


		static void OnDelete (object o, DeleteEventArgs e)
		{
			Application.Quit ();
		}	
	}
}

