using System;
using Gtk;

namespace MSSQL_Assistant_for_Linux
{
	public class MainWindow : Gtk.Window
	{
		VBox vbox;
		AssistantMenuBar menubar;
		AssistantToolbar toolbar;
		HBox hbox;
		VBox lbox;
		VBox rbox;
		HBox hbCurrentDB;
		Label lCurrentDB;
		ComboBox cbCurrentDB;
		ScrolledWindow swDBStructure;
		TreeView tvDBStructure;

		public MainWindow (string title):base(title)
		{
			DefaultHeight = 640;
			DefaultWidth = 1000;
			vbox = new VBox (false,1);
			menubar = new AssistantMenuBar ();
			vbox.PackStart (menubar, true, true, 0);
			toolbar = new AssistantToolbar ();
			vbox.PackStart (toolbar, true, true, 0);
			hbox = new HBox (false, 1);
			rbox = new VBox (false, 1);
			hbCurrentDB = new HBox (false, 1);
			lCurrentDB = new Label ("Current Database:");
			cbCurrentDB = new ComboBox ();
			swDBStructure = new ScrolledWindow ();
			tvDBStructure = new TreeView ();

			Add (vbox);
			ShowAll ();

		}

	}
}

