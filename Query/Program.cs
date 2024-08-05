using System;
using System.Windows.Forms;
using Query.Form;

namespace Query;

static class Program {
	[STAThread]
	private static void Main() {
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new MainForm());
	}
}
