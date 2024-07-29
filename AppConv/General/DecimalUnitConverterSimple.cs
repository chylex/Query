using System;

namespace AppConv.General{
    internal abstract class DecimalUnitConverterSimple<T> : DecimalUnitConverterBase<T> where T : struct{
        // ReSharper disable once StaticMemberInGenericType
        private static readonly Func<decimal, decimal> FuncNoChange = val => val;

        private readonly NameMap UnitNames = new NameMap();
        private readonly DecimalFuncMap MapFrom = new DecimalFuncMap();
        private readonly DecimalFuncMap MapTo = new DecimalFuncMap();

        private int invalidUnitObject = -1;
        
        protected sealed override NameMap Names{
            get{
                return UnitNames;
            }
        }

        protected sealed override DecimalFuncMap ConvertFrom{
            get{
                return MapFrom;
            }
        }

        protected sealed override DecimalFuncMap ConvertTo{
            get{
                return MapTo;
            }
        }

        protected override int Precision{
            get{
                return 3;
            }
        }

        protected override bool CaseCheck{
            get{
                return true;
            }
        }

        protected void AddUnit(T unitObject, params string[] names){
            foreach(string name in names){
                UnitNames.Add(name, unitObject);
            }

            ConvertFrom.Add(unitObject, FuncNoChange);
            ConvertTo.Add(unitObject, FuncNoChange);
        }

        protected void SetUnitFactor(T unitObject, decimal factor){
            ConvertFrom[unitObject] = val => val*factor;
            ConvertTo[unitObject] = val => val/factor;
        }

        protected void SetInvalidUnitObject(T unitObject){
            this.invalidUnitObject = (int)(object)unitObject;
        }

        protected sealed override bool IsValueInvalid(T value){
            return (int)(object)value == invalidUnitObject;
        }
    }
}
