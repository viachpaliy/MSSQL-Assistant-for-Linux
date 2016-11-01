using System;
using Gtk;

namespace MSSQL_Assistant_for_Linux
{
	public partial class MsSqlAssistant : Gtk.Window
	{
		MenuBar menubar;
		MenuItem connectionMI;
		Menu connectionMenu;
		MenuItem queryMI;
		Menu queryMenu;
		MenuItem edit;
		Menu editMenu;

	public	MenuItem newConnectionMI;
	public	MenuItem updateConnectionMI;
	public	MenuItem closeConnectionMI;

		public MenuItem newQuery;
		public MenuItem open;
		public MenuItem save;
		public MenuItem saveAs;
		public MenuItem close;

		public MenuItem copy;
		public MenuItem paste;
		public MenuItem cut;
		public MenuItem undo;
		public MenuItem redo;
		
		 void createAssistantMenuBar ()
		{
			menubar = new MenuBar();
			connectionMI = new MenuItem ("Connection");
			connectionMenu=new Menu();
			connectionMI.Submenu = connectionMenu;
			newConnectionMI = new MenuItem ("Open connection");
			updateConnectionMI = new MenuItem ("Update");
			closeConnectionMI = new MenuItem ("Close connection");
			connectionMenu.Append (newConnectionMI);
			connectionMenu.Append (updateConnectionMI);
			connectionMenu.Append (closeConnectionMI);

			queryMI = new MenuItem ("Query");
			queryMenu = new Menu ();
			queryMI.Submenu = queryMenu;
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

			menubar.Append (connectionMI);
			menubar.Append (queryMI);
			menubar.Append (edit);

			newConnectionMI.Activated += OnNewConnect;
			newConnectionMI.Activated += updateDBStructure;

			updateConnectionMI.Activated += OnUpdateConnect;
			updateConnectionMI.Activated += updateDBStructure;

			closeConnectionMI.Activated += OnCloseConnect;

			newQuery.Activated +=OnNew;

			open.Activated += OnOpen;

			save.Activated += OnSave;

			saveAs.Activated += OnSaveAs;

			close.Activated += OnClose;

			undo.Activated += OnUndo;

			redo.Activated += OnRedo;

			copy.Activated += OnCopy;

			cut.Activated += OnCut;

			paste.Activated += OnPaste;

		}
	}
}

