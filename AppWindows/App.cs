using System.Linq;
using AppSys.Handlers;
using Base;

namespace AppSys {
	public sealed class App : IApp {
		private static readonly IHandler[] Handlers = {
			new HandlerProcesses(),
			new HandlerApps()
		};

		public string[] RecognizedNames => new string[] {
			"sys",
			"os",
			"win"
		};

		public MatchConfidence GetConfidence(Command cmd) {
			return Handlers.Any(handler => handler.Matches(cmd)) ? MatchConfidence.Full : MatchConfidence.None;
		}

		public string ProcessCommand(Command cmd) {
			return Handlers.First(handler => handler.Matches(cmd)).Handle(cmd);
		}
	}
}
