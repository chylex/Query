using System.Linq;
using System.Text.RegularExpressions;
using Base.Utils;

namespace Base;

public sealed class Command(string text) {
	private static readonly Regex RegexBalancedBrackets = new (RegexUtils.Balance(@"\[", @"\]"), RegexOptions.Compiled);
	private static readonly Regex RegexBalancedParentheses = new (RegexUtils.Balance(@"\(", @"\)"), RegexOptions.Compiled);

	public string Text { get; } = text;

	public string? PotentialAppName {
		get {
			int firstSpace = Text.IndexOf(' ');

			if (firstSpace == -1) {
				return null;
			}

			string firstToken = Text[..firstSpace];

			if (!firstToken.All(char.IsLetter)) {
				return null;
			}

			return firstToken;
		}
	}

	public bool IsSingleToken => !Text.Contains(' ');

	public Command ReplaceBrackets(MatchEvaluator evaluator) {
		return new Command(RegexBalancedParentheses.Replace(RegexBalancedBrackets.Replace(Text, evaluator), evaluator));
	}
}
