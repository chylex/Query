﻿using System;

namespace Query.Command;

sealed class CommandException : Exception {
	public CommandException(string message) : base(message) {}
	public CommandException(string message, Exception innerException) : base(message, innerException) {}
}
