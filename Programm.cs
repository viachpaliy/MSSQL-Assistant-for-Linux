using System;
using Gtk;

namespace MSSQL_Assistant_for_Linux
{
	public class Programm
	{
		public static void Main()
		{
			Application.Init ();
			var window = new MainWindow ("MSSQL Assistant for Linux");
			window.ShowAll ();

			window.DeleteEvent += OnDelete;
			Application.Run ();
		}
		static void OnDelete (object o, DeleteEventArgs e)
		{
			Application.Quit ();
		}	
	}
}

