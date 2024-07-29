using System;
using AppConv.General;
using AppConv.Utils;

namespace AppConv.Units {
    internal class Storage : DecimalUnitConverterSimple<Storage.Units>{
        internal enum Units{
            Invalid = 0, Byte, Bit
        }

        public Storage(){
            AddUnit(Units.Byte, "B", "byte", "bytes");
            AddUnit(Units.Bit, "b", "bit", "bits");

            SetUnitFactor(Units.Bit, 8M);

            SetInvalidUnitObject(Units.Invalid);

            SI.ExtededProperties bitConversionProperties = new SI.ExtededProperties{
                FactorPredicate = factor => factor > 0 && factor%3 == 0,
                FromFunctionGenerator = exponent => () => (decimal)Math.Pow(1024, -(int)(exponent/3)),
                ToFunctionGenerator = exponent => () => (decimal)Math.Pow(1024, (int)(exponent/3))
            };

            SI.AddSupportCustom(typeof(Units), Units.Byte, new []{ "B" }, new []{ "byte", "bytes" }, ConvertFrom, ConvertTo, Names, bitConversionProperties);
            SI.AddSupportCustom(typeof(Units), Units.Bit, new []{ "b" }, new []{ "bit", "bits" }, ConvertFrom, ConvertTo, Names, bitConversionProperties);
        }
    }
}
