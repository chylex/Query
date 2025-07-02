namespace Query.Form {
	sealed partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			queryLog = new Query.Form.QueryHistoryLog();
			queryBox = new Query.Form.QueryTextBox();
			trayIcon = new System.Windows.Forms.NotifyIcon(components);
			contextMenuTray = new System.Windows.Forms.ContextMenuStrip(components);
			showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			hookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			contextMenuTray.SuspendLayout();
			SuspendLayout();
			// 
			// queryLog
			// 
			queryLog.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
			queryLog.BackColor = System.Drawing.Color.Transparent;
			queryLog.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) 238));
			queryLog.Location = new System.Drawing.Point(7, 8);
			queryLog.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
			queryLog.Name = "queryLog";
			queryLog.Size = new System.Drawing.Size(696, 295);
			queryLog.TabIndex = 1;
			// 
			// queryBox
			// 
			queryBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
			queryBox.BackColor = System.Drawing.Color.FromArgb(((int) ((byte) 36)), ((int) ((byte) 36)), ((int) ((byte) 36)));
			queryBox.Location = new System.Drawing.Point(0, 311);
			queryBox.Margin = new System.Windows.Forms.Padding(0);
			queryBox.Name = "queryBox";
			queryBox.Size = new System.Drawing.Size(709, 51);
			queryBox.TabIndex = 0;
			// 
			// trayIcon
			// 
			trayIcon.ContextMenuStrip = contextMenuTray;
			trayIcon.Icon = ((System.Drawing.Icon) resources.GetObject("trayIcon.Icon"));
			trayIcon.Text = "Query";
			trayIcon.Visible = true;
			trayIcon.Click += trayIcon_Click;
			// 
			// contextMenuTray
			// 
			contextMenuTray.ImageScalingSize = new System.Drawing.Size(20, 20);
			contextMenuTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				showToolStripMenuItem, hookToolStripMenuItem, exitToolStripMenuItem
			});
			contextMenuTray.Name = "contextMenuTray";
			contextMenuTray.ShowImageMargin = false;
			contextMenuTray.ShowItemToolTips = false;
			contextMenuTray.Size = new System.Drawing.Size(90, 76);
			// 
			// showToolStripMenuItem
			// 
			showToolStripMenuItem.Name = "showToolStripMenuItem";
			showToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
			showToolStripMenuItem.Text = "Show";
			showToolStripMenuItem.Click += showToolStripMenuItem_Click;
			// 
			// hookToolStripMenuItem
			// 
			hookToolStripMenuItem.Name = "hookToolStripMenuItem";
			hookToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
			hookToolStripMenuItem.Text = "Hook";
			hookToolStripMenuItem.Click += hookToolStripMenuItem_Click;
			// 
			// exitToolStripMenuItem
			// 
			exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			exitToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
			exitToolStripMenuItem.Text = "Exit";
			exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.FromArgb(((int) ((byte) 64)), ((int) ((byte) 64)), ((int) ((byte) 64)));
			ClientSize = new System.Drawing.Size(709, 362);
			ControlBox = false;
			Controls.Add(queryLog);
			Controls.Add(queryBox);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			Icon = ((System.Drawing.Icon) resources.GetObject("$this.Icon"));
			Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			MaximizeBox = false;
			MinimizeBox = false;
			ShowInTaskbar = false;
			Text = "Query";
			Deactivate += MainForm_Deactivate;
			Shown += MainForm_Shown;
			contextMenuTray.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private Query.Form.QueryTextBox queryBox;
		private Query.Form.QueryHistoryLog queryLog;
		private System.Windows.Forms.NotifyIcon trayIcon;
		private System.Windows.Forms.ContextMenuStrip contextMenuTray;
		private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hookToolStripMenuItem;
	}
}
