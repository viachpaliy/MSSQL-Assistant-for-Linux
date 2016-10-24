using System;
using Gtk;

namespace MSSQL_Assistant_for_Linux
{
	public class ConnectDialog:Dialog
	{
		public	Label serverNameLabel;
		public	Label loginLabel;
		public	Label passwordLabel;
		public	Entry serverNameEntry;
		public	Entry loginEntry;
		public	Entry passwordEntry;

		public ConnectDialog ()
		{
			this.Title = "Connect to Server";
			this.AddButton (Stock.Connect, ResponseType.Ok);
			this.AddButton (Stock.Cancel, ResponseType.Cancel);
			VBox vbox =this.VBox;
			HBox hbox = new HBox (false, 1);
			VBox leftBox = new VBox (true, 1);
			VBox rightBox = new VBox (true, 1);
			serverNameLabel = new Label ("Server name:");
			loginLabel = new Label ("Login:");
			passwordLabel = new Label ("Password:");
			serverNameEntry = new Entry ();
			loginEntry = new Entry ();
			passwordEntry = new Entry ();
			leftBox.PackStart (serverNameLabel, false, false, 0);
			leftBox.PackStart (loginLabel, false, false, 0);
			leftBox.PackStart (passwordLabel, false, false, 0);
			rightBox.PackStart (serverNameEntry, false, false, 0);
			rightBox.PackStart (loginEntry, false, false, 0);
			rightBox.PackStart (passwordEntry, false, false, 0);
			this.SetDefaultSize (453, 174);
			hbox.PackStart (leftBox, true, true, 0);
			hbox.PackStart (rightBox, true, true, 0);
			Gtk.Image icon=new Gtk.Image();
			Gdk.Pixbuf pic = new Gdk.Pixbuf("/home/viachpaliy/MSSQL_Assistant_for_Linux/MSSQL_Assistant_for_Linux/res/MSSQL.png");
			icon.Pixbuf = pic;
			vbox.PackStart (icon, false, false, 0);
			vbox.PackStart(hbox,true, true, 0);

			this.ShowAll ();
		}
	}
}

