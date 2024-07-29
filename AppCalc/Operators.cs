namespace AppCalc;

static class Operators {
	internal static readonly string[] With2Operands = [ "+", "-", "*", "/", "%", "^" ];

	internal static int GetPrecedence(string token) {
		return token switch {
			"^"               => 4,
			"*" or "/" or "%" => 3,
			"+" or "-"        => 2,
			_                 => 1
		};
	}

	internal static bool IsRightAssociative(string token) {
		return token == "^";
	}
}
