using Calculator.Math;
using Calculator.Parser;

namespace Calculator;

public sealed class CalculatorExpressionVisitor : ExpressionVisitor<NumberWithUnit> {
	public NumberWithUnit VisitNumber(Expression.Number number) {
		return new NumberWithUnit(number.NumberToken.Value, null);
	}

	public NumberWithUnit VisitNumbersWithUnits(Expression.NumbersWithUnits numbersWithUnits) {
		NumberWithUnit result = new Number.Rational(0);
		
		foreach ((Token.Number number, Unit unit) in numbersWithUnits.NumberTokensWithUnits) {
			result += new NumberWithUnit(number.Value, unit);
		}
		
		return result;
	}

	public NumberWithUnit VisitGrouping(Expression.Grouping grouping) {
		return Evaluate(grouping.Expression);
	}

	public NumberWithUnit VisitUnary(Expression.Unary unary) {
		(Token.Simple op, Expression right) = unary;

		return op.Type switch {
			SimpleTokenType.PLUS  => +Evaluate(right),
			SimpleTokenType.MINUS => -Evaluate(right),
			_                     => throw new CalculatorException("Unsupported unary operator: " + op.Type)
		};
	}

	public NumberWithUnit VisitBinary(Expression.Binary binary) {
		(Expression left, Token.Simple op, Expression right) = binary;

		return op.Type switch {
			SimpleTokenType.PLUS    => Evaluate(left) + Evaluate(right),
			SimpleTokenType.MINUS   => Evaluate(left) - Evaluate(right),
			SimpleTokenType.STAR    => Evaluate(left) * Evaluate(right),
			SimpleTokenType.SLASH   => Evaluate(left) / Evaluate(right),
			SimpleTokenType.PERCENT => Evaluate(left) % Evaluate(right),
			SimpleTokenType.CARET   => Evaluate(left).Pow(Evaluate(right)),
			_                       => throw new CalculatorException("Unsupported binary operator: " + op.Type)
		};
	}

	public NumberWithUnit VisitUnitAssignment(Expression.UnitAssignment unitAssignment) {
		(Expression left, Unit right) = unitAssignment;

		NumberWithUnit number = Evaluate(left);
		
		if (number.Unit is null) {
			return number with { Unit = right };
		}
		else {
			throw new CalculatorException("Expression already has a unit, cannot assign a new unit: " + right);
		}
	}

	public NumberWithUnit VisitUnitConversion(Expression.UnitConversion unitConversion) {
		(Expression left, Unit unit) = unitConversion;
		
		return Evaluate(left).ConvertTo(unit);
	}

	private NumberWithUnit Evaluate(Expression expression) {
		return expression.Accept(this);
	}
}
