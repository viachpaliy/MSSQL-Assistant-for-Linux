using System;
using Gtk;
using System.IO;
using System.Data;
using System.Data.SqlClient;

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
		Viewport viewer;
		public Table responseTable;

		DBStructure dataBasesStructure;
		QueryEditor queryEditor;
			
		TextTag keywordTag;
		TextTag namesTag;

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
			viewer = new Viewport ();
			responseTable = new Table (1,1,false);
			viewer.Add (responseTable);
			responseWindow.Add (viewer);
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




			 keywordTag = new TextTag ("keywordTag");
			keywordTag.Foreground = "blue";
			queryText.Buffer.TagTable.Add (keywordTag);
			namesTag = new TextTag ("namesTag");
			namesTag.Foreground = "green";
			queryText.Buffer.TagTable.Add (namesTag);

			queryText.Buffer.Changed+=queryEditor.OnTextChanged;
			queryText.Buffer.UserActionBegun += queryEditor.OnUserActionBegun;
			queryText.Buffer.UserActionBegun += highlightingSQLkeywords;
			queryText.Buffer.UserActionBegun += highlightingNames;

			toolbar.executeBtn.Clicked += OnExecuteQuery;

			ShowAll ();

		}

		void OnExecuteQuery(object o, EventArgs args)
		{
			string query;

			TextIter startIter;
			TextIter finishIter;

			if (queryText.Buffer.GetSelectionBounds (out startIter, out finishIter)) {
				query = queryText.Buffer.GetText (startIter, finishIter, true);
			} else {
				query = queryText.Buffer.Text;
			}
			if (cbCurrentDB.Active != -1) {
				int pos = query.ToLower ().IndexOf ("use " + cbCurrentDB.ActiveText.ToLower ());
				if (pos == -1)
					query = "USE " + cbCurrentDB.ActiveText + " " + query;
			}
			try{
			using (SqlConnection connection = new SqlConnection (dataBasesStructure.connectionString)) {
				SqlCommand command;
				SqlDataReader reader;

				 command = new SqlCommand(query, connection);
				connection.Open();
			   reader = command.ExecuteReader();
				viewer.Remove (responseTable);
				responseTable = new Table (1, 1, false);
				viewer.Add (responseTable);

				uint row = 1;
					try{
					while(reader.Read())
					{
						responseTable.Resize(row,(uint)reader.FieldCount);
						for (int i=0;i<reader.FieldCount;i++){
							responseTable.Attach(new Label(reader[i].ToString()),(uint)i,(uint)(i+1),row,row+1);
						
						}
						row++;

					}	
											
				}
				
				finally{
					reader.Close ();
				}
							
			}
			}
			catch(Exception e) {
				MessageDialog md = new MessageDialog (this,
					DialogFlags.DestroyWithParent,
					MessageType.Error,
					ButtonsType.Ok,
					e.Message);
				md.Run ();
				md.Destroy();
				responseTable.Attach (new Label (e.Message), 0, 2, 0, 2);
			}
		
			ShowAll();


		}

		void updateDBStructure(object o, EventArgs args)
		{
			
			tvDBStructure.Model = dataBasesStructure.structureStore;
			tvDBStructure.HeadersVisible = true;
			var column = tvDBStructure.GetColumn (0);
			if (column != null)
				tvDBStructure.RemoveColumn (column);
			tvDBStructure.AppendColumn ("Database structure", new CellRendererText (), "text", 0);


			var model = new Gtk.ListStore (typeof(string));
			for (int i=0;i<dataBasesStructure.dataBasesNames.Count;i++) {
				model.AppendValues(dataBasesStructure.dataBasesNames[i]);
			}
			cbCurrentDB.Model = model;
			if (cbCurrentDB.Active==-1) cbCurrentDB.Active=0;
			ShowAll ();
		}

		public void highlightingSQLkeywords(object sender,EventArgs args)
		{
			int pos, startpos;
			bool exit;

			string[] keywords=new string[]{ "create","select", "drop", "delete", "insert", "update", "truncate",
				"grant ","print","sp_executesql ","objects","declare","table","into","sqlcancel","sqlsetprop",
				"sqlexec","sqlcommit","revoke","rollback","sqlrollback","values","sqldisconnect","sqlconnect",
				"user","system_user","use","schema_name","schemata","information_schema","dbo","guest",
				"db_owner",	"db_","table","@@","Users","execute","sysname","sp_who","sysobjects","sp_",
				"sysprocesses ","sys","db_","is_","exec", "end", "xp_","; --", "/*", "*/", "alter",
				"begin", "cursor", "kill","--" ,"tabname","or","sys","for","from"};
			foreach (string item in keywords) {
				startpos = 0;
				exit = false;
				do {
					pos=queryText.Buffer.Text.ToLower().IndexOf(" "+item+" ",startpos);
					if (pos==-1){exit=true;}
					else{
						queryText.Buffer.ApplyTag(keywordTag,queryText.Buffer.GetIterAtOffset(pos+1),
							queryText.Buffer.GetIterAtOffset(pos+1+item.Length));
							startpos=pos+item.Length;
					}
					if (startpos>=queryText.Buffer.CharCount){exit=true;}	
				} while(!exit);
			}
		}


		public void highlightingNames(object sender,EventArgs args)
		{
			int pos, startpos;
			bool exit;
			//highlighting databases names
			foreach (string item in dataBasesStructure.dataBasesNames) {
				startpos = 0;
				exit = false;
				do {
					pos=queryText.Buffer.Text.IndexOf(item,startpos);
					if (pos==-1){exit=true;}
					else{
						queryText.Buffer.ApplyTag(namesTag,queryText.Buffer.GetIterAtOffset(pos),
							queryText.Buffer.GetIterAtOffset(pos+item.Length));
						startpos=pos+item.Length;
					}
					if (startpos>=queryText.Buffer.CharCount){exit=true;}	
				} while(!exit);
			}

			//highlighting tables names
			foreach (string item in dataBasesStructure.tablesNames) {
				startpos = 0;
				exit = false;
				do {
					pos=queryText.Buffer.Text.IndexOf(item,startpos);
					if (pos==-1){exit=true;}
					else{
						queryText.Buffer.ApplyTag(namesTag,queryText.Buffer.GetIterAtOffset(pos),
							queryText.Buffer.GetIterAtOffset(pos+item.Length));
						startpos=pos+item.Length;
					}
					if (startpos>=queryText.Buffer.CharCount){exit=true;}	
				} while(!exit);
			}


			//highlighting columnes names
			foreach (string item in dataBasesStructure.columnsNames) {
				startpos = 0;
				exit = false;
				do {
					pos=queryText.Buffer.Text.IndexOf(item,startpos);
					if (pos==-1){exit=true;}
					else{
						queryText.Buffer.ApplyTag(namesTag,queryText.Buffer.GetIterAtOffset(pos),
							queryText.Buffer.GetIterAtOffset(pos+item.Length));
						startpos=pos+item.Length;
					}
					if (startpos>=queryText.Buffer.CharCount){exit=true;}	
				} while(!exit);
			}

		}


	}
}

