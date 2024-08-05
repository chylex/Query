namespace Calculator.Parser;

public interface ExpressionVisitor<T> {
	T VisitNumber(Expression.Number number);

	T VisitNumbersWithUnits(Expression.NumbersWithUnits numbersWithUnits);

	T VisitGrouping(Expression.Grouping grouping);

	T VisitUnary(Expression.Unary unary);

	T VisitBinary(Expression.Binary binary);

	T VisitUnitAssignment(Expression.UnitAssignment unitAssignment);

	T VisitUnitConversion(Expression.UnitConversion unitConversion);
}
