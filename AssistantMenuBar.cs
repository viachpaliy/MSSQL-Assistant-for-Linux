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

		MenuItem newConnectionMI;
		MenuItem updateConnectionMI;
		MenuItem closeConnectionMI;

		MenuItem newQuery;
		MenuItem open;
		MenuItem save;
		MenuItem saveAs;
		MenuItem close;
		MenuItem executeQuery;

		MenuItem copy;
		MenuItem paste;
		MenuItem cut;
		MenuItem undo;
		MenuItem redo;

		AccelGroup accelGroup;
		
		 void createAssistantMenuBar ()
		{
			accelGroup = new AccelGroup ();
			AddAccelGroup (accelGroup);
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
			executeQuery = new MenuItem ("Execute query");
			queryMenu.Append (newQuery);
			queryMenu.Append (open);
			queryMenu.Append (save);
			queryMenu.Append (saveAs);
			queryMenu.Append (new SeparatorMenuItem ());
			queryMenu.Append (executeQuery);
			queryMenu.Append (new SeparatorMenuItem ());
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
			newQuery.AddAccelerator("activate",accelGroup,
				new AccelKey(Gdk.Key.N, Gdk.ModifierType.ControlMask,
					AccelFlags.Visible));

			open.Activated += OnOpen;
			open.AddAccelerator("activate",accelGroup,
				new AccelKey(Gdk.Key.O, Gdk.ModifierType.ControlMask,
					AccelFlags.Visible));

			save.Activated += OnSave;
			save.AddAccelerator("activate",accelGroup,
				new AccelKey(Gdk.Key.S, Gdk.ModifierType.ControlMask,
					AccelFlags.Visible));

			saveAs.Activated += OnSaveAs;

			close.Activated += OnClose;
			close.AddAccelerator("activate",accelGroup,
				new AccelKey(Gdk.Key.W, Gdk.ModifierType.ControlMask,
					AccelFlags.Visible));

			undo.Activated += OnUndo;
			undo.AddAccelerator("activate",accelGroup,
				new AccelKey(Gdk.Key.Z, Gdk.ModifierType.ControlMask,
					AccelFlags.Visible));

			redo.Activated += OnRedo;
			redo.AddAccelerator("activate",accelGroup,
				new AccelKey(Gdk.Key.Y, Gdk.ModifierType.ControlMask,
					AccelFlags.Visible));

			copy.Activated += OnCopy;
			copy.AddAccelerator("activate",accelGroup,
				new AccelKey(Gdk.Key.C, Gdk.ModifierType.ControlMask,
					AccelFlags.Visible));

			cut.Activated += OnCut;
			cut.AddAccelerator("activate",accelGroup,
				new AccelKey(Gdk.Key.X, Gdk.ModifierType.ControlMask,
					AccelFlags.Visible));

			paste.Activated += OnPaste;
			paste.AddAccelerator("activate",accelGroup,
				new AccelKey(Gdk.Key.V, Gdk.ModifierType.ControlMask,
					AccelFlags.Visible));

			executeQuery.Activated += OnExecuteQuery;
		}
	}
}

