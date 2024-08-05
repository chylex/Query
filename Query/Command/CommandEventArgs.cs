using System;

namespace Query.Command;

sealed class CommandEventArgs(string command) : EventArgs {
	public string Command => command;
};
