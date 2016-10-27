using System;
using Gtk;
using System.IO;

namespace MSSQL_Assistant_for_Linux
{
	public class MainWindow : Gtk.Window
	{
		VBox vbox;
		public AssistantMenuBar menubar;
		public AssistantToolbar toolbar;
		HBox hbox;
		VBox lbox;
		VBox rbox;
		HBox hbCurrentDB;
		Label lCurrentDB;
		public ComboBox cbCurrentDB;
		public ScrolledWindow swDBStructure;
		public TreeView tvDBStructure;
		ScrolledWindow queryWindow;
		public TextView queryText;
		ScrolledWindow responseWindow;
		public TreeView responseTable;

		DBStructure dataBasesStructure;
		QueryEditor queryEditor;

		public MainWindow (string title):base(title)
		{
			DefaultHeight = 640;
			DefaultWidth = 1000;

			Gdk.Pixbuf icon = new Gdk.Pixbuf ("res/icona.png");
			this.Icon = icon;
			vbox = new VBox (false,1);
			menubar = new AssistantMenuBar ();
			vbox.PackStart (menubar, false, true, 0);
			toolbar = new AssistantToolbar ();
			vbox.PackStart (toolbar, false, true, 0);
			hbox = new HBox (false, 1);
			vbox.PackStart (hbox, true, true, 0);
			lbox = new VBox (false, 3);
			lbox.BorderWidth = 3;

			hbCurrentDB = new HBox (false, 1);
			lCurrentDB = new Label ("Current Database:");
			cbCurrentDB = ComboBox.NewText ();
			hbCurrentDB.PackStart (lCurrentDB, false, false, 0);
			hbCurrentDB.PackStart (cbCurrentDB, true, true, 0);
			lbox.PackStart (hbCurrentDB, false, false, 0);

			swDBStructure = new ScrolledWindow ();
			tvDBStructure = new TreeView ();
			swDBStructure.Add (tvDBStructure);
			lbox.WidthRequest = 300;
			lbox.PackStart (swDBStructure, true, true, 0);

			rbox = new VBox (false, 3);

			queryWindow = new ScrolledWindow ();
			queryText = new TextView ();
			queryWindow.Add (queryText);
			queryWindow.BorderWidth = 3;
			rbox.PackStart (queryWindow, true, true, 0);

			responseWindow = new ScrolledWindow ();
			responseTable = new TreeView ();
			responseWindow.Add (responseTable);
			responseWindow.BorderWidth = 3;
			rbox.PackStart (responseWindow, true, true, 0);

			hbox.PackStart (lbox, false, true, 0);
			hbox.PackStart (rbox, true, true, 0);

			vbox.PackStart (hbox, true, true, 0);
			Add (vbox);

		    dataBasesStructure = new DBStructure ();
		
			queryEditor = new QueryEditor ();
			queryEditor.parent = this;
			queryEditor.textBuffer = queryText.Buffer;


			menubar.newConnection.Activated += dataBasesStructure.OnNewConnect;
			menubar.newConnection.Activated += updateDBStructure;

			toolbar.newConnection.Clicked += dataBasesStructure.OnNewConnect;
			toolbar.newConnection.Clicked += updateDBStructure;

			menubar.updateConnection.Activated+=dataBasesStructure.OnUpdateConnect;
			menubar.updateConnection.Activated+=updateDBStructure;

			toolbar.updateConnection.Clicked += dataBasesStructure.OnUpdateConnect;
			toolbar.updateConnection.Clicked += updateDBStructure;

			menubar.newQuery.Activated += queryEditor.OnNew;
			toolbar.newBtn.Clicked += queryEditor.OnNew;

			menubar.open.Activated += queryEditor.OnOpen;
			toolbar.openBtn.Clicked += queryEditor.OnOpen;

			menubar.save.Activated += queryEditor.OnSave;
			toolbar.saveBtn.Clicked += queryEditor.OnSave;

			menubar.saveAs.Activated += queryEditor.OnSaveAs;
			toolbar.saveAsBtn.Clicked += queryEditor.OnSaveAs;

			menubar.close.Activated += queryEditor.OnClose;
			toolbar.closeBtn.Clicked += queryEditor.OnClose;

			menubar.undo.Activated += queryEditor.OnUndo;
			toolbar.undoBtn.Clicked += queryEditor.OnUndo;

			menubar.redo.Activated += queryEditor.OnRedo;
			toolbar.redoBtn.Clicked += queryEditor.OnRedo;

			menubar.copy.Activated += queryEditor.OnCopy;
			toolbar.copyBtn.Clicked += queryEditor.OnCopy;

			menubar.cut.Activated += queryEditor.OnCut;
			toolbar.cutBtn.Clicked += queryEditor.OnCut;

			menubar.paste.Activated += queryEditor.OnPaste;
			toolbar.pasteBtn.Clicked += queryEditor.OnPaste;

			queryText.Buffer.Changed+=queryEditor.OnTextChanged;
			queryText.Buffer.Changed += queryEditor.SingleKeywordsHighlighting;
			queryText.Buffer.UserActionBegun += queryEditor.OnUserActionBegun;

			ShowAll ();

		}

		void updateDBStructure(object o, EventArgs args)
		{
			
			tvDBStructure.Model = dataBasesStructure.structureStore;
			tvDBStructure.HeadersVisible = true;
			var column = tvDBStructure.GetColumn (0);
			if (column != null)
				tvDBStructure.RemoveColumn (column);
			tvDBStructure.AppendColumn ("Database", new CellRendererText (), "text", 0);


			var model = new Gtk.ListStore (typeof(string));
			for (int i=0;i<dataBasesStructure.dataBasesNames.Count;i++) {
				model.AppendValues(dataBasesStructure.dataBasesNames[i]);
			}
			cbCurrentDB.Model = model;

			ShowAll ();
		}



	}
}

