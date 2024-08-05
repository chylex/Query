using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Calculator.Parser;
using ExtendedNumerics;

namespace Calculator.Math;

[SuppressMessage("ReSharper", "ConvertToConstant.Local")]
[SuppressMessage("ReSharper", "HeapView.ObjectAllocation")]
static class Units {
	public static UnitUniverse Time { get; } = TimeUniverse().Build();
	public static UnitUniverse Length { get; } = LengthUniverse().Build();
	public static UnitUniverse Mass { get; } = MassUniverse().Build();
	public static UnitUniverse Area { get; } = AreaUniverse().Build();
	public static UnitUniverse Volume { get; } = VolumeUniverse().Build();
	public static UnitUniverse Angle { get; } = AngleUniverse().Build();
	public static UnitUniverse Temperature { get; } = TemperatureUniverse().Build();
	public static UnitUniverse InformationEntropy { get; } = InformationEntropyUniverse().Build();

	public static UnitUniverses All { get; } = new (Time, Length, Mass, Area, Volume, Angle, Temperature, InformationEntropy);

	private static UnitUniverse.Builder TimeUniverse() {
		var minute = 60;
		var hour = minute * 60;
		var day = hour * 24;
		var week = day * 7;

		return new UnitUniverse.Builder(new Unit("s", Pluralize("second")))
		       .AddUnit(new Unit("min", Pluralize("minute")), minute)
		       .AddUnit(new Unit("h", Pluralize("hour")), hour)
		       .AddUnit(new Unit("d", Pluralize("day")), day)
		       .AddUnit(new Unit("wk", Pluralize("week")), week);
	}

	private static UnitUniverse.Builder LengthUniverse() {
		var inch = Parse("0", "0254");
		var foot = inch * 12;
		var yard = foot * 3;
		var furlong = yard * 220;
		var mile = yard * 1760;
		var nauticalMile = 1_852;
		var lightYear = 9_460_730_472_580_800;

		return new UnitUniverse.Builder(new Unit("m", Pluralize("meter", "metre")))
		       .AddSI()
		       .AddUnit(new Unit("in", [ "inch", "inches", "\"" ]), inch)
		       .AddUnit(new Unit("ft", [ "foot", "feet", "'" ]), foot)
		       .AddUnit(new Unit("yd", Pluralize("yard")), yard)
		       .AddUnit(new Unit("fur", Pluralize("furlong")), furlong)
		       .AddUnit(new Unit("mi", Pluralize("mile")), mile)
		       .AddUnit(new Unit("nmi", Pluralize("nautical mile")), nauticalMile)
		       .AddUnit(new Unit("ly", Pluralize("light-year", "light year")), lightYear);
	}

	private static UnitUniverse.Builder MassUniverse() {
		var pound = Parse("453", "59237");
		var stone = pound * 14;
		var ounce = pound / 16;
		var dram = ounce / 16;

		return new UnitUniverse.Builder(new Unit("g", Pluralize("gram")))
		       .AddSI()
		       .AddUnit(new Unit("lb", [ "lbs", "pound", "pounds" ]), pound)
		       .AddUnit(new Unit("st", Pluralize("stone")), stone)
		       .AddUnit(new Unit("oz", Pluralize("ounce")), ounce)
		       .AddUnit(new Unit("dr", Pluralize("dram")), dram);
	}

	private static UnitUniverse.Builder AreaUniverse() {
		static Unit SquareMeter(string shortPrefix, string longPrefix) {
			return new Unit(shortPrefix + "m2", [
				$"square {shortPrefix}m",
				$"{shortPrefix}m square",
				$"{shortPrefix}m squared",
				$"{longPrefix}meter squared",
				$"{longPrefix}meters squared",
				$"{longPrefix}metre squared",
				$"{longPrefix}metres squared",
				..Pluralize($"square {longPrefix}meter", $"square {longPrefix}metre")
			]);
		}

		return new UnitUniverse.Builder(SquareMeter(string.Empty, string.Empty))
		       .AddSI(static si => SquareMeter(si.ShortPrefix, si.LongPrefix), static factor => factor * 2)
		       .AddUnit(new Unit("a", Pluralize("are")), 100)
		       .AddUnit(new Unit("ha", Pluralize("hectare")), 10_000);
	}

