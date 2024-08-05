using System.Collections.Immutable;
using System.Linq;
using Calculator.Math;

namespace Calculator.Parser;

public abstract record Expression {
	private Expression() {}

	public abstract T Accept<T>(ExpressionVisitor<T> visitor);

	public sealed record Number(Token.Number NumberToken) : Expression {
		public override T Accept<T>(ExpressionVisitor<T> visitor) {
			return visitor.VisitNumber(this);
		}
	}
	
	public sealed record NumbersWithUnits(ImmutableArray<(Token.Number, Unit)> NumberTokensWithUnits) : Expression {
		public override T Accept<T>(ExpressionVisitor<T> visitor) {
			return visitor.VisitNumbersWithUnits(this);
		}

		public override string ToString() {
			return nameof(NumbersWithUnits) + " { " + string.Join(", ", NumberTokensWithUnits.Select(static (number, unit) => number + " " + unit)) + " }";
		}
	}
	
	public sealed record Grouping(Expression Expression) : Expression {
		public override T Accept<T>(ExpressionVisitor<T> visitor) {
			return visitor.VisitGrouping(this);
		}
	}

	public sealed record Unary(Token.Simple Operator, Expression Right) : Expression {
		public override T Accept<T>(ExpressionVisitor<T> visitor) {
			return visitor.VisitUnary(this);
		}
	}

	public sealed record Binary(Expression Left, Token.Simple Operator, Expression Right) : Expression {
		public override T Accept<T>(ExpressionVisitor<T> visitor) {
			return visitor.VisitBinary(this);
		}
	}

	public sealed record UnitAssignment(Expression Left, Unit Unit) : Expression {
		public override T Accept<T>(ExpressionVisitor<T> visitor) {
			return visitor.VisitUnitAssignment(this);
		}
	}

	public sealed record UnitConversion(Expression Left, Unit Unit) : Expression {
		public override T Accept<T>(ExpressionVisitor<T> visitor) {
			return visitor.VisitUnitConversion(this);
		}
	}
}
