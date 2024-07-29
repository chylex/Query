using System;
using System.Collections.Generic;
using System.Linq;
using AppConv.General;
using AppConv.Utils;
using Base;

namespace AppConv.Units {
	class Radix : IUnitType {
		private static readonly Dictionary<string, int> RadixDescriptions = new Dictionary<string, int> {
			{ "UNARY", 1 },
			{ "BINARY", 2 },
			{ "BIN", 2 },
			{ "TERNARY", 3 },
			{ "QUATERNARY", 4 },
			{ "QUINARY", 5 },
			{ "SENARY", 6 },
			{ "OCTAL", 8 },
			{ "OCT", 8 },
			{ "DECIMAL", 10 },
			{ "DEC", 10 },
			{ "UNDECIMAL", 11 },
			{ "DUODECIMAL", 12 },
			{ "DOZENAL", 12 },
			{ "TRIDECIMAL", 13 },
			{ "TETRADECIMAL", 14 },
			{ "PENTADECIMAL", 15 },
			{ "HEXADECIMAL", 16 },
			{ "HEX", 16 }
		};

		static Radix() {
			for (int baseNumber = 1; baseNumber <= 16; baseNumber++) {
				RadixDescriptions.Add("RADIX " + baseNumber, baseNumber);
				RadixDescriptions.Add("BASE " + baseNumber, baseNumber);
			}
		}

		public bool TryProcess(string src, string dst, out string result) {
			int targetBase;

			if (!RadixDescriptions.TryGetValue(dst.ToUpperInvariant(), out targetBase)) {
				result = string.Empty;
				return false;
			}

			string contents;
			int sourceBase;

			if (!ParseSrc(src, out contents, out sourceBase)) {
				result = string.Empty;
				return false;
			}

			if (contents[0] == '-') {
				throw new CommandException("Negative numbers are not supported.");
			}
			else if (!RadixConversion.IsBaseValid(sourceBase) || !RadixConversion.IsBaseValid(targetBase)) {
				throw new CommandException("Only bases between 1 and 16 allowed.");
			}
			else if (!RadixConversion.IsNumberValid(contents, sourceBase)) {
				throw new CommandException("The input is not a valid base " + sourceBase + " number: " + contents);
			}

			if (sourceBase == targetBase) {
				result = src;
				return true;
			}

			try {
				result = RadixConversion.Do(contents, sourceBase, targetBase);
			} catch (OverflowException) {
				throw new CommandException("The number has overflown.");
			}

			return true;
		}

		private static bool ParseSrc(string src, out string sourceContent, out int sourceBase) {
			if (src.All(chr => chr >= '0' && chr <= '9')) {
				sourceContent = src;
				sourceBase = 10;
				return true;
			}

			string upper = src.ToUpperInvariant();

			if (upper.StartsWith("0X")) {
				sourceContent = upper.Substring(2);
				sourceBase = 16;
				return true;
			}

			if (upper.StartsWith("0B")) {
				sourceContent = upper.Substring(2);
				sourceBase = 2;
				return true;
			}

			int fromIndex = src.IndexOf("FROM", StringComparison.InvariantCultureIgnoreCase);

			if (fromIndex != -1) {
				src = src.Remove(fromIndex, 4);
			}

			foreach (KeyValuePair<string, int> kvp in RadixDescriptions) {
				if (src.StartsWith(kvp.Key, StringComparison.InvariantCultureIgnoreCase)) {
					sourceContent = src.Substring(kvp.Key.Length).Trim();
					sourceBase = kvp.Value;
					return true;
				}
				else if (src.EndsWith(kvp.Key, StringComparison.InvariantCultureIgnoreCase)) {
					sourceContent = src.Substring(0, src.Length - kvp.Key.Length).Trim();
					sourceBase = kvp.Value;
					return true;
				}
			}

			sourceContent = string.Empty;
			sourceBase = 0;
			return false;
		}
	}
}
