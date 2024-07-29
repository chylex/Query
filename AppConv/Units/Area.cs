using AppConv.General;

namespace AppConv.Units {
	class Area : DecimalUnitConverterSimple<Area.Units> {
		internal enum Units {
			Invalid = 0,
			SquareMM,
			SquareCM,
			SquareDM,
			SquareM,
			SquareKM,
			SquareMile,
			SquareYard,
			SquareFoot,
			SquareInch,
			Acre,
			Centiare,
			Deciare,
			Are,
			Decare,
			Hectare
		}

		public Area() {
			AddUnit(Units.SquareMM, "mm2", "square mm", "square millimeter", "square millimeters", "square millimetre", "square millimetres");
			AddUnit(Units.SquareCM, "cm2", "square cm", "square centimeter", "square centimeters", "square centimetre", "square centimetres");
			AddUnit(Units.SquareDM, "dm2", "square dm", "square decimeter", "square decimeters", "square decimetre", "square decimetres");
			AddUnit(Units.SquareM, "m2", "square m", "square meter", "square meters", "square metre", "square metres");
			AddUnit(Units.SquareKM, "km2", "square km", "square kilometer", "square kilometers", "square kilometre", "square kilometres");
			AddUnit(Units.SquareMile, "mi2", "sq mi", "sq mile", "sq miles", "square mi", "square mile", "square miles");
			AddUnit(Units.SquareYard, "yd2", "sq yd", "sq yard", "sq yards", "square yd", "square yard", "square yards");
			AddUnit(Units.SquareFoot, "ft2", "sq ft", "sq foot", "sq feet", "square ft", "square foot", "square feet");
			AddUnit(Units.SquareInch, "in2", "sq in", "sq inch", "sq inches", "square in", "square inch", "square inches");
			AddUnit(Units.Acre, "ac", "acre", "acres");
			AddUnit(Units.Centiare, "ca", "centiare", "centiares");
			AddUnit(Units.Deciare, "da", "deciare", "deciares"); // da is not canon but w/e
			AddUnit(Units.Are, "a", "are", "ares");
			AddUnit(Units.Decare, "daa", "decare", "decares");
			AddUnit(Units.Hectare, "ha", "hectare", "hectares");

			SetUnitFactor(Units.SquareMM, 1E+6M);
			SetUnitFactor(Units.SquareCM, 1E+4M);
			SetUnitFactor(Units.SquareDM, 1E+2M);
			SetUnitFactor(Units.SquareKM, 1E-6M);
			SetUnitFactor(Units.SquareMile, 3.8610215854245E-7M);
			SetUnitFactor(Units.SquareYard, 1.1959900463011M);
			SetUnitFactor(Units.SquareFoot, 10.76391041671M);
			SetUnitFactor(Units.SquareInch, 1550.0031000062M);
			SetUnitFactor(Units.Acre, 2.4710538146717E-4M);
			SetUnitFactor(Units.Deciare, 1E-1M);
			SetUnitFactor(Units.Are, 1E-2M);
			SetUnitFactor(Units.Decare, 1E-3M);
			SetUnitFactor(Units.Hectare, 1E-4M);

			SetInvalidUnitObject(Units.Invalid);
		}
	}
}
