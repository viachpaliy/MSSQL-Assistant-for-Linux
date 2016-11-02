using System;
using Gtk;
using System.Collections.Generic;
using Pango;

namespace MSSQL_Assistant_for_Linux
{
	public partial class MsSqlAssistant : Gtk.Window
	{
		//public TextBuffer textBuffer{ get; set;}
		//public Window parent{ get; set;}
		bool isSaved;
		public Stack<string> undoStack;
		public Stack<string> redoStack;
		string clipboard;
		public string fileName;
	

		public void initQueryEditor ()
		{
			undoStack = new Stack<string> ();
			redoStack = new Stack<string> ();



			fileName = "Untitled";

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
					System.IO.File.WriteAllText (fileName, queryText.Buffer.Text);
				}

				dialog.Destroy ();
			}
			queryText.Buffer.Clear ();
		}


		public void OnPaste(object sender, EventArgs args)
		{			
			queryText.Buffer.InsertAtCursor (clipboard);
		}

		public void OnCut(object sender, EventArgs args)
		{
			TextIter startIter;
			TextIter finishIter;
			if (queryText.Buffer.GetSelectionBounds (out startIter, out finishIter)) {
				clipboard = queryText.Buffer.GetText (startIter, finishIter, true);
				if(pasteBtn.Sensitive==false)pasteBtn.Sensitive = true;
				if (paste.Sensitive == false) paste.Sensitive = true; 
				undoStack.Push (queryText.Buffer.Text);
				if (undoBtn.Sensitive == false)	undoBtn.Sensitive = true;
				if (undo.Sensitive == false) undo.Sensitive = true;
				queryText.Buffer.Delete (ref startIter, ref finishIter);
			}

		}

		public void OnCopy(object sender, EventArgs args)
		{
			TextIter startIter;
			TextIter finishIter;
			if (queryText.Buffer.GetSelectionBounds (out startIter, out finishIter)) {
				clipboard = queryText.Buffer.GetText (startIter, finishIter, true);
				if(pasteBtn.Sensitive==false)pasteBtn.Sensitive = true;
				if (paste.Sensitive == false) paste.Sensitive = true; 
			}
		}

		public void OnRedo(object sender, EventArgs args)
		{
			
			if (redoStack.Count > 0) {
				undoStack.Push (queryText.Buffer.Text);
				if (undoBtn.Sensitive == false)	undoBtn.Sensitive = true;
				if (undo.Sensitive == false) undo.Sensitive = true;
				queryText.Buffer.Text = redoStack.Pop ();
			}
			if (redoStack.Count == 0) {
				redoBtn.Sensitive = false;
				redo.Sensitive = false;
			}

		}

		public void OnUndo(object sender, EventArgs args)
		{
			
			if (undoStack.Count > 0) {
				redoStack.Push (queryText.Buffer.Text);
				if (redoBtn.Sensitive == false)	redoBtn.Sensitive = true;
				if (redo.Sensitive == false)	redo.Sensitive = true;
				queryText.Buffer.Text = undoStack.Pop ();
			}
			if (undoStack.Count == 0) {
				undoBtn.Sensitive = false;
				undo.Sensitive = false;
			}
		}

		public void OnUserActionBegun(object sender, EventArgs args)
		{
			undoStack.Push (queryText.Buffer.Text);
			if (undoBtn.Sensitive == false)	undoBtn.Sensitive = true;
			if (undo.Sensitive == false) undo.Sensitive = true;
		}

		public void OnTextChanged(object sender, EventArgs args)
		{
			isSaved = false;
			if (saveBtn.Sensitive==false) saveBtn.Sensitive = true;
			if (save.Sensitive==false) save.Sensitive = true;
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
					System.IO.File.WriteAllText (fileName, queryText.Buffer.Text);
				}
				if (response == (int)ResponseType.Cancel) {
					cancel = true;}
				dialog.Destroy ();
			}
			if (!cancel) {
				queryText.Buffer.Clear ();
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
			}
		}

		public void OnSave(object sender, EventArgs args)
		{
			if (fileName != "Untitled") {
				System.IO.File.WriteAllText (fileName, queryText.Buffer.Text);
				isSaved = true;
				saveBtn.Sensitive = false;
				save.Sensitive = false;
			} else
				OnSaveAs (sender, args);


		}

		public void OnSaveAs(object sender, EventArgs args)
		{

			Gtk.FileChooserDialog filechooser =
				new Gtk.FileChooserDialog ("Save file",
					this,
					FileChooserAction.Save);
			filechooser.AddButton(Stock.Cancel, ResponseType.Cancel);
			filechooser.AddButton(Stock.Save, ResponseType.Ok);

			if (filechooser.Run () == (int)ResponseType.Ok) {
				System.IO.File.WriteAllText (filechooser.Filename, queryText.Buffer.Text);
				fileName=filechooser.Filename;
				this.Title ="MSSQL Asistant - " + fileName;
				isSaved = true;
				saveBtn.Sensitive = false;
				save.Sensitive = false;
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
					System.IO.File.WriteAllText (fileName, queryText.Buffer.Text);
				}
				if (response == (int)ResponseType.Cancel) {
					cancel = true;
				}
				dialog.Destroy ();
			}

			if (!cancel) {

				Gtk.FileChooserDialog filechooser =
					new Gtk.FileChooserDialog ("Choose the file to open",
						this,
						FileChooserAction.Open
					);
				filechooser.Visible = true;
				filechooser.AddButton (Stock.Cancel, ResponseType.Cancel);
				filechooser.AddButton (Stock.Open, ResponseType.Ok);

				if (filechooser.Run () == (int)ResponseType.Ok) {

					fileName = filechooser.Filename;
					queryText.Buffer.Text = System.IO.File.ReadAllText (filechooser.Filename);

				}

				filechooser.Destroy ();
				this.Title ="MSSQL Assistant - " + fileName;
				isSaved = true;
				saveBtn.Sensitive = false;
				save.Sensitive = false;
				undoBtn.Sensitive = false;
				undo.Sensitive = false;
				undoStack.Clear ();
				redoBtn.Sensitive = false;
				redo.Sensitive = false;
				redoStack.Clear ();
			}

		}







	}
}