	private static UnitUniverse.Builder VolumeUniverse() {
		static Unit CubicMeter(string shortPrefix, string longPrefix) {
			return new Unit(shortPrefix + "m3", [
				$"cubic {shortPrefix}m",
				$"{shortPrefix}m cubed",
				$"{longPrefix}meter cubed",
				$"{longPrefix}meters cubed",
				$"{longPrefix}metre cubed",
				$"{longPrefix}metres cubed",
				..Pluralize($"cubic {longPrefix}meter", $"cubic {longPrefix}metre")
			]);
		}

		return new UnitUniverse.Builder(new Unit("l", Pluralize("litre", "liter")))
		       .AddSI()
		       .AddUnit(CubicMeter(string.Empty, string.Empty), 1000)
		       .AddSI(static si => CubicMeter(si.ShortPrefix, si.LongPrefix), static factor => (factor * 3) + 3);
	}

	private static UnitUniverse.Builder AngleUniverse() {
		return new UnitUniverse.Builder(new Unit("deg", [ "°", "degree", "degrees" ]))
		       .AddUnit(new Unit("rad", Pluralize("radian")), new Number.Decimal((decimal) System.Math.PI / 180M))
		       .AddUnit(new Unit("grad", Pluralize("gradian", "grade", "gon")), Ratio(9, 10));
	}

	private static BigRational KelvinOffset { get; } = Parse("273", "15");

	private static UnitUniverse.Builder TemperatureUniverse() {
		return new UnitUniverse.Builder(new Unit("°C", [ "C", "Celsius", "celsius" ]))
		       .AddUnit(new Unit("°F", [ "F", "Fahrenheit", "fahrenheit" ]), static f => (f - 32) * Ratio(5, 9), static c => c * Ratio(9, 5) + 32)
		       .AddUnit(new Unit("K", [ "Kelvin", "kelvin" ]), static k => k - KelvinOffset, static c => c + KelvinOffset);
	}

	private static UnitUniverse.Builder InformationEntropyUniverse() {
		var bit = Ratio(1, 8);
		var nibble = bit * 4;

		return new UnitUniverse.Builder(new Unit("B", Pluralize("byte")))
		       .AddSI()
		       .AddUnit(new Unit("b", Pluralize("bit")), bit)
		       .AddUnit(new Unit("nibbles", [ "nibble" ]), nibble)
		       .AddUnit(new Unit("KiB", Pluralize("kibibyte")), Pow(1024, 1))
		       .AddUnit(new Unit("MiB", Pluralize("mebibyte")), Pow(1024, 2))
		       .AddUnit(new Unit("GiB", Pluralize("gibibyte")), Pow(1024, 3))
		       .AddUnit(new Unit("TiB", Pluralize("tebibyte")), Pow(1024, 4))
		       .AddUnit(new Unit("PiB", Pluralize("pebibyte")), Pow(1024, 5))
		       .AddUnit(new Unit("EiB", Pluralize("exbibyte")), Pow(1024, 6))
		       .AddUnit(new Unit("ZiB", Pluralize("zebibyte")), Pow(1024, 7))
		       .AddUnit(new Unit("YiB", Pluralize("yobibyte")), Pow(1024, 8));
	}

	private static BigRational Parse(string integerPart, string fractionalPart) {
		return Tokenizer.ParseNumber(integerPart, fractionalPart);
	}

	private static BigRational Ratio(long numerator, long denominator) {
		return new BigRational(numerator, denominator);
	}

	private static BigRational Pow(int value, int exponent) {
		return BigRational.Pow(value, exponent);
	}

	private static ImmutableArray<string> Pluralize(params string[] names) {
		return [..names.SelectMany(static name => new [] { name, name + "s" })];
	}
}
