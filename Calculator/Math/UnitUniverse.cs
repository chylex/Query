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
	string name,
	FrozenDictionary<Unit, Func<Number, Number>> unitToConversionToPrimaryUnit,
	FrozenDictionary<Unit, Func<Number, Number>> unitToConversionFromPrimaryUnit
) {
	public ImmutableArray<Unit> AllUnits => unitToConversionToPrimaryUnit.Keys;
	
	internal Number Convert(Number value, Unit fromUnit, Unit toUnit) {
		return TryConvert(value, fromUnit, toUnit, out var converted) ? converted : throw new ArgumentException("Cannot convert from " + fromUnit + " to " + toUnit);
	}
	
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
	
	public override string ToString() {
		return name;
	}
	
	internal sealed record SI(string ShortPrefix, string LongPrefix, int Factor) {
		internal static readonly List<SI> All = [
			new ("Q", "quetta", Factor: 30),
			new ("R", "ronna", Factor: 27),
			new ("Y", "yotta", Factor: 24),
			new ("Z", "zetta", Factor: 21),
			new ("E", "exa", Factor: 18),
			new ("P", "peta", Factor: 15),
			new ("T", "tera", Factor: 12),
			new ("G", "giga", Factor: 9),
			new ("M", "mega", Factor: 6),
			new ("k", "kilo", Factor: 3),
			new ("h", "hecto", Factor: 2),
			new ("da", "deca", Factor: 1),
			new ("d", "deci", Factor: -1),
			new ("c", "centi", Factor: -2),
			new ("m", "milli", Factor: -3),
			new ("μ", "micro", Factor: -6),
			new ("n", "nano", Factor: -9),
			new ("p", "pico", Factor: -12),
			new ("f", "femto", Factor: -15),
			new ("a", "atto", Factor: -18),
			new ("z", "zepto", Factor: -21),
			new ("y", "yocto", Factor: -24),
			new ("r", "ronto", Factor: -27),
			new ("q", "quecto", Factor: -30),
		];
	}
	
	internal sealed class Builder {
		private readonly string name;
		private readonly Unit primaryUnit;
		private readonly Dictionary<Unit, Func<Number, Number>> unitToConversionToPrimaryUnit = new (ReferenceEqualityComparer.Instance);
		private readonly Dictionary<Unit, Func<Number, Number>> unitToConversionFromPrimaryUnit = new (ReferenceEqualityComparer.Instance);
		
		public Builder(string name, Unit primaryUnit) {
			this.name = name;
			this.primaryUnit = primaryUnit;
			AddUnit(primaryUnit, amountInPrimaryUnit: 1);
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
			BigInteger powerOfTen = BigInteger.Pow(value: 10, System.Math.Abs(factor));
			BigRational amountInPrimaryUnit = factor > 0 ? new BigRational(powerOfTen) : new BigRational(numerator: 1, powerOfTen);
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
				name,
				unitToConversionToPrimaryUnit.ToFrozenDictionary(ReferenceEqualityComparer.Instance),
				unitToConversionFromPrimaryUnit.ToFrozenDictionary(ReferenceEqualityComparer.Instance)
			);
		}
	}
}
