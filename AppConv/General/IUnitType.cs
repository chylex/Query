namespace AppConv.General {
	interface IUnitType {
		bool TryProcess(string src, string dst, out string result);
	}
}
