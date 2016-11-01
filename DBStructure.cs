using System;
using System.Collections.Generic;
using Gtk;
using System.Data;
using System.Data.SqlClient;

namespace MSSQL_Assistant_for_Linux
{
	public partial class MsSqlAssistant : Gtk.Window
	{
		SqlConnection connection;
		public string connectionString;
		public TreeStore structureStore;
		public List<string> dataBasesNames;
		public List<string> tablesNames;
		public List<string> columnsNames;

		private static Dialog dialog = null;
		private static Label dialogLabel = null;
		private static ConnectDialog connectDialog=null;

		public void createDBStructure ()
		{
			structureStore=new TreeStore(typeof(string));
			dataBasesNames = new List<string> ();
			tablesNames = new List<string> ();
			columnsNames = new List<string> ();
		}


		void OnCloseConnect(object o, EventArgs args)
		{
			connectionString = null;
			connection = null;
			structureStore.Clear ();
			dataBasesNames.Clear ();
			tablesNames.Clear ();
			columnsNames.Clear ();
			tvDBStructure.Model = structureStore;
			var column = tvDBStructure.GetColumn (0);
			if (column != null) {
				tvDBStructure.RemoveColumn (column);
			}
		}

		public void OnNewConnect(object o, EventArgs args)
		{
			getConnectionString ();
			if(!String.IsNullOrEmpty(connectionString) || !String.IsNullOrWhiteSpace(connectionString))
			{connection = new SqlConnection (connectionString);}
			getDataBasesStructure ();

		}

		public void OnUpdateConnect(object o, EventArgs args)
		{
			getDataBasesStructure ();
		}

		public void getDataBasesStructure()
		{
			structureStore.Clear ();
			dataBasesNames.Clear ();
			tablesNames.Clear ();
			columnsNames.Clear ();
			getDataBasesNames ();
			getTablesNames ();
			getColumnsNames ();
			dialog.Destroy ();
			dialog = null;

		}

		public bool CheckDialogFields(string[] entries)
		{
			bool ret = true;
			foreach (var item in entries) {
				ret = ret && !(String.IsNullOrEmpty (item) || String.IsNullOrWhiteSpace (item));
			}
			return ret;
		}

		public void getConnectionString()
		{
			if (connectDialog == null) {
				connectDialog = new ConnectDialog ();

			}
			bool exitFlag = false;
			do
			{ResponseType response = (ResponseType) connectDialog.Run(); 
				if (response == ResponseType.Cancel) { 
					exitFlag=true;
				}
				if (response == ResponseType.Ok) { 
					string[] strings = new string[] {(string)connectDialog.serverNameEntry.Text,
						(string)connectDialog.loginEntry.Text,
						(string)connectDialog.passwordEntry.Text	};
					if( CheckDialogFields(strings))
					{connectionString ="Data Source="+ strings [0]+";"+
							"User id="+strings[1]+";"+
							"Password="+strings[2]+";";
						exitFlag=true;
					}
				}
			}while(!exitFlag);
			connectDialog.Destroy ();
			connectDialog = null;
		
		}

		private void getDataBasesNames()
		{
			string queryString = "SELECT name FROM sys.databases;";
			try{
				if(!String.IsNullOrEmpty(connectionString) || !String.IsNullOrWhiteSpace(connectionString))
				{connection = new SqlConnection (connectionString);}
			using (connection )
			{
				
				SqlCommand command = new SqlCommand(
					queryString, connection);
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				try
				{
					while (reader.Read())
					{
						structureStore.AppendValues(reader.GetString(0));
						dataBasesNames.Add(reader.GetString(0));
					}
				}
				finally
				{
					// Always call Close when done reading.
					reader.Close();
				}
				
			}
			}
			catch(Exception e) {
				Dialog md = new Dialog ();
				md.Title = "Error-getDataBasesNames";	
				md.SetDefaultSize (200, 100);
				md.AddButton (Stock.Ok, ResponseType.Ok);
				Gtk.Image icon = new Gtk.Image (Stock.DialogError, IconSize.Dialog);
				Label message = new Label (e.Message);
				HBox hbox = new HBox (false,1);
				hbox.PackStart (message);
				hbox.PackStart (icon);
				md.VBox.PackStart (hbox);
				md.Run ();
				md.Destroy();
				viewer.Remove (responseTable);
				responseTable = new Table (1, 1, false);
				responseTable.Attach (new Label (e.Message), 0, 2, 0, 2);
			}
		}

		private void getTablesNames()
		{
			TreeIter iterator = new TreeIter ();
			structureStore.GetIterFirst (out iterator);
			do{
				findTablesNames ( iterator);
			}while(structureStore.IterNext(ref iterator));

		}


