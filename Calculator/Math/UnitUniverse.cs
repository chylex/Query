using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using ExtendedNumerics;

namespace Calculator.Math;

sealed class UnitUniverse(
	Unit primaryUnit,
	FrozenDictionary<Unit, Func<Number, Number>> unitToConversionToPrimaryUnit,
	FrozenDictionary<Unit, Func<Number, Number>> unitToConversionFromPrimaryUnit
) {
	public ImmutableArray<Unit> AllUnits => unitToConversionToPrimaryUnit.Keys;

	internal bool TryConvert(Number value, Unit fromUnit, Unit toUnit, [NotNullWhen(true)] out Number? converted) {
		if (fromUnit == toUnit) {
			converted = value;
			return true;
		}
		else if (unitToConversionToPrimaryUnit.TryGetValue(fromUnit, out var convertToPrimaryUnit) && unitToConversionFromPrimaryUnit.TryGetValue(toUnit, out var convertFromPrimaryUnit)) {
			converted = convertFromPrimaryUnit(convertToPrimaryUnit(value));
			return true;
		}
		else {
			converted = null;
			return false;
		}
	}

	internal sealed record SI(string ShortPrefix, string LongPrefix, int Factor) {
		internal static readonly List<SI> All = [
			new SI("Q", "quetta", 30),
			new SI("R", "ronna", 27),
			new SI("Y", "yotta", 24),
			new SI("Z", "zetta", 21),
			new SI("E", "exa", 18),
			new SI("P", "peta", 15),
			new SI("T", "tera", 12),
			new SI("G", "giga", 9),
			new SI("M", "mega", 6),
			new SI("k", "kilo", 3),
			new SI("h", "hecto", 2),
			new SI("da", "deca", 1),
			new SI("d", "deci", -1),
			new SI("c", "centi", -2),
			new SI("m", "milli", -3),
			new SI("μ", "micro", -6),
			new SI("n", "nano", -9),
			new SI("p", "pico", -12),
			new SI("f", "femto", -15),
			new SI("a", "atto", -18),
			new SI("z", "zepto", -21),
			new SI("y", "yocto", -24),
			new SI("r", "ronto", -27),
			new SI("q", "quecto", -30)
		];
	}

	internal sealed class Builder {
		private readonly Dictionary<Unit, Func<Number, Number>> unitToConversionToPrimaryUnit = new (ReferenceEqualityComparer.Instance);
		private readonly Dictionary<Unit, Func<Number, Number>> unitToConversionFromPrimaryUnit = new (ReferenceEqualityComparer.Instance);
		private readonly Unit primaryUnit;

		public Builder(Unit primaryUnit) {
			this.primaryUnit = primaryUnit;
			AddUnit(primaryUnit, 1);
		}

		public Builder AddUnit(Unit unit, Func<Number, Number> convertToPrimaryUnit, Func<Number, Number> convertFromPrimaryUnit) {
			unitToConversionToPrimaryUnit.Add(unit, convertToPrimaryUnit);
			unitToConversionFromPrimaryUnit.Add(unit, convertFromPrimaryUnit);
			return this;
		}

		public Builder AddUnit(Unit unit, Number amountInPrimaryUnit) {
			return AddUnit(unit, number => number * amountInPrimaryUnit, number => number / amountInPrimaryUnit);
		}

		private void AddUnitSI(SI si, Func<SI, Unit> unitFactory, Func<int, int> factorModifier) {
			int factor = factorModifier(si.Factor);
			BigInteger powerOfTen = BigInteger.Pow(10, System.Math.Abs(factor));
			BigRational amountInPrimaryUnit = factor > 0 ? new BigRational(powerOfTen) : new BigRational(1, powerOfTen);
			AddUnit(unitFactory(si), amountInPrimaryUnit);
		}

		public Builder AddSI(Func<SI, Unit> unitFactory, Func<int, int> factorModifier) {
			foreach (SI si in SI.All) {
				AddUnitSI(si, unitFactory, factorModifier);
			}

			return this;
		}

		public Builder AddSI(Func<int, int> factorModifier) {
			Unit PrefixPrimaryUnit(SI si) {
				return new Unit(si.ShortPrefix + primaryUnit.ShortName, [..primaryUnit.LongNames.Select(longName => si.LongPrefix + longName)]);
			}

			foreach (SI si in SI.All) {
				AddUnitSI(si, PrefixPrimaryUnit, factorModifier);
			}

			return this;
		}

		public Builder AddSI() {
			return AddSI(static factor => factor);
		}

		public UnitUniverse Build() {
			return new UnitUniverse(
				primaryUnit,
				unitToConversionToPrimaryUnit.ToFrozenDictionary(ReferenceEqualityComparer.Instance),
				unitToConversionFromPrimaryUnit.ToFrozenDictionary(ReferenceEqualityComparer.Instance)
			);
		}
	}
}
