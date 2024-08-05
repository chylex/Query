using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Calculator.Math;

[SuppressMessage("ReSharper", "MemberCanBeInternal")]
public readonly record struct NumberWithUnit(Number Number, Unit? Unit) : IAdditionOperators<NumberWithUnit, NumberWithUnit, NumberWithUnit>,
                                                                          ISubtractionOperators<NumberWithUnit, NumberWithUnit, NumberWithUnit>,
                                                                          IMultiplyOperators<NumberWithUnit, NumberWithUnit, NumberWithUnit>,
                                                                          IDivisionOperators<NumberWithUnit, NumberWithUnit, NumberWithUnit>,
                                                                          IModulusOperators<NumberWithUnit, NumberWithUnit, NumberWithUnit>,
                                                                          IUnaryPlusOperators<NumberWithUnit, NumberWithUnit>,
                                                                          IUnaryNegationOperators<NumberWithUnit, NumberWithUnit> {
	public NumberWithUnit ConvertTo(Unit targetUnit) {
		if (Unit == null) {
			return this with { Unit = targetUnit };
		}
		else if (Units.All.TryGetUniverse(Unit, out var universe) && universe.TryConvert(Number, Unit, targetUnit, out var converted)) {
			return new NumberWithUnit(converted, targetUnit);
		}
		else {
			throw new ArithmeticException("Cannot convert '" + Unit + "' to '" + targetUnit + "'");
		}
	}

	public string ToString(IFormatProvider? formatProvider) {
		string number = Number.ToString(formatProvider);
		return Unit == null ? number : number + " " + Unit;
	}

	public override string ToString() {
		return ToString(CultureInfo.InvariantCulture);
	}

	public static implicit operator NumberWithUnit(Number number) {
		return new NumberWithUnit(number, null);
	}

	public static NumberWithUnit operator +(NumberWithUnit value) {
		return value with { Number = +value.Number };
	}

	public static NumberWithUnit operator -(NumberWithUnit value) {
		return value with { Number = -value.Number };
	}

	public static NumberWithUnit operator +(NumberWithUnit left, NumberWithUnit right) {
		return Operate(left, right, Number.Add, static (leftNumber, leftUnit, rightNumber, rightUnit) => {
			if (leftUnit == rightUnit) {
				return new NumberWithUnit(leftNumber + rightNumber, leftUnit);
			}
			else if (Units.All.TryGetUniverse(leftUnit, out UnitUniverse? universe) && universe.TryConvert(rightNumber, rightUnit, leftUnit, out Number? rightConverted)) {
				return new NumberWithUnit(leftNumber + rightConverted, leftUnit);
			}
			else {
				throw new ArithmeticException("Cannot add '" + leftUnit + "' and '" + rightUnit + "'");
			}
		});
	}

	public static NumberWithUnit operator -(NumberWithUnit left, NumberWithUnit right) {
		return Operate(left, right, Number.Subtract, static (leftNumber, leftUnit, rightNumber, rightUnit) => {
			if (leftUnit == rightUnit) {
				return new NumberWithUnit(leftNumber - rightNumber, leftUnit);
			}
			else if (Units.All.TryGetUniverse(leftUnit, out UnitUniverse? universe) && universe.TryConvert(rightNumber, rightUnit, leftUnit, out Number? rightConverted)) {
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

	private static NumberWithUnit Operate(NumberWithUnit left, NumberWithUnit right, Func<Number, Number, Number> withoutUnitsOperation, Func<Number, Unit, Number, Unit, NumberWithUnit> withUnitsOperation) {
		if (right.Unit is null) {
			return left with { Number = withoutUnitsOperation(left.Number, right.Number) };
		}
		else if (left.Unit is null) {
			return right with { Number = withoutUnitsOperation(left.Number, right.Number) };
		}
		else {
			return withUnitsOperation(left.Number, left.Unit, right.Number, right.Unit);
		}
	}

	private static NumberWithUnit OperateWithoutUnits(NumberWithUnit left, NumberWithUnit right, Func<Number, Number, Number> withoutUnitsOperation, string messagePrefix) {
		return Operate(left, right, withoutUnitsOperation, (_, leftUnit, _, rightUnit) => throw new ArithmeticException(messagePrefix + " '" + leftUnit + "' and '" + rightUnit + "'"));
	}
}
