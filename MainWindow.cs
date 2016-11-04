using System;
using Gtk;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace MSSQL_Assistant_for_Linux
{
	public partial class MsSqlAssistant : Gtk.Window
	{
		VBox vbox;
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
				
		TextTag keywordTag;
		TextTag namesTag;

		public MsSqlAssistant (string title):base(title)
		{
			DefaultHeight = 640;
			DefaultWidth = 1000;

			Gdk.Pixbuf icon = new Gdk.Pixbuf ("res/icona.png");
			this.Icon = icon;
			vbox = new VBox (false,1);

			createAssistantMenuBar();
			vbox.PackStart (menubar, false, true, 0);

			createAssistantToolbar();
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
		    
			createDBStructure();

			initQueryEditor();

			keywordTag = new TextTag ("keywordTag");
			keywordTag.Foreground = "blue";
			queryText.Buffer.TagTable.Add (keywordTag);
			namesTag = new TextTag ("namesTag");
			namesTag.Foreground = "green";
			queryText.Buffer.TagTable.Add (namesTag);

			queryText.Buffer.Changed+=OnTextChanged;
			queryText.Buffer.UserActionBegun += OnUserActionBegun;
			queryText.Buffer.UserActionBegun += highlightingSQLkeywords;
			queryText.Buffer.UserActionBegun += highlightingNames;

			updateConnectionMI.Sensitive = false;
			updateConnectionBtn.Sensitive = false;
			closeConnectionMI.Sensitive = false;
			closeConnectionBtn.Sensitive = false;

			fileName =  "Untitled";
			this.Title ="MSSQL Asistant - "+ fileName;
			isSaved = true;
			saveBtn.Sensitive = false;
			save.Sensitive = false;
			undoBtn.Sensitive = false;
			undo.Sensitive = false;
			undoStack.Clear ();
			redoBtn.Sensitive = false;
			redo.Sensitive = false;
			redoStack.Clear ();

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
//			if (cbCurrentDB.Active >0) {
//				int pos = query.ToLower ().IndexOf ("use " + cbCurrentDB.ActiveText.ToLower ());
//				if (pos == -1)
//					query = "USE " + cbCurrentDB.ActiveText + " " + query;
//			}
			try{
				if(!String.IsNullOrEmpty(connectionString) || !String.IsNullOrWhiteSpace(connectionString))
				{connection = new SqlConnection (connectionString);	}
			using ( connection ) {
				SqlCommand command;
				SqlDataReader reader;

				 command = new SqlCommand(query, connection);
				connection.Open();
					if (cbCurrentDB.Active !=-1) {connection.ChangeDatabase(cbCurrentDB.ActiveText);}
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
				viewer.Remove (responseTable);
				responseTable = new Table (1, 1, false);
				responseTable.Attach (new Label (e.Message), 0, 2, 0, 2);
			}
		
			ShowAll();


		}

		void updateDBStructure(object o, EventArgs args)
		{
			
			tvDBStructure.Model = structureStore;
			tvDBStructure.HeadersVisible = true;
			var column = tvDBStructure.GetColumn (0);
			if (column != null)
				tvDBStructure.RemoveColumn (column);
			tvDBStructure.AppendColumn ("Database structure", new CellRendererText (), "text", 0);


			var model = new Gtk.ListStore (typeof(string));
			//model.AppendValues ("not selected");
			for (int i=0;i<dataBasesNames.Count;i++) {
				model.AppendValues(dataBasesNames[i]);
			}
			cbCurrentDB.Model = model;
			if (cbCurrentDB.Active==-1) cbCurrentDB.Active=0;
			ShowAll ();
		}

		public void highlightingSQLkeywords(object sender,EventArgs args)
		{
			queryText.Buffer.RemoveAllTags (queryText.Buffer.StartIter, queryText.Buffer.EndIter);
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
					pos=queryText.Buffer.Text.ToLower().IndexOf(item,startpos);
					if (pos==-1){exit=true;}
					else{
							
						char[] startSymbols=new char[]{' ','{','(','}',')','\n','\t',','};
						bool startCondition= pos==0;
						foreach (char ch in startSymbols)
						{startCondition=startCondition ||(pos>0 && queryText.Buffer.Text[pos-1]==ch);	}

						char[] endSymbols=new char[]{' ','{','(','}',')','\n','\t',',','.'};
						bool endCondition= pos+item.Length==queryText.Buffer.CharCount;
						foreach (char ch in endSymbols)
						{endCondition=endCondition ||
								(pos+item.Length<queryText.Buffer.CharCount &&
									queryText.Buffer.Text[pos+item.Length]==ch);}
						
						if (startCondition && endCondition)
						{queryText.Buffer.ApplyTag(keywordTag,queryText.Buffer.GetIterAtOffset(pos),
							queryText.Buffer.GetIterAtOffset(pos+item.Length));}
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
			foreach (string item in dataBasesNames) {
				startpos = 0;
				exit = false;
				do {
					pos=queryText.Buffer.Text.IndexOf(item,startpos);
					if (pos==-1){exit=true;}
					else{
						char[] startSymbols=new char[]{' ','{','(','}',')','\n','\t',','};
						bool startCondition= pos==0;
						foreach (char ch in startSymbols)
						{startCondition=startCondition ||(pos>0 && queryText.Buffer.Text[pos-1]==ch);	}

						char[] endSymbols=new char[]{' ','{','(','}',')','\n','\t',',','.'};
						bool endCondition= pos+item.Length==queryText.Buffer.CharCount;
						foreach (char ch in endSymbols)
						{endCondition=endCondition ||
								(pos+item.Length<queryText.Buffer.CharCount &&
									queryText.Buffer.Text[pos+item.Length]==ch);}

						if (startCondition && endCondition)
						{queryText.Buffer.ApplyTag(namesTag,queryText.Buffer.GetIterAtOffset(pos),
							queryText.Buffer.GetIterAtOffset(pos+item.Length));}
						startpos=pos+item.Length;
					}
					if (startpos>=queryText.Buffer.CharCount){exit=true;}	
				} while(!exit);
			}

			//highlighting tables names
			foreach (string item in tablesNames) {
				startpos = 0;
				exit = false;
				do {
					pos=queryText.Buffer.Text.IndexOf(item,startpos);
					if (pos==-1){exit=true;}
					else{
						char[] startSymbols=new char[]{' ','{','(','}',')','\n','\t',',','.'};
						bool startCondition= pos==0;
						foreach (char ch in startSymbols)
						{startCondition=startCondition ||(pos>0 && queryText.Buffer.Text[pos-1]==ch);	}

						char[] endSymbols=new char[]{' ','{','(','}',')','\n','\t',',','.'};
						bool endCondition= pos+item.Length==queryText.Buffer.CharCount;
						foreach (char ch in endSymbols)
						{endCondition=endCondition ||
								(pos+item.Length<queryText.Buffer.CharCount &&
									queryText.Buffer.Text[pos+item.Length]==ch);}

						if (startCondition && endCondition)
						{queryText.Buffer.ApplyTag(namesTag,queryText.Buffer.GetIterAtOffset(pos),
							queryText.Buffer.GetIterAtOffset(pos+item.Length));}
						startpos=pos+item.Length;
					}
					if (startpos>=queryText.Buffer.CharCount){exit=true;}	
				} while(!exit);
			}


			//highlighting columnes names
			foreach (string item in columnsNames) {
				startpos = 0;
				exit = false;
				do {
					pos=queryText.Buffer.Text.IndexOf(item,startpos);
					if (pos==-1){exit=true;}
					else{
						char[] startSymbols=new char[]{' ','{','(','}',')','\n','\t',',','.'};
						bool startCondition= pos==0;
						foreach (char ch in startSymbols)
						{startCondition=startCondition ||(pos>0 && queryText.Buffer.Text[pos-1]==ch);	}

						char[] endSymbols=new char[]{' ','{','(','}',')','\n','\t',',','.'};
						bool endCondition= pos+item.Length==queryText.Buffer.CharCount;
						foreach (char ch in endSymbols)
						{endCondition=endCondition ||
								(pos+item.Length<queryText.Buffer.CharCount &&
									queryText.Buffer.Text[pos+item.Length]==ch);}

						if (startCondition && endCondition)
						{queryText.Buffer.ApplyTag(namesTag,queryText.Buffer.GetIterAtOffset(pos),
							queryText.Buffer.GetIterAtOffset(pos+item.Length));}
						startpos=pos+item.Length;
					}
					if (startpos>=queryText.Buffer.CharCount){exit=true;}	
				} while(!exit);
			}

		}


	}
}

