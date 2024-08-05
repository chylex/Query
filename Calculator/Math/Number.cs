using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using ExtendedNumerics;

namespace Calculator.Math;

[SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
[SuppressMessage("ReSharper", "MemberCanBeInternal")]
public abstract record Number : IAdditionOperators<Number, Number, Number>,
                                ISubtractionOperators<Number, Number, Number>,
                                IMultiplyOperators<Number, Number, Number>,
                                IDivisionOperators<Number, Number, Number>,
                                IModulusOperators<Number, Number, Number>,
                                IUnaryPlusOperators<Number, Number>,
                                IUnaryNegationOperators<Number, Number>,
                                IAdditiveIdentity<Number, Number.Rational>,
                                IMultiplicativeIdentity<Number, Number.Rational> {
	protected abstract decimal AsDecimal { get; }

	public abstract Number Pow(Number exponent);

	public abstract string ToString(IFormatProvider? formatProvider);

	public sealed override string ToString() {
		return ToString(CultureInfo.InvariantCulture);
	}

	/// <summary>
	/// Represents an integer number with arbitrary precision.
	/// </summary>
	public sealed record Rational(BigRational Value) : Number {
		protected override decimal AsDecimal => (decimal) Value;

		public override Number Pow(Number exponent) {
			if (exponent is Rational { Value: {} rationalExponent }) {
				Fraction fractionExponent = rationalExponent.GetImproperFraction();
				if (fractionExponent.Denominator == 1 && fractionExponent.Numerator >= 0) {
					try {
						return new Rational(BigRational.Pow(Value, fractionExponent.Numerator));
					} catch (OverflowException) {}
				}
			}

			return new Decimal(AsDecimal).Pow(exponent);
		}

		public override string ToString(IFormatProvider? formatProvider) {
			Fraction fraction = Value.GetImproperFraction();
			return fraction.Denominator == 1 ? fraction.Numerator.ToString(formatProvider) : AsDecimal.ToString(formatProvider);
		}
	}

	/// <summary>
	/// Represents a decimal number with limited precision.
	/// </summary>
	public sealed record Decimal(decimal Value) : Number {
		public Decimal(double value) : this((decimal) value) {}

		protected override decimal AsDecimal => Value;

		public override Number Pow(Number exponent) {
			double doubleValue = (double) Value;
			double doubleExponent = (double) exponent.AsDecimal;
			return new Decimal(System.Math.Pow(doubleValue, doubleExponent));
		}

		public override string ToString(IFormatProvider? formatProvider) {
			return Value.ToString(formatProvider);
		}
	}

	public static implicit operator Number(BigRational value) {
		return new Rational(value);
	}

	public static implicit operator Number(int value) {
		return new Rational(value);
	}
	
	public static implicit operator Number(long value) {
		return new Rational(value);
	}
	
	public static Rational AdditiveIdentity => new (BigInteger.Zero);
	public static Rational MultiplicativeIdentity => new (BigInteger.One);

	public static Number Add(Number left, Number right) {
		return left + right;
	}

	public static Number Subtract(Number left, Number right) {
		return left - right;
	}

	public static Number Multiply(Number left, Number right) {
		return left * right;
	}

	public static Number Divide(Number left, Number right) {
		return left / right;
	}

	public static Number Remainder(Number left, Number right) {
		return left % right;
	}

	public static Number Pow(Number left, Number right) {
		return left.Pow(right);
	}

	public static Number operator +(Number value) {
		return value;
	}

	public static Number operator -(Number value) {
		return Operate(value, BigRational.Negate, decimal.Negate);
	}

	public static Number operator +(Number left, Number right) {
		return Operate(left, right, BigRational.Add, decimal.Add);
	}

	public static Number operator -(Number left, Number right) {
		return Operate(left, right, BigRational.Subtract, decimal.Subtract);
	}

	public static Number operator *(Number left, Number right) {
		return Operate(left, right, BigRational.Multiply, decimal.Multiply);
	}

	public static Number operator /(Number left, Number right) {
		return Operate(left, right, BigRational.Divide, decimal.Divide);
	}

	public static Number operator %(Number left, Number right) {
		return Operate(left, right, BigRational.Mod, decimal.Remainder);
	}

	private static Number Operate(Number value, Func<BigRational, BigRational> rationalOperation, Func<decimal, decimal> decimalOperation) {
		return value is Rational rational
			       ? new Rational(rationalOperation(rational.Value))
			       : new Decimal(decimalOperation(value.AsDecimal));
	}

	private static Number Operate(Number left, Number right, Func<BigRational, BigRational, BigRational> rationalOperation, Func<decimal, decimal, decimal> decimalOperation) {
		return left is Rational leftRational && right is Rational rightRational
			       ? new Rational(rationalOperation(leftRational.Value, rightRational.Value))
			       : new Decimal(decimalOperation(left.AsDecimal, right.AsDecimal));
	}
}
