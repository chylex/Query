using System.Globalization;
using System.Text.RegularExpressions;
using AppConv.General;
using Base.Utils;

namespace AppConv.Units {
	class Temperature : DecimalUnitConverterBase<Temperature.Units> {
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

		private static readonly NameMap UnitNames = new NameMap(21) {
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

		private static readonly DecimalFuncMap FromCelsius = new DecimalFuncMap(8) {
			{ Units.Celsius, val => val },
			{ Units.Kelvin, val => val + 273.15M },
			{ Units.Fahrenheit, val => val * 1.8M + 32M },
			{ Units.Rankine, val => (val + 273.15M) * 1.8M },
			{ Units.Delisle, val => (100M - val) * 1.5M },
			{ Units.Newton, val => val * 0.33M },
			{ Units.Reaumur, val => val * 0.8M },
			{ Units.Romer, val => val * 0.525M + 7.5M }
		};

		private static readonly DecimalFuncMap ToCelsius = new DecimalFuncMap(8) {
			{ Units.Celsius, val => val },
			{ Units.Kelvin, val => val - 273.15M },
			{ Units.Fahrenheit, val => (val - 32M) * 5M / 9M },
			{ Units.Rankine, val => (val - 491.67M) * 5M / 9M },
			{ Units.Delisle, val => 100M - val * 2M / 3M },
			{ Units.Newton, val => val * 100M / 33M },
			{ Units.Reaumur, val => val * 1.25M },
			{ Units.Romer, val => (val - 7.5M) * 40M / 21M }
		};

		private static readonly Regex RegexCleanup = new Regex("deg(?:rees?)?|°", RegexUtils.Text);

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
}
