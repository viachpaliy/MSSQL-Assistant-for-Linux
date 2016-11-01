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
				queryText.Buffer.Clear ();
			}
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

				undoStack.Push (queryText.Buffer.Text);

				queryText.Buffer.Delete (ref startIter, ref finishIter);
			}

		}

		public void OnCopy(object sender, EventArgs args)
		{
			TextIter startIter;
			TextIter finishIter;
			if (queryText.Buffer.GetSelectionBounds (out startIter, out finishIter)) {
				clipboard = queryText.Buffer.GetText (startIter, finishIter, true);

			}
		}

		public void OnRedo(object sender, EventArgs args)
		{
			undoStack.Push (queryText.Buffer.Text);

			if (redoStack.Count>0)	queryText.Buffer.Text = redoStack.Pop ();


		}

		public void OnUndo(object sender, EventArgs args)
		{
			redoStack.Push (queryText.Buffer.Text);

			if (undoStack.Count>0)	queryText.Buffer.Text = undoStack.Pop ();

		}

		public void OnUserActionBegun(object sender, EventArgs args)
		{
			undoStack.Push (queryText.Buffer.Text);

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

				undoStack.Clear ();
			
				redoStack.Clear ();
			}
		}

		public void OnSave(object sender, EventArgs args)
		{
			if (fileName != "Untitled") {
				System.IO.File.WriteAllText (fileName, queryText.Buffer.Text);
				isSaved = true;
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
			
				undoStack.Clear ();
			
				redoStack.Clear ();
			}

		}







	}
}

