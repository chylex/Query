namespace Query {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.queryLog = new Query.Controls.QueryHistoryLog();
            this.queryBox = new Query.Controls.QueryTextBox();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // queryLog
            // 
            this.queryLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queryLog.BackColor = System.Drawing.Color.Transparent;
            this.queryLog.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.queryLog.Location = new System.Drawing.Point(5, 5);
            this.queryLog.Margin = new System.Windows.Forms.Padding(5);
            this.queryLog.Name = "queryLog";
            this.queryLog.Size = new System.Drawing.Size(522, 192);
            this.queryLog.TabIndex = 1;
            // 
            // queryBox
            // 
            this.queryBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queryBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.queryBox.Location = new System.Drawing.Point(0, 202);
            this.queryBox.Margin = new System.Windows.Forms.Padding(0);
            this.queryBox.Name = "queryBox";
            this.queryBox.Size = new System.Drawing.Size(532, 33);
            this.queryBox.TabIndex = 0;
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.contextMenuTray;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Query";
            this.trayIcon.Visible = true;
            this.trayIcon.Click += new System.EventHandler(this.trayIcon_Click);
            // 
            // contextMenuTray
            // 
            this.contextMenuTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.hookToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuTray.Name = "contextMenuTray";
            this.contextMenuTray.ShowImageMargin = false;
            this.contextMenuTray.ShowItemToolTips = false;
            this.contextMenuTray.Size = new System.Drawing.Size(128, 92);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // hookToolStripMenuItem
            // 
            this.hookToolStripMenuItem.Name = "hookToolStripMenuItem";
            this.hookToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.hookToolStripMenuItem.Text = "Hook";
            this.hookToolStripMenuItem.Click += new System.EventHandler(this.hookToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(532, 235);
            this.ControlBox = false;
            this.Controls.Add(this.queryLog);
            this.Controls.Add(this.queryBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Query";
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.contextMenuTray.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Query.Controls.QueryTextBox queryBox;
        private Controls.QueryHistoryLog queryLog;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuTray;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hookToolStripMenuItem;
    }
}