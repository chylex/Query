using System;
using System.Diagnostics.CodeAnalysis;
using AppConv.General;
using AppConv.Utils;

namespace AppConv.Units;

sealed class Storage : DecimalUnitConverterSimple<Storage.Units> {
	internal enum Units {
		Invalid = 0,
		Byte,
		Bit
	}

	[SuppressMessage("ReSharper", "PossibleLossOfFraction")]
	public Storage() {
		AddUnit(Units.Byte, "B", "byte", "bytes");
		AddUnit(Units.Bit, "b", "bit", "bits");

		SetUnitFactor(Units.Bit, 8M);

		SetInvalidUnitObject(Units.Invalid);

		var bitConversionProperties = new SI.ExtededProperties {
			FactorPredicate = static factor => factor > 0 && factor % 3 == 0,
			FromFunctionGenerator = static exponent => () => (decimal) Math.Pow(1024, -(exponent / 3)),
			ToFunctionGenerator = static exponent => () => (decimal) Math.Pow(1024, exponent / 3)
		};

		SI.AddSupportCustom(Units.Byte, [ "B" ], [ "byte", "bytes" ], ConvertFrom, ConvertTo, Names, bitConversionProperties);
		SI.AddSupportCustom(Units.Bit, [ "b" ], [ "bit", "bits" ], ConvertFrom, ConvertTo, Names, bitConversionProperties);
	}
}
