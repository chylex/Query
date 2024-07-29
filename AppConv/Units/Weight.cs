using AppConv.General;
using AppConv.Utils;

namespace AppConv.Units {
    internal class Weight : DecimalUnitConverterSimple<Weight.Units>{
        internal enum Units{
            Invalid = 0, Gram, Pound, Ounce, Stone
        }

        public Weight(){
            AddUnit(Units.Gram, "g", "gram", "grams");
            AddUnit(Units.Pound, "lb", "lbs", "pound", "pounds");
            AddUnit(Units.Ounce, "oz", "ounce", "ounces");
            AddUnit(Units.Stone, "st", "stone", "stones");

            SetUnitFactor(Units.Pound, 0.0022046226218M);
            SetUnitFactor(Units.Ounce, 0.03527396195M);
            SetUnitFactor(Units.Stone, 0.0001574730444177697M);

            SetInvalidUnitObject(Units.Invalid);

            SI.AddSupport(typeof(Units), Units.Gram, new []{ "g" }, new []{ "gram", "grams" }, ConvertFrom, ConvertTo, Names);
        }
    }
}
