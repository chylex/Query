using System.Linq;
using System.Text.RegularExpressions;
using Base.Utils;

namespace Base {
	public sealed class Command {
		private static readonly Regex RegexBalancedBrackets = new Regex(RegexUtils.Balance(@"\[", @"\]"), RegexOptions.Compiled);
		private static readonly Regex RegexBalancedParentheses = new Regex(RegexUtils.Balance(@"\(", @"\)"), RegexOptions.Compiled);

		public string Text { get; private set; }

		public string PotentialAppName {
			get {
				int firstSpace = Text.IndexOf(' ');

				if (firstSpace == -1) {
					return null;
				}

				string firstToken = Text.Substring(0, firstSpace);

				if (!firstToken.All(char.IsLetter)) {
					return null;
				}

				return firstToken;
			}
		}

		public bool IsSingleToken => Text.IndexOf(' ') == -1;

		public Command(string text) {
			Text = text;
		}

		public Command ReplaceBrackets(MatchEvaluator evaluator) {
			return new Command(RegexBalancedParentheses.Replace(RegexBalancedBrackets.Replace(Text, evaluator), evaluator));
		}
	}
}
