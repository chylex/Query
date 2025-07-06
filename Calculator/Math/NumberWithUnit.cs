using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Calculator.Math;

[SuppressMessage("ReSharper", "MemberCanBeInternal")]
public sealed class NumberWithUnit : IAdditionOperators<NumberWithUnit, NumberWithUnit, NumberWithUnit>,
                                     ISubtractionOperators<NumberWithUnit, NumberWithUnit, NumberWithUnit>,
                                     IMultiplyOperators<NumberWithUnit, NumberWithUnit, NumberWithUnit>,
                                     IDivisionOperators<NumberWithUnit, NumberWithUnit, NumberWithUnit>,
                                     IModulusOperators<NumberWithUnit, NumberWithUnit, NumberWithUnit>,
                                     IUnaryPlusOperators<NumberWithUnit, NumberWithUnit>,
                                     IUnaryNegationOperators<NumberWithUnit, NumberWithUnit> {
	public Number Value { get; }
	public Unit? PrimaryUnit => Unit?.Primary;
	
	private UnitDescription? Unit { get; }
	
	private NumberWithUnit(Number value, UnitDescription? unit) {
		this.Value = value;
		this.Unit = unit;
	}
	
	public NumberWithUnit(Number value, ImmutableArray<Unit> units) : this(value, UnitDescription.From(units)) {}
	
	private NumberWithUnit WithValue(Number value) {
		return new NumberWithUnit(value, Unit);
	}
	
	public NumberWithUnit ConvertTo(ImmutableArray<Unit> targetUnits) {
		if (targetUnits.IsEmpty || Unit == null) {
			return new NumberWithUnit(Value, targetUnits);
		}
		else if (Unit.Universe.TryConvert(Value, Unit.Primary, targetUnits[0], out Number? converted)) {
			return new NumberWithUnit(converted, targetUnits);
		}
		else {
			throw new ArithmeticException("Cannot convert '" + Unit.Primary + "' to '" + targetUnits[0] + "'");
		}
	}
	
	public string ToString(IFormatProvider? formatProvider) {
		if (Unit == null) {
			return Value.ToString(formatProvider);
		}
		else if (Unit.Display.Length <= 1) {
			return Value.ToString(formatProvider) + " " + Unit.Primary;
		}
		else {
			ImmutableArray<Unit> unitsFromLargest = [..Unit.Display.OrderByDescending(unit => Unit.Universe.Convert(value: 1, fromUnit: unit, toUnit: Unit.Primary))];
			Unit smallestUnit = unitsFromLargest[^1];
			Number remaining = Unit.Universe.Convert(Value, Unit.Primary, smallestUnit);
			
			StringBuilder formatted = new StringBuilder();
			
			void AppendNumberWithUnit(Number value, Unit unit) {
				formatted.Append(value.ToString(formatProvider));
				formatted.Append(' ');
				formatted.Append(unit);
				formatted.Append(' ');
			}
			
			foreach (Unit nextUnit in unitsFromLargest[..^1]) {
				Number wholePart = Unit.Universe.Convert(remaining, smallestUnit, nextUnit).WholePart;
				
				if (!wholePart.IsZero) {
					AppendNumberWithUnit(wholePart, nextUnit);
					remaining -= Unit.Universe.Convert(wholePart, nextUnit, smallestUnit);
				}
			}
			
			if (!remaining.IsZero || formatted.Length == 0) {
				AppendNumberWithUnit(remaining, smallestUnit);
			}
			
			return formatted.Remove(formatted.Length - 1, length: 1).ToString();
		}
	}
	
	public override string ToString() {
		return ToString(CultureInfo.InvariantCulture);
	}
	
	public static implicit operator NumberWithUnit(Number number) {
		return new NumberWithUnit(number, unit: null);
	}
	
	public static NumberWithUnit operator +(NumberWithUnit value) {
		return value.WithValue(+value.Value);
	}
	
	public static NumberWithUnit operator -(NumberWithUnit value) {
		return value.WithValue(-value.Value);
	}
	
	public static NumberWithUnit operator +(NumberWithUnit left, NumberWithUnit right) {
		return Operate(left, right, Number.Add, static (leftNumber, leftUnit, rightNumber, rightUnit) => {
			if (leftUnit.Primary == rightUnit.Primary) {
				return new NumberWithUnit(leftNumber + rightNumber, leftUnit);
			}
			else if (rightUnit.Universe.TryConvert(rightNumber, fromUnit: rightUnit.Primary, toUnit: leftUnit.Primary, out Number? rightConverted)) {
				return new NumberWithUnit(leftNumber + rightConverted, leftUnit);
			}
			else {
				throw new ArithmeticException("Cannot add '" + leftUnit + "' and '" + rightUnit + "'");
			}
		});
	}
	
	public static NumberWithUnit operator -(NumberWithUnit left, NumberWithUnit right) {
		return Operate(left, right, Number.Subtract, static (leftNumber, leftUnit, rightNumber, rightUnit) => {
			if (leftUnit.Primary == rightUnit.Primary) {
				return new NumberWithUnit(leftNumber - rightNumber, leftUnit);
			}
			else if (rightUnit.Universe.TryConvert(rightNumber, fromUnit: rightUnit.Primary, toUnit: leftUnit.Primary, out Number? rightConverted)) {
				return new NumberWithUnit(leftNumber - rightConverted, leftUnit);
			}
			else {
				throw new ArithmeticException("Cannot subtract '" + leftUnit + "' and '" + rightUnit + "'");
			}
		});
	}
	
	public static NumberWithUnit operator *(NumberWithUnit left, NumberWithUnit right) {
		return OperateWithoutUnits(left, right, Number.Multiply, "Cannot multiply");
	}
	
	public static NumberWithUnit operator /(NumberWithUnit left, NumberWithUnit right) {
		return OperateWithoutUnits(left, right, Number.Divide, "Cannot divide");
	}
	
	public static NumberWithUnit operator %(NumberWithUnit left, NumberWithUnit right) {
		return OperateWithoutUnits(left, right, Number.Remainder, "Cannot modulo");
	}
	
	public NumberWithUnit Pow(NumberWithUnit exponent) {
		return OperateWithoutUnits(this, exponent, Number.Pow, "Cannot exponentiate");
	}
	
	private static NumberWithUnit Operate(NumberWithUnit left, NumberWithUnit right, Func<Number, Number, Number> withoutUnitsOperation, Func<Number, UnitDescription, Number, UnitDescription, NumberWithUnit> withUnitsOperation) {
		if (right.Unit is null) {
			return left.WithValue(withoutUnitsOperation(left.Value, right.Value));
		}
		else if (left.Unit is null) {
			return right.WithValue(withoutUnitsOperation(left.Value, right.Value));
		}
		else {
			return withUnitsOperation(left.Value, left.Unit, right.Value, right.Unit);
		}
	}
	
	private static NumberWithUnit OperateWithoutUnits(NumberWithUnit left, NumberWithUnit right, Func<Number, Number, Number> withoutUnitsOperation, string messagePrefix) {
		return Operate(left, right, withoutUnitsOperation, (_, leftUnit, _, rightUnit) => throw new ArithmeticException(messagePrefix + " '" + leftUnit + "' and '" + rightUnit + "'"));
	}
	
	private sealed record UnitDescription(Unit Primary, ImmutableArray<Unit> Display, UnitUniverse Universe) {
		public static UnitDescription? From(ImmutableArray<Unit> units) {
			if (units.IsEmpty) {
				return null;
			}
			
			if (!Units.All.TryGetUniverse(units[0], out UnitUniverse? primaryUnitUniverse)) {
				throw new ArgumentException("Unknown unit universe: " + units[0]);
			}
			
			for (int index = 1; index < units.Length; index++) {
				Unit secondaryUnit = units[index];
				
				if (!Units.All.TryGetUniverse(secondaryUnit, out UnitUniverse? secondaryUnitUniverse)) {
					throw new ArgumentException("Unknown unit universe: " + secondaryUnit);
				}
				else if (primaryUnitUniverse != secondaryUnitUniverse) {
					throw new ArgumentException("All units must be from the same universe: " + secondaryUnitUniverse + " != " + primaryUnitUniverse);
				}
			}
			
			return new UnitDescription(units[0], units, primaryUnitUniverse);
		}
	}
}
