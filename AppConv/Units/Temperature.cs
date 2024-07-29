using System.Globalization;
using System.Text.RegularExpressions;
using AppConv.General;

namespace AppConv.Units;

sealed partial class Temperature : DecimalUnitConverterBase<Temperature.Units> {
	internal enum Units {
		Invalid = 0,
		Celsius,
		Kelvin,
		Fahrenheit,
		Rankine,
		Delisle,
		Newton,
		Reaumur,
		Romer
	}

	private static readonly NameMap UnitNames = new (21) {
		{ "C", Units.Celsius },
		{ "Celsius", Units.Celsius },
		{ "K", Units.Kelvin },
		{ "Kelvin", Units.Kelvin },
		{ "F", Units.Fahrenheit },
		{ "Fahrenheit", Units.Fahrenheit },
		{ "R", Units.Rankine },
		{ "Ra", Units.Rankine },
		{ "Rankine", Units.Rankine },
		{ "De", Units.Delisle },
		{ "Delisle", Units.Delisle },
		{ "N", Units.Newton },
		{ "Newton", Units.Newton },
		{ "Re", Units.Reaumur },
		{ "Ré", Units.Reaumur },
		{ "Reaumur", Units.Reaumur },
		{ "Réaumur", Units.Reaumur },
		{ "Ro", Units.Romer },
		{ "Rø", Units.Romer },
		{ "Romer", Units.Romer },
		{ "Rømer", Units.Romer }
	};

	private static readonly DecimalFuncMap FromCelsius = new (8) {
		{ Units.Celsius, static val => val },
		{ Units.Kelvin, static val => val + 273.15M },
		{ Units.Fahrenheit, static val => val * 1.8M + 32M },
		{ Units.Rankine, static val => (val + 273.15M) * 1.8M },
		{ Units.Delisle, static val => (100M - val) * 1.5M },
		{ Units.Newton, static val => val * 0.33M },
		{ Units.Reaumur, static val => val * 0.8M },
		{ Units.Romer, static val => val * 0.525M + 7.5M }
	};

	private static readonly DecimalFuncMap ToCelsius = new (8) {
		{ Units.Celsius, static val => val },
		{ Units.Kelvin, static val => val - 273.15M },
		{ Units.Fahrenheit, static val => (val - 32M) * 5M / 9M },
		{ Units.Rankine, static val => (val - 491.67M) * 5M / 9M },
		{ Units.Delisle, static val => 100M - val * 2M / 3M },
		{ Units.Newton, static val => val * 100M / 33M },
		{ Units.Reaumur, static val => val * 1.25M },
		{ Units.Romer, static val => (val - 7.5M) * 40M / 21M }
	};

	private static readonly Regex RegexCleanup = GetRegexCleanup();

	[GeneratedRegex("deg(?:rees?)?|°", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
	private static partial Regex GetRegexCleanup();

	protected override NameMap Names => UnitNames;

	protected override DecimalFuncMap ConvertFrom => FromCelsius;

	protected override DecimalFuncMap ConvertTo => ToCelsius;

	protected override int Precision => 2;

	protected override NumberStyles NumberStyle => NumberStyles.Float;

	protected override string ProcessSrc(string src) {
		return RegexCleanup.Replace(src, "");
	}

	protected override string ProcessDst(string dst) {
		return RegexCleanup.Replace(dst, "");
	}

	protected override bool IsValueInvalid(Units value) {
		return value == Units.Invalid;
	}
}
