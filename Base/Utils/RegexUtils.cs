using System.Text;
using System.Text.RegularExpressions;

namespace Base.Utils;

public static class RegexUtils {
	public const RegexOptions Text = RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled;

	internal static string Balance(string escapedStart, string escapedEnd) { // \(((?>[^()]+|\((?<n>)|\)(?<-n>))+(?(n)(?!)))\)
		return new StringBuilder()
		       .Append(escapedStart)
		       .Append("((?>[^")
		       .Append(escapedStart)
		       .Append(escapedEnd)
		       .Append("]+|")
		       .Append(escapedStart)
		       .Append("(?<n>)|")
		       .Append(escapedEnd)
		       .Append("(?<-n>))+(?(n)(?!)))")
		       .Append(escapedEnd)
		       .ToString();
	}
}
