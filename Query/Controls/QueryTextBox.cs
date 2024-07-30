using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Base;
using Query.Core;

namespace Query.Controls;

sealed partial class QueryTextBox : UserControl {
	public event EventHandler<CommandEventArgs>? CommandRan;

	private CommandHistory history = null!;
	private Action<string> log = null!;

	public QueryTextBox() {
		InitializeComponent();
	}

	public void Setup(CommandHistory historyObj, Action<string> logFunc) {
		history = historyObj;
		log = logFunc;
	}

	private void OnCommandRan() {
		CommandRan?.Invoke(this, new CommandEventArgs(tb.Text));
	}

	private sealed class CustomTextBox : TextBox {
		private string lastInputStr = string.Empty;
		private int lastInputPos = 0;

		private bool doResetHistoryMemory;
		private bool lastArrowShift;
		private int historyOffset;

		public CustomTextBox() {
			TextChanged += CustomTextBox_TextChanged;
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			QueryTextBox input = (QueryTextBox) Parent!;
			CommandHistory history = input.history;

			Keys key = e.KeyCode;
			bool handled = false;

			switch (key) {
				case Keys.Enter:
					if (Text != string.Empty) {
						input.OnCommandRan();

						Text = string.Empty;
						doResetHistoryMemory = true;
						handled = true;
					}

					break;

				case Keys.Up:
					if (lastArrowShift != e.Shift) {
						lastArrowShift = e.Shift;
						historyOffset = 0;
					}

					--historyOffset;

					if (InsertFromHistory(e.Shift ? history.Results : history.Queries)) {
						++historyOffset;
					}

					handled = true;
					break;

				case Keys.Down:
					if (lastArrowShift != e.Shift) {
						lastArrowShift = e.Shift;
						historyOffset = 0;
					}

					++historyOffset;

					if (InsertFromHistory(e.Shift ? history.Results : history.Queries)) {
						--historyOffset;
					}

					handled = true;
					break;

				case Keys.C:
					if (e.Modifiers == Keys.Control) {
						if (SelectionLength == 0 && history.Results.Count > 0) {
							Clipboard.SetText(history.Results.Last(), TextDataFormat.UnicodeText);
							input.log("Copied to clipboard.");
							handled = true;
						}
					}

					break;
			}

			if (!handled && key != Keys.ControlKey && key != Keys.ShiftKey && key != Keys.Menu) {
				doResetHistoryMemory = true;
			}

			e.Handled = e.SuppressKeyPress = handled;
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			base.OnKeyUp(e);

			if (doResetHistoryMemory) {
				doResetHistoryMemory = false;
				ResetHistoryMemory();
			}
		}

		private void CustomTextBox_TextChanged(object? sender, EventArgs e) {
			ResetHistoryMemory();
		}

		// Management

		private void ResetHistoryMemory() {
			lastInputStr = Text;
			lastInputPos = SelectionStart;
			historyOffset = 0;
		}

		private bool InsertFromHistory(IList<string> collection) {
			if (collection.Count == 0) {
				return true;
			}

			int index = collection.Count + historyOffset;
			bool wasClamped = false;

			if (index < 0) {
				index = 0;
				wasClamped = true;
			}
			else if (index >= collection.Count) {
				index = collection.Count - 1;
				wasClamped = true;
			}

			TextChanged -= CustomTextBox_TextChanged;

			Text = lastInputStr.Insert(lastInputPos, collection[index]);
			SelectionStart = lastInputPos + collection[index].Length;
			SelectionLength = 0;

			TextChanged += CustomTextBox_TextChanged;
			return wasClamped;
		}
	}
}
