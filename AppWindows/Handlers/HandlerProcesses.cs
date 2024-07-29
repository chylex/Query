using System;
using System.Diagnostics;
using System.Text;
using Base;

namespace AppSys.Handlers;

sealed class HandlerProcesses : IHandler {
	public bool Matches(Command cmd) {
		return cmd.Text.StartsWith("kill ", StringComparison.InvariantCultureIgnoreCase);
	}

	public string Handle(Command cmd) {
		string[] processNames = cmd.Text["kill ".Length..].Split(',', ';');
		int succeeded = 0, failed = 0;

		foreach (string processName in processNames) {
			try {
				Process[] processes = Process.GetProcessesByName(processName.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase) ? processName[..^4] : processName);

				foreach (Process process in processes) {
					try {
						process.Kill();
						++succeeded;
					} catch {
						++failed;
					}

					process.Close();
				}
			} catch {
				++failed;
			}
		}

		if (succeeded == 0 && failed == 0 && (cmd.Text.Equals("kill me", StringComparison.InvariantCultureIgnoreCase) || cmd.Text.StartsWith("kill me ", StringComparison.InvariantCultureIgnoreCase) || cmd.Text.StartsWith("kill me,", StringComparison.InvariantCultureIgnoreCase))) {
			return "No.";
		}

		var build = new StringBuilder();
		build.Append("Killed ").Append(succeeded).Append(" process").Append(succeeded == 1 ? "" : "es");

		if (failed > 0) {
			build.Append(", failed ").Append(failed);
		}

		return build.Append('.').ToString();
	}
}
