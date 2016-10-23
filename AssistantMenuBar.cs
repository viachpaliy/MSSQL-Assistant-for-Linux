using System;
using Gtk;

namespace MSSQL_Assistant_for_Linux
{
	public class AssistantMenuBar:MenuBar
	{
		MenuItem connection;
		Menu connectionMenu;
		MenuItem query;
		Menu queryMenu;
		MenuItem edit;
		Menu editMenu;

		MenuItem newConnection;
		MenuItem updateConnection;
		MenuItem closeConnection;

		MenuItem newQuery;
		MenuItem open;
		MenuItem save;
		MenuItem saveAs;
		MenuItem close;

		MenuItem copy;
		MenuItem paste;
		MenuItem cut;
		MenuItem undo;
		MenuItem redo;
		
		public AssistantMenuBar ():base()
		{
			connection = new MenuItem ("Connection");
			connectionMenu=new Menu();
			connection.Submenu = connectionMenu;
			newConnection = new MenuItem ("Open connection");
			updateConnection = new MenuItem ("Update");
			closeConnection = new MenuItem ("Close connection");
			connectionMenu.Append (newConnection);
			connectionMenu.Append (updateConnection);
			connectionMenu.Append (closeConnection);

			query = new MenuItem ("Query");
			queryMenu = new Menu ();
			query.Submenu = queryMenu;
			newQuery = new MenuItem ("New query");
			open = new MenuItem ("Open query");
			save = new MenuItem ("Save query");
			saveAs = new MenuItem ("Save file as ...");
			close = new MenuItem ("Close");
			queryMenu.Append (newQuery);
			queryMenu.Append (open);
			queryMenu.Append (save);
			queryMenu.Append (saveAs);
			queryMenu.Append (close);

			edit = new MenuItem ("Edit");
			editMenu = new Menu ();
			edit.Submenu = editMenu;
			copy = new MenuItem ("Copy");
			paste = new MenuItem ("Paste");
			cut = new MenuItem ("Cut");
			undo = new MenuItem ("Undo");
			redo = new MenuItem ("Redo");
			editMenu.Append (copy);
			editMenu.Append (paste);
			editMenu.Append (cut);
			editMenu.Append (undo);
			editMenu.Append (redo);

			this.Append (connection);
			this.Append (query);
			this.Append (edit);
		}
	}
}

