using Base;

namespace AppSys{
    internal interface IHandler{
        bool Matches(Command cmd);
        string Handle(Command cmd);
    }
}
