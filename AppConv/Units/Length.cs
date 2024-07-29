using System;
using System.Globalization;
using System.Linq;
using AppConv.General;
using AppConv.Utils;

namespace AppConv.Units;

sealed class Length : DecimalUnitConverterSimple<Length.Units> {
	internal enum Units {
		Invalid = 0,
		Meter,
		Inch,
		Foot,
		Yard,
		Mile
	}

	private static readonly string[] NamesInch = [ "in", "inch", "inches", "\"", "''" ];
	private static readonly string[] NamesFoot = [ "ft", "foot", "feet", "'" ];
	
	private static readonly char[] Separator = [ ' ' ];

	public Length() {
		AddUnit(Units.Meter, "m", "meter", "metre", "meters", "metres");
		AddUnit(Units.Inch, NamesInch);
		AddUnit(Units.Foot, NamesFoot);
		AddUnit(Units.Yard, "yd", "yard", "yards");
		AddUnit(Units.Mile, "mi", "mile", "miles");

		SetUnitFactor(Units.Inch, 39.37007874M);
		SetUnitFactor(Units.Foot, 3.280839895M);
		SetUnitFactor(Units.Yard, 1.093613298M);
		SetUnitFactor(Units.Mile, 0.0006213711922M);

		SetInvalidUnitObject(Units.Invalid);

		SI.AddSupport(Units.Meter, [ "m" ], [ "meter", "metre", "meters", "metres" ], ConvertFrom, ConvertTo, Names);
	}

	protected override string ProcessSrc(string src) {
		string updatedStr = src;

		updatedStr = updatedStr.Replace("&", " ");
		updatedStr = updatedStr.Replace(",", " ");

		string inchName = NamesInch.FirstOrDefault(name => src.Contains(name, StringComparison.OrdinalIgnoreCase));

		if (inchName == null) {
			return src;
		}

		int inchIndex = src.IndexOf(inchName, StringComparison.OrdinalIgnoreCase);
		updatedStr = updatedStr.Remove(inchIndex, inchName.Length).Insert(inchIndex, new string(' ', inchName.Length));

		string footName = NamesFoot.FirstOrDefault(name => updatedStr.Contains(name, StringComparison.OrdinalIgnoreCase));

		if (footName == null) {
			return src;
		}

		int footIndex = updatedStr.IndexOf(footName, StringComparison.OrdinalIgnoreCase);
		updatedStr = updatedStr.Remove(footIndex, footName.Length).Insert(footIndex, new string(' ', footName.Length));

		string[] tokens = updatedStr.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
		decimal[] numbers = new decimal[2];
		int numberIndex = 0;

		foreach (string token in tokens) {
			if (decimal.TryParse(token.Trim(), NumberStyle, CultureInfo.InvariantCulture, out decimal number)) {
				if (numberIndex < numbers.Length) {
					numbers[numberIndex++] = number;
				}
				else {
					return src;
				}
			}
		}

		if (numberIndex != numbers.Length) {
			return src;
		}

		decimal srcFeet = numbers[footIndex < inchIndex ? 0 : 1];
		decimal srcInches = numbers[inchIndex < footIndex ? 0 : 1];

		return srcInches + srcFeet * 12M + " in";
	}
}
