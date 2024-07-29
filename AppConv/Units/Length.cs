using System;
using System.Globalization;
using System.Linq;
using AppConv.General;
using AppConv.Utils;

namespace AppConv.Units{
    internal class Length : DecimalUnitConverterSimple<Length.Units>{
        internal enum Units{
            Invalid = 0, Meter, Inch, Foot, Yard, Mile
        }

        private static readonly string[] NamesInch = { "in", "inch", "inches", "\"", "''" };
        private static readonly string[] NamesFoot = { "ft", "foot", "feet", "'" };

        public Length(){
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

            SI.AddSupport(typeof(Units), Units.Meter, new []{ "m" }, new []{ "meter", "metre", "meters", "metres" }, ConvertFrom, ConvertTo, Names);
        }

        protected override string ProcessSrc(string src){
            string updatedStr = src;

            updatedStr = updatedStr.Replace("&", " ");
            updatedStr = updatedStr.Replace(",", " ");

            string inchName = NamesInch.FirstOrDefault(name => src.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1);

            if (inchName == null){
                return src;
            }

            int inchIndex = src.IndexOf(inchName, StringComparison.OrdinalIgnoreCase);
            updatedStr = updatedStr.Remove(inchIndex, inchName.Length).Insert(inchIndex, new string(' ', inchName.Length));

            string footName = NamesFoot.FirstOrDefault(name => updatedStr.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1);

            if (footName == null){
                return src;
            }

            int footIndex = updatedStr.IndexOf(footName, StringComparison.OrdinalIgnoreCase);
            updatedStr = updatedStr.Remove(footIndex, footName.Length).Insert(footIndex, new string(' ', footName.Length));

            string[] tokens = updatedStr.Split(new char[]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
            decimal[] numbers = new decimal[2];
            int numberIndex = 0;

            foreach(string token in tokens){
                decimal number;

                if (decimal.TryParse(token.Trim(), NumberStyle, CultureInfo.InvariantCulture, out number)){
                    if (numberIndex < numbers.Length){
                        numbers[numberIndex++] = number;
                    }
                    else{
                        return src;
                    }
                }
            }

            if (numberIndex != numbers.Length){
                return src;
            }

            decimal srcFeet = numbers[footIndex < inchIndex ? 0 : 1];
            decimal srcInches = numbers[inchIndex < footIndex ? 0 : 1];

            return srcInches+srcFeet*12M+" in";
        }
    }
}
