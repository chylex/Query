using System;
using System.Drawing;
using System.Windows.Forms;
using Base;
using Query.Controls;
using Query.Core;

namespace Query;

sealed partial class MainForm : Form {
	private readonly CommandProcessor processor;
	private readonly CommandHistory history;

	private readonly Timer focusTimer;
	private readonly KeyboardHook keyboardHook;

	private bool isLoaded;

	public MainForm() {
		InitializeComponent();

		processor = new CommandProcessor {
			SingleTokenProcessor = ProcessSingleToken
		};

		processor.AddApp<AppCalc.App>();
		processor.AddApp<AppConv.App>();
		processor.AddApp<AppMeme.App>();
		processor.AddApp<AppSys.App>();

		history = new CommandHistory();
		queryBox.Setup(history, str => queryLog.AddEntry(str, QueryHistoryLog.EntryType.Information));

		keyboardHook = new KeyboardHook();
		keyboardHook.Triggered += keyboardHook_Triggered;

		focusTimer = new Timer {
			Interval = 1
		};

		focusTimer.Tick += focusTimer_Tick;

		Disposed += MainForm_Disposed;
		queryBox.CommandRan += queryBox_CommandRan;
	}

	private void SetShown(bool show) {
		if (show) {
			focusTimer.Start();
		}
		else {
			Hide();
		}
	}

	private void MainForm_Shown(object? sender, EventArgs e) {
		if (Screen.PrimaryScreen is {} primaryScreen) {
			Rectangle screenRect = primaryScreen.WorkingArea;
			Location = new Point(screenRect.X + screenRect.Width - Width, screenRect.Y + screenRect.Height - Height);
		}

		if (!isLoaded) {
			isLoaded = true;
			keyboardHook.StartHook();
		}
	}

	private void MainForm_Deactivate(object? sender, EventArgs e) {
		SetShown(false);
	}

	private void MainForm_Disposed(object? sender, EventArgs e) {
		keyboardHook.StopHook();
	}

	private void trayIcon_Click(object? sender, EventArgs e) {
		if (((MouseEventArgs) e).Button == MouseButtons.Left) {
			SetShown(true);
		}
	}

	private void showToolStripMenuItem_Click(object? sender, EventArgs e) {
		SetShown(true);
	}

	private void hookToolStripMenuItem_Click(object? sender, EventArgs e) {
		keyboardHook.StopHook();
		keyboardHook.StartHook();
	}

	private void exitToolStripMenuItem_Click(object? sender, EventArgs e) {
		Application.Exit();
	}

	private void keyboardHook_Triggered(object? sender, EventArgs e) {
		SetShown(!Visible);
	}

	private void focusTimer_Tick(object? sender, EventArgs e) {
		WindowState = FormWindowState.Minimized;
		Show();
		Activate();
		WindowState = FormWindowState.Normal;

		queryBox.Focus();
		focusTimer.Stop();
	}

	private void queryBox_CommandRan(object? sender, CommandEventArgs e) {
		try {
			string? result = processor.Run(e.Command);

			if (result != null) {
				queryLog.AddEntry("> " + e.Command.Text, QueryHistoryLog.EntryType.UserInput);
				history.AddQuery(e.Command.Text);

				queryLog.AddEntry(result, QueryHistoryLog.EntryType.CommandResult);
				history.AddResult(result);
			}
		} catch (CommandException ex) {
			queryLog.AddEntry("> " + e.Command.Text, QueryHistoryLog.EntryType.UserInput);
			history.AddQuery(e.Command.Text);

			queryLog.AddEntry(ex.Message, QueryHistoryLog.EntryType.Error);
		}
	}

	private bool ProcessSingleToken(string token) {
		switch (token) {
			case "exit":
			case "quit":
				Application.Exit();
				return true;

			case "clear":
				queryLog.ClearEntries();
				history.Clear();
				return true;

			case "hide":
				Hide();
				return true;

			default:
				return false;
		}
	}
}
