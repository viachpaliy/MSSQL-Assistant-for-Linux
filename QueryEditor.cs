using System;
using Gtk;
using System.Collections.Generic;
using Pango;

namespace MSSQL_Assistant_for_Linux
{
	public class QueryEditor
	{
		public TextBuffer textBuffer{ get; set;}
		public Window parent{ get; set;}
		bool isSaved;
		public Stack<string> undoStack;
		public Stack<string> redoStack;
		string clipboard;
		public string fileName;
		string[] keywords;
		string keywordsColor;

		public QueryEditor ()
		{
			undoStack = new Stack<string> ();
			redoStack = new Stack<string> ();


			keywords=new string[]{ "create", "drop", "delete", "insert", "update", "truncate",
				"grant ","print","sp_executesql ","objects","declare","table","into","sqlcancel","sqlsetprop",
				"sqlexec","sqlcommit","revoke","rollback","sqlrollback","values","sqldisconnect","sqlconnect",
				"user","system_user","use","schema_name","schemata","information_schema","dbo","guest",
				"db_owner",	"db_","table","@@","Users","execute","sysname","sp_who","sysobjects","sp_",
				"sysprocesses ","master","sys","db_","is_","exec", "end", "xp_","; --", "/*", "*/", "alter",
				"begin", "cursor", "kill","--" ,"tabname","or","sys"};
			keywordsColor = "maroon";
			fileName = "Untitled";

		}


		public void SingleKeywordsHighlighting(object sender,EventArgs args)
		{
			int pos, startpos;
			bool exit;
			TextTag keywordTag = new TextTag ("keywordTag");
			keywordTag.Foreground = keywordsColor;
			textBuffer.TagTable.Add (keywordTag);
			foreach (string item in keywords) {
				startpos = 0;
				exit = false;
				string tem = item.ToLower ();
				do {
					pos=textBuffer.Text.IndexOf(tem,startpos);
					if (pos==-1){exit=true;}
					else{
						textBuffer.ApplyTag(keywordTag,textBuffer.GetIterAtOffset(pos),
							textBuffer.GetIterAtOffset(pos+item.Length));
							startpos=pos+item.Length;
					}
					if (startpos>=textBuffer.CharCount){exit=true;}	
				} while(!exit);
			}
		}

		public void OnClose(object sender, EventArgs args)
		{
			if (!isSaved) {
				var dialog = new Dialog ();
				dialog.AddButton ("Close without save", ResponseType.No);
				dialog.AddButton (Stock.Cancel, ResponseType.Cancel);
				dialog.AddButton ("Save changes", ResponseType.Ok);
				var dialogLabel = new Label ("Save changes in file?");
				dialog.VBox.PackStart (dialogLabel, false, false, 0);
				dialog.ShowAll ();
				int response = dialog.Run ();
				if (response == (int)ResponseType.Ok) {
					System.IO.File.WriteAllText (fileName, textBuffer.Text);
				}

				dialog.Destroy ();
				textBuffer.Clear ();
			}
		}


		public void OnPaste(object sender, EventArgs args)
		{			
			textBuffer.InsertAtCursor (clipboard);
		}

		public void OnCut(object sender, EventArgs args)
		{
			TextIter startIter;
			TextIter finishIter;
			if (textBuffer.GetSelectionBounds (out startIter, out finishIter)) {
				clipboard = textBuffer.GetText (startIter, finishIter, true);

				undoStack.Push (textBuffer.Text);

				textBuffer.Delete (ref startIter, ref finishIter);
			}

		}

		public void OnCopy(object sender, EventArgs args)
		{
			TextIter startIter;
			TextIter finishIter;
			if (textBuffer.GetSelectionBounds (out startIter, out finishIter)) {
				clipboard = textBuffer.GetText (startIter, finishIter, true);

			}
		}

		public void OnRedo(object sender, EventArgs args)
		{
			undoStack.Push (textBuffer.Text);

			if (redoStack.Count>0)	textBuffer.Text = redoStack.Pop ();


		}

