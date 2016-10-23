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
		ScrolledWindow queryWindow;
		TextView queryText;
		ScrolledWindow responseWindow;
		TreeView responseTable;

		public MainWindow (string title):base(title)
		{
			DefaultHeight = 640;
			DefaultWidth = 1000;
			vbox = new VBox (false,1);
			menubar = new AssistantMenuBar ();
			vbox.PackStart (menubar, false, true, 0);
			toolbar = new AssistantToolbar ();
			vbox.PackStart (toolbar, false, true, 0);
			hbox = new HBox (false, 1);
			vbox.PackStart (hbox, true, true, 0);
			lbox = new VBox (false, 1);

			hbCurrentDB = new HBox (false, 1);
			lCurrentDB = new Label ("Current Database:");
			cbCurrentDB = new ComboBox ();
			hbCurrentDB.PackStart (lCurrentDB, false, false, 0);
			hbCurrentDB.PackStart (cbCurrentDB, true, true, 0);
			lbox.PackStart (hbCurrentDB, false, false, 0);

			swDBStructure = new ScrolledWindow ();
			tvDBStructure = new TreeView ();
			swDBStructure.Add (tvDBStructure);
			lbox.PackStart (swDBStructure, true, true, 0);

			rbox = new VBox (false, 1);
			queryWindow = new ScrolledWindow ();
			queryText = new TextView ();
			queryWindow.Add (queryText);
			rbox.PackStart (queryWindow, true, true, 0);

			responseWindow = new ScrolledWindow ();
			responseTable = new TreeView ();
			responseWindow.Add (responseTable);
			rbox.PackStart (responseWindow, true, true, 0);

			hbox.PackStart (lbox, false, true, 0);
			hbox.PackStart (rbox, true, true, 0);

			vbox.PackStart (hbox, true, true, 0);
			Add (vbox);
			ShowAll ();

		}

	}
}

