using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Calculator.Math;

sealed class UnitUniverses {
	private readonly FrozenDictionary<Unit, UnitUniverse> unitToUniverse;
	private readonly FrozenDictionary<string, Unit> nameToUnit;

	internal UnitUniverses(params UnitUniverse[] universes) {
		Dictionary<Unit, UnitUniverse> unitToUniverseBuilder = new (ReferenceEqualityComparer.Instance);
		Dictionary<string, Unit> nameToUnitBuilder = new ();

		foreach (UnitUniverse universe in universes) {
			foreach (Unit unit in universe.AllUnits) {
				unitToUniverseBuilder.Add(unit, universe);
				unit.AssignNamesTo(nameToUnitBuilder);
			}
		}

		unitToUniverse = unitToUniverseBuilder.ToFrozenDictionary(ReferenceEqualityComparer.Instance);
		nameToUnit = nameToUnitBuilder.ToFrozenDictionary();
	}
	
	public bool TryGetUnit(string name, [NotNullWhen(true)] out Unit? unit) {
		return nameToUnit.TryGetValue(name, out unit);
	}
	
	public bool TryGetUniverse(Unit unit, [NotNullWhen(true)] out UnitUniverse? universe) {
		return unitToUniverse.TryGetValue(unit, out universe);
	}
}
