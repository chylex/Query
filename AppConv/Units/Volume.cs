using AppConv.General;
using AppConv.Utils;

namespace AppConv.Units {
	class Volume : DecimalUnitConverterSimple<Volume.Units> {
		internal enum Units {
			Invalid = 0,
			Liter,
			CubicMM,
			CubicCM,
			CubicDM,
			CubicM,
			CubicKM
		}

		public Volume() {
			AddUnit(Units.Liter, "l", "liter", "liters", "litre", "litres");
			AddUnit(Units.CubicMM, "mm3", "cubic mm", "cubic millimeter", "cubic millimeters", "cubic millimetre", "cubic millimetres");
			AddUnit(Units.CubicCM, "cm3", "cubic cm", "cubic centimeter", "cubic centimeters", "cubic centimetre", "cubic centimetres");
			AddUnit(Units.CubicDM, "dm3", "cubic dm", "cubic decimeter", "cubic decimeters", "cubic decimetre", "cubic decimetres");
			AddUnit(Units.CubicM, "m3", "cubic m", "cubic meter", "cubic meters", "cubic metre", "cubic metres");
			AddUnit(Units.CubicKM, "km3", "cubic km", "cubic kilometer", "cubic kilometers", "cubic kilometre", "cubic kilometres");

			SetUnitFactor(Units.CubicMM, 1000000M);
			SetUnitFactor(Units.CubicCM, 1000M);
			SetUnitFactor(Units.CubicM, 0.001M);
			SetUnitFactor(Units.CubicKM, 1E-12M);

			SetInvalidUnitObject(Units.Invalid);

			SI.AddSupport(typeof(Units), Units.Liter, new [] { "l" }, new [] { "liter", "litre", "liters", "litres" }, ConvertFrom, ConvertTo, Names);
		}
	}
}
