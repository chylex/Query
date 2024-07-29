using Base;

namespace AppSys {
	interface IHandler {
		bool Matches(Command cmd);
		string Handle(Command cmd);
	}
}
