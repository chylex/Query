using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Text;
using ExtendedNumerics;

namespace Calculator.Parser;

public sealed class Tokenizer(string input) {
	private int position = 0;

	private bool IsEOF => position >= input.Length;

	private char Advance() {
		return input[position++];
	}

	private bool Match(char c) {
		return Match(found => found == c, out _);
	}

	private bool Match(Predicate<char> predicate, out char c) {
		if (IsEOF) {
			c = default;
			return false;
		}

		c = input[position];

		if (!predicate(c)) {
			return false;
		}

		position++;
		return true;
	}

	private void MatchWhile(StringBuilder result, Predicate<char> predicate) {
		while (Match(predicate, out char c)) {
			result.Append(c);
		}
	}

	private string MatchWhile(Predicate<char> predicate) {
		var result = new StringBuilder();
		MatchWhile(result, predicate);
		return result.ToString();
	}

	private string MatchRest(char firstChar, Predicate<char> predicate) {
		var result = new StringBuilder();
		result.Append(firstChar);
		MatchWhile(result, predicate);
		return result.ToString();
	}

	public ImmutableArray<Token> Scan() {
		ImmutableArray<Token>.Builder tokens = ImmutableArray.CreateBuilder<Token>();

		void AddToken(Token token) {
			tokens.Add(token);
		}

		void AddSimpleToken(SimpleTokenType tokenType) {
			AddToken(new Token.Simple(tokenType));
		}

		while (!IsEOF) {
			char c = Advance();
			switch (c) {
				case ' ':
					// Ignore whitespace.
					break;

				case '+':
					AddSimpleToken(SimpleTokenType.PLUS);
					break;

				case '-':
					AddSimpleToken(SimpleTokenType.MINUS);
					break;

				case '*':
					AddSimpleToken(SimpleTokenType.STAR);
					break;

				case '/':
					AddSimpleToken(SimpleTokenType.SLASH);
					break;

				case '%':
					AddSimpleToken(SimpleTokenType.PERCENT);
					break;

				case '^':
					AddSimpleToken(SimpleTokenType.CARET);
					break;

				case '(':
					AddSimpleToken(SimpleTokenType.LEFT_PARENTHESIS);
					break;

				case ')':
					AddSimpleToken(SimpleTokenType.RIGHT_PARENTHESIS);
					break;

				case '"' or '\'':
					AddToken(new Token.Text(c.ToString()));
					break;

				case '°':
				case {} when char.IsLetter(c):
					AddToken(new Token.Text(MatchRest(c, char.IsLetterOrDigit)));
					break;

				case {} when char.IsAsciiDigit(c):
					string integerPart = MatchRest(c, char.IsAsciiDigit);

					if (Match('.')) {
						string fractionalPart = MatchWhile(char.IsAsciiDigit);
						AddToken(new Token.Number(ParseNumber(integerPart, fractionalPart)));
					}
					else {
						AddToken(new Token.Number(ParseNumber(integerPart)));
					}

					break;

				default:
					throw new TokenizationException("Unexpected character: " + c, c);
			}
		}

		return tokens.ToImmutable();
	}

	internal static BigRational ParseNumber(string integerPart, string? fractionalPart = null) {
		if (fractionalPart == null) {
			return new BigRational(BigInteger.Parse(integerPart, NumberStyles.Integer, CultureInfo.InvariantCulture));
		}
		else {
			BigInteger numerator = BigInteger.Parse(integerPart + fractionalPart, NumberStyles.Integer, CultureInfo.InvariantCulture);
			BigInteger denominator = BigInteger.Pow(10, fractionalPart.Length);
			return new BigRational(numerator, denominator);
		}
	}
}
