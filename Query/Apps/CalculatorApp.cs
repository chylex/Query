using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Calculator;
using Calculator.Math;
using Calculator.Parser;

namespace Query.Apps;

sealed class CalculatorApp : IApp {
	public bool TryRun(string command, [NotNullWhen(true)] out string? output) {
		ImmutableArray<Token> tokens = new Tokenizer(command).Scan();
		Expression expression = new Parser(tokens).Parse();
		NumberWithUnit result = expression.Accept(new CalculatorExpressionVisitor());

		output = result.ToString();
		return true;
	}
}
