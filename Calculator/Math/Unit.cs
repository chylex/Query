using System.Collections.Generic;
using System.Collections.Immutable;

namespace Calculator.Math;

public sealed record Unit(string ShortName, ImmutableArray<string> LongNames) {
	internal void AssignNamesTo(Dictionary<string, Unit> nameToUnitDictionary) {
		nameToUnitDictionary.Add(ShortName, this);

		foreach (string longName in LongNames) {
			nameToUnitDictionary.Add(longName, this);
		}
	}

	public override string ToString() {
		return ShortName;
	}
}
