using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Query.Apps;

sealed class KillProcessApp : IApp {
	public bool TryRun(string command, [NotNullWhen(true)] out string? output) {
		string[] args = command.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
		if (args is not ["kill", _, ..]) {
			output = null;
			return false;
		}
		
		int succeeded = 0, failed = 0;

		foreach (string processName in args[1..]) {
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

		var build = new StringBuilder();
		build.Append("Killed ").Append(succeeded).Append(" process").Append(succeeded == 1 ? "" : "es");

		if (failed > 0) {
			build.Append(", failed ").Append(failed);
		}

		output = build.Append('.').ToString();
		return true;
	}
}
