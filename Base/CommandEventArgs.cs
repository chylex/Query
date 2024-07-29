using System;

namespace Base;

public sealed class CommandEventArgs(string text) : EventArgs {
	public Command Command { get; private set; } = new (text);
}
