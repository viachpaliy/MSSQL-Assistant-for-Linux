using System;
using Gtk;

namespace MSSQL_Assistant_for_Linux
{
	public class AssistantToolbar:Toolbar
	{
		ToolButton newConnection;
		ToolButton updateConnection;
		ToolButton closeConnection;
		
		ToolButton newBtn;
		ToolButton openBtn;
		ToolButton saveBtn;
		ToolButton saveAsBtn;
		ToolButton closeBtn;


		ToolButton copyBtn;
		ToolButton pasteBtn;
		ToolButton cutBtn;
		ToolButton undoBtn;
		ToolButton redoBtn;

		public AssistantToolbar ()
		{
			newConnection = new ToolButton (Stock.Connect);
			newConnection.TooltipText="Open connection";
			updateConnection = new ToolButton (Stock.Refresh);
			updateConnection.TooltipText = "Update connection";
			closeConnection = new ToolButton (Stock.Quit);
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

			this.Insert (newConnection, 0);
			this.Insert (updateConnection, 1);
			this.Insert (closeConnection, 2);
			this.Insert (newBtn, 3);
			this.Insert (openBtn, 4);
			this.Insert (saveBtn, 5);
			this.Insert (saveAsBtn, 6);
			this.Insert (closeBtn, 7);
			this.Insert (copyBtn, 8);
			this.Insert (pasteBtn, 9);
			this.Insert (cutBtn, 10);
			this.Insert (undoBtn, 11);
			this.Insert (redoBtn, 12);
		}
	}
}

