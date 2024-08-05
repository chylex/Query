using System;
using System.Collections.Generic;
using Query.Apps;

namespace Query.Command;

sealed class CommandProcessor {
	private readonly List<IApp> apps = [
		new MemeApp(),
		new KillProcessApp(),
		new CalculatorApp()
	];

	public string Run(string command) {
		try {
			foreach (IApp app in apps) {
				if (app.TryRun(command, out string? output)) {
					return output;
				}
			}

			return "Unknown command.";
		} catch (Exception e) {
			throw new CommandException(e.Message, e);
		}
	}
}
