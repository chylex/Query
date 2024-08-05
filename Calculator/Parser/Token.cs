using System;
using System.Globalization;

namespace Calculator.Parser;

public abstract record Token {
	private Token() {}

	public sealed record Simple(SimpleTokenType Type) : Token {
		#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
		public override string ToString() {
			return Type switch {
				SimpleTokenType.PLUS              => "+",
				SimpleTokenType.MINUS             => "-",
				SimpleTokenType.STAR              => "*",
				SimpleTokenType.SLASH             => "/",
				SimpleTokenType.PERCENT           => "%",
				SimpleTokenType.CARET             => "^",
				SimpleTokenType.LEFT_PARENTHESIS  => "(",
				SimpleTokenType.RIGHT_PARENTHESIS => ")",
				_                                 => throw new ArgumentOutOfRangeException()
			};
		}
		#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
	}

	public sealed record Text(string Value) : Token {
		public override string ToString() {
			return Value;
		}
	}

	public sealed record Number(Math.Number Value) : Token {
		public override string ToString() {
			return Value.ToString(CultureInfo.InvariantCulture);
		}
	}
}
