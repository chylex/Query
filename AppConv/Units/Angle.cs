using System;
using AppConv.General;

namespace AppConv.Units {
    internal class Angle : DecimalUnitConverterSimple<Angle.Units>{
        internal enum Units{
            Invalid = 0, Degree, Radian, Gradian
        }

        protected override int Precision{
            get{
                return 4;
            }
        }

        public Angle(){
            AddUnit(Units.Degree, "deg", "degree", "degrees", "arc degree", "arc degrees", "arcdegree", "arcdegrees", "°");
            AddUnit(Units.Radian, "rad", "radian", "radians");
            AddUnit(Units.Gradian, "grad", "grade", "gon", "gradian", "gradians");

            SetUnitFactor(Units.Radian, (decimal)Math.PI/180M);
            SetUnitFactor(Units.Gradian, 10M/9M);

            SetInvalidUnitObject(Units.Invalid);
        }

        // TODO convert degree notation 15°24'9"
    }
}
