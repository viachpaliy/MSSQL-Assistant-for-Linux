using System;
using Gtk;

namespace MSSQL_Assistant_for_Linux
{
	public partial class MsSqlAssistant : Gtk.Window
	{
		Toolbar toolbar;
		public ToolButton newConnectionBtn;
		public ToolButton updateConnectionBtn;
		public ToolButton closeConnectionBtn;
		
		public ToolButton newBtn;
		public ToolButton openBtn;
		public ToolButton saveBtn;
		public ToolButton saveAsBtn;
		public ToolButton closeBtn;


		public ToolButton copyBtn;
		public ToolButton pasteBtn;
		public ToolButton cutBtn;
		public ToolButton undoBtn;
		public ToolButton redoBtn;

		public ToolButton executeBtn;

		 void createAssistantToolbar ()
		{
			toolbar = new Toolbar ();
			newConnectionBtn = new ToolButton (Stock.Connect);
			newConnectionBtn.TooltipText="Open connection";
			updateConnectionBtn = new ToolButton (Stock.Refresh);
			updateConnectionBtn.TooltipText = "Update connection";
			closeConnectionBtn = new ToolButton (Stock.Disconnect);
			closeConnectionBtn.TooltipText = "Close connection";

			newBtn = new ToolButton (Stock.New);
			newBtn.TooltipText = "New query Ctrl+N";
			openBtn = new ToolButton (Stock.Open);
			openBtn.TooltipText = "Open query Ctrl+O";
			saveBtn = new ToolButton (Stock.Save);
			saveBtn.TooltipText = "Save query Ctrl+S";
			saveAsBtn = new ToolButton (Stock.SaveAs);
			saveAsBtn.TooltipText = "Save query as...";
			closeBtn = new ToolButton (Stock.Close);
			closeBtn.TooltipText = "Close query Ctrl+W";

			copyBtn = new ToolButton (Stock.Copy);
			copyBtn.TooltipText = "Copy Ctrl+C";
			pasteBtn = new ToolButton (Stock.Paste);
			pasteBtn.TooltipText = "Paste Ctpl+V";
			cutBtn = new ToolButton (Stock.Cut);
			cutBtn.TooltipText = "Cut Ctrl+X";
			undoBtn = new ToolButton (Stock.Undo);
			undoBtn.TooltipText = "Undo Ctrl+Z";
			redoBtn = new ToolButton (Stock.Redo);
			redoBtn.TooltipText = "Redo Ctrl+Y";

			executeBtn = new ToolButton (Stock.Execute);
			executeBtn.TooltipText = "Query execute";

			toolbar.Insert (newConnectionBtn, 0);
			toolbar.Insert (updateConnectionBtn, 1);
			toolbar.Insert (closeConnectionBtn, 2);
			toolbar.Insert (new SeparatorToolItem (), 3);
			toolbar.Insert (newBtn, 4);
			toolbar.Insert (openBtn, 5);
			toolbar.Insert (saveBtn, 6);
			toolbar.Insert (saveAsBtn, 7);
			toolbar.Insert (closeBtn, 8);
			toolbar.Insert (new SeparatorToolItem (), 9);
			toolbar.Insert (copyBtn, 10);
			toolbar.Insert (pasteBtn, 11);
			toolbar.Insert (cutBtn, 12);
			toolbar.Insert (undoBtn, 13);
			toolbar.Insert (redoBtn, 14);
			toolbar.Insert (new SeparatorToolItem (), 15);
			toolbar.Insert (executeBtn, 16);

			newConnectionBtn.Clicked += OnNewConnect;
			newConnectionBtn.Clicked += updateDBStructure;

			updateConnectionBtn.Clicked += OnUpdateConnect;
			updateConnectionBtn.Clicked += updateDBStructure;

			closeConnectionBtn.Clicked += OnCloseConnect;

			newBtn.Clicked += OnNew;

			openBtn.Clicked += OnOpen;
						
			saveBtn.Clicked += OnSave;

			saveAsBtn.Clicked += OnSaveAs;

			closeBtn.Clicked += OnClose;

			undoBtn.Clicked += OnUndo;

			redoBtn.Clicked += OnRedo;

			copyBtn.Clicked += OnCopy;

			cutBtn.Clicked += OnCut;

			pasteBtn.Clicked += OnPaste;

			executeBtn.Clicked += OnExecuteQuery;

		}
	}
}

