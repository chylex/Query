using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Base;

namespace AppSys.Handlers {
	class HandlerApps : IHandler {
		private static readonly string PathSystem = Environment.GetFolderPath(Environment.SpecialFolder.System);

		private static readonly Dictionary<string, ProcessStartInfo> Mappings = new Dictionary<string, ProcessStartInfo> {
			{
				"audio", new ProcessStartInfo {
					FileName = Path.Combine(PathSystem, "control.exe"),
					Arguments = "mmsys.cpl"
				}
			}, {
				"programs", new ProcessStartInfo {
					FileName = Path.Combine(PathSystem, "control.exe"),
					Arguments = "appwiz.cpl"
				}
			}, {
				"system", new ProcessStartInfo {
					FileName = Path.Combine(PathSystem, "control.exe"),
					Arguments = "sysdm.cpl"
				}
			}, {
				"environment", new ProcessStartInfo {
					FileName = Path.Combine(PathSystem, "rundll32.exe"),
					Arguments = "sysdm.cpl,EditEnvironmentVariables"
				}
			}
		};

		private static readonly Dictionary<string, string> Substitutions = new Dictionary<string, string> {
			{ "sounds", "audio" },
			{ "apps", "programs" },
			{ "appwiz", "programs" },
			{ "env", "environment" },
			{ "envvars", "environment" },
			{ "vars", "environment" },
			{ "variables", "environment" }
		};

		public bool Matches(Command cmd) {
			return Mappings.ContainsKey(cmd.Text) || Substitutions.ContainsKey(cmd.Text);
		}

		public string Handle(Command cmd) {
			string key;

			if (!Substitutions.TryGetValue(cmd.Text, out key)) {
				key = cmd.Text;
			}

			using (Process.Start(Mappings[key])) {}

			return null;
		}
	}
}