		public void OnUndo(object sender, EventArgs args)
		{
			redoStack.Push (textBuffer.Text);

			if (undoStack.Count>0)	textBuffer.Text = undoStack.Pop ();

		}

		public void OnUserActionBegun(object sender, EventArgs args)
		{
			undoStack.Push (textBuffer.Text);

		}

		public void OnTextChanged(object sender, EventArgs args)
		{
			isSaved = false;

		}


		public void OnNew(object sender, EventArgs args)
		{
			bool cancel = false;
			if (!isSaved && fileName!="Untitled")
			{
				var dialog = new Dialog ();
				dialog.AddButton ("Close without save", ResponseType.No);
				dialog.AddButton (Stock.Cancel, ResponseType.Cancel);
				dialog.AddButton ("Save changes", ResponseType.Ok);
				var dialogLabel = new Label ("Save changes in file?");
				dialog.VBox.PackStart (dialogLabel, false, false, 0);
				dialog.ShowAll ();
				int response = dialog.Run ();
				if (response == (int)ResponseType.Ok) {
					System.IO.File.WriteAllText (fileName, textBuffer.Text);
				}
				if (response == (int)ResponseType.Cancel) {
					cancel = true;}
				dialog.Destroy ();
			}
			if (!cancel) {
				textBuffer.Clear ();
				fileName =  "Untitled";
				parent.Title = fileName;
				isSaved = true;

				undoStack.Clear ();
			
				redoStack.Clear ();
			}
		}

		public void OnSave(object sender, EventArgs args)
		{
			if (fileName != "Untitled") {
				System.IO.File.WriteAllText (fileName, textBuffer.Text);
				isSaved = true;
			} else
				OnSaveAs (sender, args);


		}

		public void OnSaveAs(object sender, EventArgs args)
		{

			Gtk.FileChooserDialog filechooser =
				new Gtk.FileChooserDialog ("Save file",
					parent,
					FileChooserAction.Save);
			filechooser.AddButton(Stock.Cancel, ResponseType.Cancel);
			filechooser.AddButton(Stock.Save, ResponseType.Ok);

			if (filechooser.Run () == (int)ResponseType.Ok) {
				System.IO.File.WriteAllText (filechooser.Filename, textBuffer.Text);
				fileName=filechooser.Filename;
				parent.Title = fileName;
				isSaved = true;

			}

			filechooser.Destroy();


		}

		public void OnOpen (object sender, EventArgs args)
		{
			bool cancel = false;
			if (!isSaved && fileName!="Untitled") {
				var dialog = new Dialog ();
				dialog.AddButton ("Close without save", ResponseType.No);
				dialog.AddButton (Stock.Cancel, ResponseType.Cancel);
				dialog.AddButton ("Save changes", ResponseType.Ok);
				var dialogLabel = new Label ("Save changes in file?");
				dialog.VBox.PackStart (dialogLabel, false, false, 0);
				dialog.ShowAll ();
				int response = dialog.Run ();
				if (response == (int)ResponseType.Ok) {
					System.IO.File.WriteAllText (fileName, textBuffer.Text);
				}
				if (response == (int)ResponseType.Cancel) {
					cancel = true;
				}
				dialog.Destroy ();
			}

			if (!cancel) {

				Gtk.FileChooserDialog filechooser =
					new Gtk.FileChooserDialog ("Choose the file to open",
						parent,
						FileChooserAction.Open
					);
				filechooser.Visible = true;
				filechooser.AddButton (Stock.Cancel, ResponseType.Cancel);
				filechooser.AddButton (Stock.Open, ResponseType.Ok);

				if (filechooser.Run () == (int)ResponseType.Ok) {

					fileName = filechooser.Filename;
					textBuffer.Text = System.IO.File.ReadAllText (filechooser.Filename);

				}

				filechooser.Destroy ();
				parent.Title = fileName;
				isSaved = true;
			
				undoStack.Clear ();
			
				redoStack.Clear ();
			}

		}







	}
}

