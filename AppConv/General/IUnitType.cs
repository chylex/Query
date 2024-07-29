namespace AppConv.General{
    internal interface IUnitType{
        bool TryProcess(string src, string dst, out string result);
    }
}
