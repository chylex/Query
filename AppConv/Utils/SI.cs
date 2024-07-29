using System;
using System.Collections.Generic;
using AppConv.General;

namespace AppConv.Utils{
    internal static class SI{
        private static readonly List<Tuple<string, string, int>> Factors = new List<Tuple<string, string, int>>{
            new Tuple<string, string, int>("yotta", "Y", 24),
            new Tuple<string, string, int>("zetta", "Z", 21),
            new Tuple<string, string, int>("exa", "E", 18),
            new Tuple<string, string, int>("peta", "P", 15),
            new Tuple<string, string, int>("tera", "T", 12),
            new Tuple<string, string, int>("giga", "G", 9),
            new Tuple<string, string, int>("mega", "M", 6),
            new Tuple<string, string, int>("kilo", "k", 3),
            new Tuple<string, string, int>("hecto", "h", 2),
            new Tuple<string, string, int>("deca", "da", 1),
            new Tuple<string, string, int>("deci", "d", -1),
            new Tuple<string, string, int>("centi", "c", -2),
            new Tuple<string, string, int>("milli", "m", -3),
            new Tuple<string, string, int>("micro", "μ", -6),
            new Tuple<string, string, int>("nano", "n", -9),
            new Tuple<string, string, int>("pico", "p", -12),
            new Tuple<string, string, int>("femto", "f", -15),
            new Tuple<string, string, int>("atto", "a", -18),
            new Tuple<string, string, int>("zepto", "z", -21),
            new Tuple<string, string, int>("yocto", "y", -24)
        };

        public static void AddSupport<T>(Type enumType, T unitObject, string[] unitShortNames, string[] unitLongNames, DecimalUnitConverterBase<T>.DecimalFuncMap funcFrom, DecimalUnitConverterBase<T>.DecimalFuncMap funcTo, DecimalUnitConverterBase<T>.NameMap nameMap) where T : struct{
            int enumCounter = 1000+Factors.Count*(int)(object)unitObject;

            Func<decimal, decimal> convertFrom = funcFrom[unitObject];
            Func<decimal, decimal> convertTo = funcTo[unitObject];

            foreach(Tuple<string, string, int> factor in Factors){
                T enumObject = (T)(object)enumCounter++;
                int exponent = factor.Item3;

                foreach(string unitShortName in unitShortNames){
                    nameMap.Add(factor.Item2+unitShortName, enumObject);
                }

                foreach(string unitLongName in unitLongNames){
                    nameMap.Add(factor.Item1+unitLongName, enumObject);
                }
                
                funcFrom.Add(enumObject, val => convertFrom(val)*(decimal)Math.Pow(10, -exponent));
                funcTo.Add(enumObject, val => convertTo(val)*(decimal)Math.Pow(10, exponent));
            }
        }

        public static void AddSupportCustom<T>(Type enumType, T unitObject, string[] unitShortNames, string[] unitLongNames, DecimalUnitConverterBase<T>.DecimalFuncMap funcFrom, DecimalUnitConverterBase<T>.DecimalFuncMap funcTo, DecimalUnitConverterBase<T>.NameMap nameMap, ExtededProperties extendedProps) where T : struct{
            int enumCounter = 1000+Factors.Count*(int)(object)unitObject;

            Func<decimal, decimal> convertFrom = funcFrom[unitObject];
            Func<decimal, decimal> convertTo = funcTo[unitObject];

            foreach(Tuple<string, string, int> factor in Factors){
                if (extendedProps.FactorPredicate != null && !extendedProps.FactorPredicate(factor.Item3)){
                    continue;
                }

                T enumObject = (T)(object)enumCounter++;
                int exponent = factor.Item3;

                foreach(string unitShortName in unitShortNames){
                    nameMap.Add(factor.Item2+unitShortName, enumObject);
                }

                foreach(string unitLongName in unitLongNames){
                    nameMap.Add(factor.Item1+unitLongName, enumObject);
                }

                Func<decimal> genFrom = extendedProps.FromFunctionGenerator(exponent);
                Func<decimal> genTo = extendedProps.ToFunctionGenerator(exponent);
                
                funcFrom.Add(enumObject, val => convertFrom(val)*genFrom());
                funcTo.Add(enumObject, val => convertTo(val)*genTo());
            }
        }

        internal class ExtededProperties{
            public Predicate<int> FactorPredicate { get; set; }
            public Func<int, Func<decimal>> FromFunctionGenerator { get; set; }
            public Func<int, Func<decimal>> ToFunctionGenerator { get; set; }
        }
    }
}
