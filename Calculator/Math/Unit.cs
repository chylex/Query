using System.Collections.Immutable;

namespace Calculator.Math;

public sealed record Unit(string ShortName, ImmutableArray<string> LongNames) {
	public override string ToString() {
		return ShortName;
	}
}
