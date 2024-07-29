using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AppConv.General{
    internal abstract class DecimalUnitConverterBase<T> : IUnitType where T : struct{
        internal sealed class DecimalFuncMap : Dictionary<T, Func<decimal, decimal>>{
            public DecimalFuncMap(){}
            public DecimalFuncMap(int capacity) : base(capacity){}
        }

        internal sealed class NameMap : Dictionary<string, T>{
            public NameMap(){}
            public NameMap(int capacity) : base(capacity){}
        }

        protected abstract NameMap Names { get; }
        protected abstract DecimalFuncMap ConvertTo { get; }
        protected abstract DecimalFuncMap ConvertFrom { get; }

        protected virtual int Precision{
            get{
                return 0;
            }
        }

        protected virtual bool CaseCheck{
            get{
                return false;
            }
        }

        protected virtual NumberStyles NumberStyle{
            get{
                return NumberStyles.Float & ~NumberStyles.AllowLeadingSign;
            }
        }

        protected virtual string ProcessSrc(string src){
            return src;
        }

        protected virtual string ProcessDst(string dst){
            return dst;
        }

        protected abstract bool IsValueInvalid(T value);

        protected virtual decimal Convert(decimal value, T from, T to){
            return ConvertFrom[to](ConvertTo[from](value));
        }

        protected virtual string Format(decimal value){
            if (Precision > 0){
                decimal truncated = decimal.Truncate(value);

                if (value == truncated){
                    return truncated.ToString(CultureInfo.InvariantCulture);
                }
            }

            int decimalPlaces = Precision;

            if (Math.Abs(value) < 1M){
                double fractionalPart = (double)Math.Abs(value%1M);
                int fractionalZeroCount = -(int)Math.Ceiling(Math.Log(fractionalPart, 10D));

                decimalPlaces = Math.Min(28, fractionalZeroCount+Precision);
            }

            string result = decimal.Round(value, decimalPlaces, MidpointRounding.ToEven).ToString(CultureInfo.InvariantCulture);

            if (decimalPlaces > 0){
                result = result.TrimEnd('0').TrimEnd('.');
            }

            return result;
        }
        
        public bool TryProcess(string src, string dst, out string result){
            src = ProcessSrc(src);
            dst = ProcessDst(dst);

            KeyValuePair<string, T>[] pairs = new KeyValuePair<string, T>[2];

            for(int index = 0; index < 2; index++){
                string str = index == 0 ? src : dst;

                if (CaseCheck){
                    List<KeyValuePair<string, T>> list = Names.Where(kvp => str.EndsWith(kvp.Key, StringComparison.InvariantCultureIgnoreCase) && (str.Length == kvp.Key.Length || !char.IsLetter(str[str.Length-kvp.Key.Length-1]))).ToList();

                    if (list.Count == 1){
                        pairs[index] = list[0];
                    }
                    else{
                        pairs[index] = list.FirstOrDefault(kvp => str.EndsWith(kvp.Key, StringComparison.InvariantCulture));
                    }
                }
                else{
                    pairs[index] = Names.FirstOrDefault(kvp => str.EndsWith(kvp.Key, StringComparison.InvariantCultureIgnoreCase) && (str.Length == kvp.Key.Length || !char.IsLetter(str[str.Length-kvp.Key.Length-1])));
                }

                if (IsValueInvalid(pairs[index].Value)){
                    result = string.Empty;
                    return false;
                }

                if (index == 0){
                    src = src.Substring(0, src.Length-pairs[index].Key.Length).TrimEnd();
                }
            }

            decimal value;

            if (decimal.TryParse(src, NumberStyle, CultureInfo.InvariantCulture, out value)){
                result = Format(Convert(value, pairs[0].Value, pairs[1].Value));
                return true;
            }
            else{
                result = string.Empty;
                return false;
            }
        }
    }
}
