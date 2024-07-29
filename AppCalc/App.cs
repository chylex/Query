using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Base;

namespace AppCalc;

public sealed partial class App : IApp {
	private static readonly Regex RegexValidCharacters = GetRegexValidCharacters();
	private static readonly Regex RegexTokenSeparator = GetRegexTokenSeparator();
	private static readonly Regex RegexRecurringDecimal = GetRegexRecurringDecimal();

	[GeneratedRegex(@"^[\s\d\.\-+*/%^]+$", RegexOptions.Compiled)]
	private static partial Regex GetRegexValidCharacters();

	[GeneratedRegex(@"((?<!\d)-?(((\d+)?\.\d+(\.\.\.)?)|\d+))|[^\d\s]", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
	private static partial Regex GetRegexTokenSeparator();

	[GeneratedRegex(@"\b(?:(\d+?\.\d{0,}?)(\d+?)\2+|([\d+?\.]*))\b", RegexOptions.Compiled)]
	private static partial Regex GetRegexRecurringDecimal();

	private static readonly char[] SplitSpace = [ ' ' ];

	public string[] RecognizedNames => [
		"calc",
		"calculate",
		"calculator"
	];

	public MatchConfidence GetConfidence(Command cmd) {
		return RegexValidCharacters.IsMatch(cmd.Text) ? MatchConfidence.Possible : MatchConfidence.None;
	}

	string IApp.ProcessCommand(Command cmd) {
		return ParseAndProcessExpression(cmd.Text);
	}

	private static string ParseAndProcessExpression(string text) {
		// text = RegexBalancedParentheses.Replace(text, match => " "+ParseAndProcessExpression(match.Groups[1].Value)+" "); // parens are handled as apps

		string expression = RegexTokenSeparator.Replace(text, static match => " " + match.Value + " ");
		string[] tokens = expression.Split(SplitSpace, StringSplitOptions.RemoveEmptyEntries);

		decimal result = ProcessExpression(tokens);

		if (Math.Abs(result - decimal.Round(result)) < 0.0000000000000000000000000010M) {
			return decimal.Round(result).ToString(CultureInfo.InvariantCulture);
		}

		string res = result.ToString(CultureInfo.InvariantCulture);
		bool hasDecimalPoint = decimal.Round(result) != result;

		if (res.Length == 30 && hasDecimalPoint && res.IndexOf('.') < 15) { // Length 30 uses all available bytes
			res = res[..^1];

			Match match = RegexRecurringDecimal.Match(res);

			if (match.Groups[2].Success) {
				string repeating = match.Groups[2].Value;

				StringBuilder build = new StringBuilder(34);
				build.Append(match.Groups[1].Value);

				do {
					build.Append(repeating);
				} while (build.Length + repeating.Length <= res.Length);

				return build.Append(repeating[..(1 + build.Length - res.Length)]).Append("...").ToString();
			}
		}
		else if (hasDecimalPoint) {
			res = res.TrimEnd('0');
		}

		return res;
	}

	private static decimal ProcessExpression(string[] tokens) {
		bool isPostfix;

		if (tokens.Length < 3) {
			isPostfix = false;
		}
		else {
			try {
				ParseNumberToken(tokens[0]);
			} catch (CommandException) {
				throw new CommandException("Prefix notation is not supported.");
			}

			try {
				ParseNumberToken(tokens[1]);
				isPostfix = true;
			} catch (CommandException) {
				isPostfix = false;
			}
		}

		if (isPostfix) {
			return ProcessPostfixExpression(tokens);
		}
		else {
			return ProcessPostfixExpression(ConvertInfixToPostfix(tokens));
		}
	}

	private static IEnumerable<string> ConvertInfixToPostfix(IEnumerable<string> tokens) {
		Stack<string> operators = new Stack<string>();

		foreach (string token in tokens) {
			if (Operators.With2Operands.Contains(token)) {
				int currentPrecedence = Operators.GetPrecedence(token);
				bool currentRightAssociative = Operators.IsRightAssociative(token);

				while (operators.Count > 0) {
					int topPrecedence = Operators.GetPrecedence(operators.Peek());

					if ((currentRightAssociative && currentPrecedence < topPrecedence) || (!currentRightAssociative && currentPrecedence <= topPrecedence)) {
						yield return operators.Pop();
					}
					else {
						break;
					}
				}

				operators.Push(token);
			}
			else {
				yield return ParseNumberToken(token).ToString(CultureInfo.InvariantCulture);
			}
		}

		while (operators.Count > 0) {
			yield return operators.Pop();
		}
	}

	private static decimal ProcessPostfixExpression(IEnumerable<string> tokens) {
		Stack<decimal> stack = new Stack<decimal>();

		foreach (string token in tokens) {
			decimal operand1, operand2;

			if (token == "-" && stack.Count == 1) {
				operand2 = stack.Pop();
				operand1 = 0M;
			}
			else if (Operators.With2Operands.Contains(token)) {
				if (stack.Count < 2) {
					throw new CommandException("Incorrect syntax, not enough numbers in stack.");
				}

				operand2 = stack.Pop();
				operand1 = stack.Pop();
			}
			else {
				operand1 = operand2 = 0M;
			}

			switch (token) {
				case "+":
					stack.Push(operand1 + operand2);
					break;
				case "-":
					stack.Push(operand1 - operand2);
					break;
				case "*":
					stack.Push(operand1 * operand2);
					break;

				case "/":
					if (operand2 == 0M) {
						throw new CommandException("Cannot divide by zero.");
					}

					stack.Push(operand1 / operand2);
					break;

				case "%":
					if (operand2 == 0M) {
						throw new CommandException("Cannot divide by zero.");
					}

					stack.Push(operand1 % operand2);
					break;

				case "^":
					if (operand1 == 0M && operand2 == 0M) {
						throw new CommandException("Cannot evaluate 0 to the power of 0.");
					}
					else if (operand1 < 0M && Math.Abs(operand2) < 1M) {
						throw new CommandException("Cannot evaluate a root of a negative number.");
					}

					try {
						stack.Push((decimal) Math.Pow((double) operand1, (double) operand2));
					} catch (OverflowException ex) {
						throw new CommandException("Number overflow.", ex);
					}

					break;

				default:
					stack.Push(ParseNumberToken(token));
					break;
			}
		}

		if (stack.Count != 1) {
			throw new CommandException("Incorrect syntax, too many numbers in stack.");
		}

		return stack.Pop();
	}

	private static decimal ParseNumberToken(string token) {
		string str = token;

		if (str.StartsWith("-.")) {
			str = "-0" + str[1..];
		}
		else if (str[0] == '.') {
			str = "0" + str;
		}

		if (str.EndsWith("...")) {
			string truncated = str[..^3];

			if (truncated.Contains('.')) {
				str = truncated;
			}
		}

		if (decimal.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal value)) {
			if (value.ToString(CultureInfo.InvariantCulture) != str) {
				throw new CommandException("Provided number is outside of decimal range: " + token);
			}

			return value;
		}
		else {
			throw new CommandException("Invalid token, expected a number: " + token);
		}
	}
}
