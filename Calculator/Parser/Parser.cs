using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Calculator.Math;

namespace Calculator.Parser;

public sealed class Parser(ImmutableArray<Token> tokens) {
	private int current = 0;

	private bool IsEOF => current >= tokens.Length;

	private static readonly ImmutableArray<SimpleTokenType> PLUS_MINUS = [
		SimpleTokenType.PLUS,
		SimpleTokenType.MINUS
	];

	private static readonly ImmutableArray<SimpleTokenType> STAR_SLASH_PERCENT = [
		SimpleTokenType.STAR,
		SimpleTokenType.SLASH,
		SimpleTokenType.PERCENT
	];

	private static readonly ImmutableArray<SimpleTokenType> CARET = [
		SimpleTokenType.CARET
	];

	private bool Match(SimpleTokenType expectedTokenType, [NotNullWhen(true)] out Token.Simple? token) {
		return Match(simpleToken => simpleToken.Type == expectedTokenType, out token);
	}

	private bool Match(ImmutableArray<SimpleTokenType> expectedTokenTypes, [NotNullWhen(true)] out Token.Simple? token) {
		return Match(simpleToken => expectedTokenTypes.Contains(simpleToken.Type), out token);
	}

	private bool Match<T>([NotNullWhen(true)] out T? token) where T : Token {
		return Match(static _ => true, out token);
	}

	private bool Match<T>(Predicate<T> predicate, [NotNullWhen(true)] out T? token) where T : Token {
		if (!IsEOF && tokens[current] is T t && predicate(t)) {
			current++;
			token = t;
			return true;
		}

		token = null;
		return false;
	}

	public Expression Parse() {
		Expression term = Term();

		if (!IsEOF) {
			throw new ParseException("Incomplete expression");
		}

		return term;
	}

	private Expression Term() {
		return Binary(Factor, PLUS_MINUS);
	}

	private Expression Factor() {
		return Binary(Exponentiation, STAR_SLASH_PERCENT);
	}

	private Expression Exponentiation() {
		return Binary(Conversion, CARET);
	}

	private Expression Binary(Func<Expression> term, ImmutableArray<SimpleTokenType> expectedTokenTypes) {
		Expression left = term();

		while (Match(expectedTokenTypes, out Token.Simple? op)) {
			Expression right = term();
			left = new Expression.Binary(left, op, right);
		}

		return left;
	}

	private Expression Conversion() {
		Expression left = Unary();

		while (MatchUnitConversionOperator()) {
			if (!MatchUnit(out Unit? unit)) {
				throw new ParseException("Expected a unit literal");
			}

			left = new Expression.UnitConversion(left, unit);
		}

		return left;
	}

	private Expression Unary() {
		if (Match(PLUS_MINUS, out Token.Simple? op)) {
			Expression right = Unary();
			return new Expression.Unary(op, right);
		}
		else {
			return Primary();
		}
	}

	private Expression Primary() {
		if (Match(LiteralPredicate, out Token? literal)) {
			if (literal is not Token.Number number) {
				throw new ParseException("Expected a number literal");
			}

			Expression expression;

			if (!MatchUnit(out Unit? unit)) {
				expression = new Expression.Number(number);
			}
			else {
				var numbersWithUnits = ImmutableArray.CreateBuilder<(Token.Number, Unit)>();
				numbersWithUnits.Add((number, unit));

				while (MatchNumberWithUnit(out var numberWithUnit)) {
					numbersWithUnits.Add(numberWithUnit.Value);
				}

				expression = new Expression.NumbersWithUnits(numbersWithUnits.ToImmutable());
			}

			if (Match(SimpleTokenType.LEFT_PARENTHESIS, out _)) {
				expression = new Expression.Binary(expression, new Token.Simple(SimpleTokenType.STAR), InsideParentheses());
			}

			return expression;
		}

		if (Match(SimpleTokenType.LEFT_PARENTHESIS, out _)) {
			return new Expression.Grouping(InsideParentheses());
		}

		throw new ParseException("Unexpected token type: " + tokens[current]);
	}

	private static bool LiteralPredicate(Token token) {
		return token is Token.Text or Token.Number;
	}

	private Expression InsideParentheses() {
		Expression term = Term();

		if (!Match(SimpleTokenType.RIGHT_PARENTHESIS, out _)) {
			throw new ParseException("Expected ')' after expression.");
		}

		int position = current;

		if (MatchUnitConversionOperator()) {
			if (MatchUnit(out Unit? toUnit)) {
				return new Expression.UnitConversion(term, toUnit);
			}
			else {
				current = position;
			}
		}

		if (MatchUnit(out Unit? unit)) {
			return new Expression.UnitAssignment(term, unit);
		}
		else {
			return term;
		}
	}

	private bool MatchNumberWithUnit([NotNullWhen(true)] out (Token.Number, Unit)? numberWithUnit) {
		if (!Match(out Token.Number? number)) {
			numberWithUnit = null;
			return false;
		}

		if (!MatchUnit(out Unit? unit)) {
			throw new ParseException("Expected a unit literal");
		}

		numberWithUnit = (number, unit);
		return true;
	}

	private bool MatchUnit([NotNullWhen(true)] out Unit? unit) {
		int position = current;
		
		List<string> words = [];
		
		while (Match(out Token.Text? text)) {
			words.Add(text.Value);
		}

		for (int i = words.Count; i > 0; i--) {
			string unitName = string.Join(' ', words.Take(i));

			if (Units.All.TryGetUnit(unitName, out unit)) {
				current = position + i;
				return true;
			}
		}

		current = position;
		unit = null;
		return false;
	}

	private bool MatchUnitConversionOperator() {
		return Match<Token.Text>(static text => text.Value is "to" or "in", out _);
	}
}
