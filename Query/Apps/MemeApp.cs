using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Query.Apps;

sealed class MemeApp : IApp {
	private static readonly Dictionary<string, string> Map = new () {
		{ "shrug", @"¯\_(ツ)_/¯" },
		{ "lenny", @"( ͡° ͜ʖ ͡°)" },
		{ "flip", @"(╯°□°）╯︵ ┻━┻" },
		{ "tableflip", @"(╯°□°）╯︵ ┻━┻" }
	};

	public bool TryRun(string command, [NotNullWhen(true)] out string? output) {
		return Map.TryGetValue(command, out output);
	}
}
