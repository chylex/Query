using System;
using System.Linq;
using AppConv.General;
using AppConv.Units;
using Base;

namespace AppConv;

public sealed class App : IApp {
	private static readonly IUnitType[] Processors = [
		new Temperature(),
		new Weight(),
		new Length(),
		new Area(),
		new Volume(),
		new Angle(),
		new Storage(),
		new Radix()
	];

	public string[] RecognizedNames => [
		"conv",
		"convert"
	];

	public MatchConfidence GetConfidence(Command cmd) {
		return cmd.Text.Contains(" to ", StringComparison.InvariantCultureIgnoreCase) ? MatchConfidence.Possible : MatchConfidence.None;
	}

	public string ProcessCommand(Command cmd) {
		string[] data = cmd.Text.Split([ " to " ], 2, StringSplitOptions.None);

		string src = data[0].Trim();
		string dst = data[1].Trim();

		if (src.Length == 0 || dst.Length == 0) {
			throw new CommandException("Unrecognized conversion app syntax.");
		}

		string result = string.Empty;
		IUnitType used = Processors.FirstOrDefault(processor => processor.TryProcess(src, dst, out result));

		if (used == null) {
			throw new CommandException("Could not recognize conversion units.");
		}

		return result;
	}
}