		private void findTablesNames(TreeIter parentIter)
		{
			string name =(string) structureStore.GetValue (parentIter, 0);
			string questTables=	"USE "+name+" SELECT name FROM sys.tables";
			try{
				if(!String.IsNullOrEmpty(connectionString) || !String.IsNullOrWhiteSpace(connectionString))
				{connection = new SqlConnection (connectionString);}
			using ( connection ) {


				SqlCommand command = new SqlCommand(questTables, connection);
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				try{
					while(reader.Read())
					{
						structureStore.AppendValues (parentIter,reader.GetString(0));
						tablesNames.Add(reader.GetString(0));
					}	

				}
				finally{
					reader.Close ();
				}
				
			}
			}
		catch(Exception e) {
			Dialog md = new Dialog ();
				md.Title = "Error-findTablesNames";	
			md.SetDefaultSize (250, 100);
			md.AddButton (Stock.Ok, ResponseType.Ok);
			Gtk.Image icon = new Gtk.Image (Stock.DialogError, IconSize.Dialog);
			Label message = new Label (e.Message);
			HBox hbox = new HBox (false,1);
			hbox.PackStart (message);
			hbox.PackStart (icon);
			md.VBox.PackStart (hbox);
			md.Run ();
			md.Destroy();
				viewer.Remove (responseTable);
				responseTable = new Table (1, 1, false);
				responseTable.Attach (new Label (e.Message), 0, 2, 0, 2);
		}
		}

		private void updateDialog (string format, params object[] args)
		{
			string text = String.Format (format, args);

			if (dialog == null)
			{
				dialog = new Dialog ();
				dialog.Title = "Loading data from databases...";

				dialog.SetDefaultSize (453, 174);

				VBox vbox = dialog.VBox;
			
				Gtk.Image icon=new Gtk.Image();
				Gdk.Pixbuf pic = new Gdk.Pixbuf("res/MSSQL.png");
				icon.Pixbuf = pic;
				vbox.PackStart (icon, false, false, 0);
				dialogLabel = new Label (text);
				vbox.PackStart (dialogLabel, false, false, 0);
				dialog.ShowAll ();
			} else {
				dialogLabel.Text = text;
				while (Application.EventsPending ())
					Application.RunIteration ();
			}
		}


	


		private void getColumnsNames()
		{
			TreeIter iterator = new TreeIter ();
			structureStore.GetIterFirst (out iterator);
			do{
				updateDialog ("Loading data from {0}", (string) structureStore.GetValue (iterator, 0));
				getAllColumnsNames(iterator);
			}while(structureStore.IterNext(ref iterator));

		}

		private void getAllColumnsNames(TreeIter parentIter)
		{

			if (structureStore.IterNChildren (parentIter)!=null||structureStore.IterNChildren (parentIter)!= 0) {
				int nChild = structureStore.IterNChildren (parentIter);
				for (int i = 0; i < nChild; i++) {
					TreeIter childiter = new TreeIter ();
					structureStore.IterNthChild (out childiter, parentIter, i);
					findColumnesNames (childiter,parentIter);
				}
			}

		}


	

		private void findColumnesNames(TreeIter Iter,TreeIter parentIter)
		{
			try{
				if(!String.IsNullOrEmpty(connectionString) || !String.IsNullOrWhiteSpace(connectionString))
				{connection = new SqlConnection (connectionString);}
			using ( connection ) {


				string name =(string) structureStore.GetValue (Iter, 0);
				string dbname=(string) structureStore.GetValue (parentIter, 0);

				string questTables = "USE " + dbname +
				                    " SELECT c.name , t.Name ," +
				                    " c.is_nullable ," +
				                    " ISNULL(i.is_primary_key, 0) " +
				                    " FROM sys.columns c INNER JOIN sys.types t ON c.user_type_id = t.user_type_id " +
				                   " LEFT OUTER JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id" +
				                    " LEFT OUTER JOIN sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id" +
				               		" WHERE c.object_id = OBJECT_ID('"+dbname +".dbo."+ name +"')";
					

				SqlCommand command = new SqlCommand(questTables, connection);
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
		
				try{
					while(reader.Read())
					{
						TreeIter it=structureStore.AppendValues (Iter,reader.GetString(0));
						structureStore.AppendValues(it,reader.GetString(1));
						if (reader.GetBoolean(2)){structureStore.AppendValues(it,"Not NULL");}
						if (reader.GetBoolean(3)){structureStore.AppendValues(it,"Primary Key");}

					}	

				}
				finally{
					reader.Close ();
				}
				
			}
			}

			catch(Exception e) {
				Dialog md = new Dialog ();
				md.Title = "Error-findColumnesNames";	
				md.SetDefaultSize (200, 100);
				md.AddButton (Stock.Ok, ResponseType.Ok);
				Gtk.Image icon = new Gtk.Image (Stock.DialogError, IconSize.Dialog);
				Label message = new Label (e.Message);
				HBox hbox = new HBox (false,1);
				hbox.PackStart (message);
				hbox.PackStart (icon);
				md.VBox.PackStart (hbox);
				md.Run ();
				md.Destroy();
				viewer.Remove (responseTable);
				responseTable = new Table (1, 1, false);
				responseTable.Attach (new Label (e.Message), 0, 2, 0, 2);
			}

		}





	}
}

