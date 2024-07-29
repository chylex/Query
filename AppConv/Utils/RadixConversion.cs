using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AppConv.Utils {
	static class RadixConversion {
		private const string Characters = "0123456789ABCDEF";

		public static bool IsBaseValid(int checkedBase) {
			return checkedBase >= 1 && checkedBase <= 16;
		}

		public static bool IsNumberValid(string contents, int checkedBase) {
			if (checkedBase == 1) {
				return contents.All(chr => chr == '1');
			}

			if (IsBaseValid(checkedBase)) {
				return contents.Select(chr => Characters.IndexOf(char.ToUpper(chr))).All(index => index != -1 && index < checkedBase);
			}

			return false;
		}

		public static string Do(string contents, int fromBase, int toBase) { // TODO biginteger
			if (fromBase == 1) {
				contents = contents.Length.ToString(CultureInfo.InvariantCulture);
				fromBase = 10;
			}

			long wip;

			if (fromBase == 10) {
				wip = long.Parse(contents, NumberStyles.None, CultureInfo.InvariantCulture);
			}
			else {
				wip = 0;

				for (int chr = 0; chr < contents.Length; chr++) {
					int index = Characters.IndexOf(char.ToUpperInvariant(contents[chr]));

					if (index > 0) {
						wip += index * (long) Math.Pow(fromBase, contents.Length - chr - 1);

						if (wip < 0) {
							throw new OverflowException();
						}
					}
				}
			}

			if (toBase == 1) {
				if (wip <= int.MaxValue) {
					return new string('1', (int) wip);
				}
				else {
					throw new OverflowException();
				}
			}
			else if (wip < toBase) {
				return Characters[(int) wip].ToString();
			}
			else {
				var converted = new StringBuilder();

				while (wip >= toBase) {
					int index = (int) (wip % toBase);
					converted.Insert(0, Characters[index]);

					wip = wip / toBase;
				}

				return converted.Insert(0, Characters[(int) wip]).ToString();
			}
		}
	}
}
