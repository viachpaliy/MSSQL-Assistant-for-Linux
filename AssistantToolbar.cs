using System;
using Gtk;

namespace MSSQL_Assistant_for_Linux
{
	public class AssistantToolbar:Toolbar
	{
		public ToolButton newConnection;
		public ToolButton updateConnection;
		public ToolButton closeConnection;
		
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

		public AssistantToolbar ()
		{
			newConnection = new ToolButton (Stock.Connect);
			newConnection.TooltipText="Open connection";
			updateConnection = new ToolButton (Stock.Refresh);
			updateConnection.TooltipText = "Update connection";
			closeConnection = new ToolButton (Stock.Disconnect);
			closeConnection.TooltipText = "Close connection";

			newBtn = new ToolButton (Stock.New);
			newBtn.TooltipText = "New query";
			openBtn = new ToolButton (Stock.Open);
			openBtn.TooltipText = "Open query";
			saveBtn = new ToolButton (Stock.Save);
			saveBtn.TooltipText = "Save query";
			saveAsBtn = new ToolButton (Stock.SaveAs);
			saveAsBtn.TooltipText = "Save query as...";
			closeBtn = new ToolButton (Stock.Close);
			closeBtn.TooltipText = "Close query";

			copyBtn = new ToolButton (Stock.Copy);
			copyBtn.TooltipText = "Copy";
			pasteBtn = new ToolButton (Stock.Paste);
			pasteBtn.TooltipText = "Paste";
			cutBtn = new ToolButton (Stock.Cut);
			cutBtn.TooltipText = "Cut";
			undoBtn = new ToolButton (Stock.Undo);
			undoBtn.TooltipText = "Undo";
			redoBtn = new ToolButton (Stock.Redo);
			redoBtn.TooltipText = "Redo";

			executeBtn = new ToolButton (Stock.Execute);
			executeBtn.TooltipText = "Query execute";

			this.Insert (newConnection, 0);
			this.Insert (updateConnection, 1);
			this.Insert (closeConnection, 2);
			this.Insert (new SeparatorToolItem (), 3);
			this.Insert (newBtn, 4);
			this.Insert (openBtn, 5);
			this.Insert (saveBtn, 6);
			this.Insert (saveAsBtn, 7);
			this.Insert (closeBtn, 8);
			this.Insert (new SeparatorToolItem (), 9);
			this.Insert (copyBtn, 10);
			this.Insert (pasteBtn, 11);
			this.Insert (cutBtn, 12);
			this.Insert (undoBtn, 13);
			this.Insert (redoBtn, 14);
			this.Insert (new SeparatorToolItem (), 15);
			this.Insert (executeBtn, 16);
		}
	}
}

