using System.Collections.Generic;
using Base;

namespace AppMeme {
	public sealed class App : IApp {
		private static readonly Dictionary<string, string> Map = new Dictionary<string, string> {
			{ "shrug", @"¯\_(ツ)_/¯" },
			{ "lenny", @"( ͡° ͜ʖ ͡°)" },
			{ "flip", @"(╯°□°）╯︵ ┻━┻" },
			{ "tableflip", @"(╯°□°）╯︵ ┻━┻" }
		};

		public string[] RecognizedNames => new string[] {
			"meme"
		};

		public MatchConfidence GetConfidence(Command cmd) {
			return Map.ContainsKey(cmd.Text) ? MatchConfidence.Full : MatchConfidence.None;
		}

		public string ProcessCommand(Command cmd) {
			return Map[cmd.Text];
		}
	}
}
