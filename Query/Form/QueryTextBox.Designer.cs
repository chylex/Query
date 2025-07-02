namespace Query.Form {
	partial class QueryTextBox {
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

		#region Component Designer generated code
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			tb = new Query.Form.QueryTextBox.CustomTextBox();
			SuspendLayout();
			// 
			// tb
			// 
			tb.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
			tb.BackColor = System.Drawing.Color.FromArgb(((int) ((byte) 36)), ((int) ((byte) 36)), ((int) ((byte) 36)));
			tb.BorderStyle = System.Windows.Forms.BorderStyle.None;
			tb.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) 238));
			tb.ForeColor = System.Drawing.Color.FromArgb(((int) ((byte) 240)), ((int) ((byte) 240)), ((int) ((byte) 240)));
			tb.Location = new System.Drawing.Point(7, 9);
			tb.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
			tb.Name = "tb";
			tb.Size = new System.Drawing.Size(678, 28);
			tb.TabIndex = 0;
			// 
			// QueryTextBox
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.FromArgb(((int) ((byte) 36)), ((int) ((byte) 36)), ((int) ((byte) 36)));
			Controls.Add(tb);
			Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			Size = new System.Drawing.Size(692, 51);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Query.Form.QueryTextBox.CustomTextBox tb;
	}
}
