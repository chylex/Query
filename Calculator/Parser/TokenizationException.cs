using System;

namespace Calculator.Parser;

sealed class TokenizationException(string message, char character) : Exception(message) {
	public char Character { get; } = character;
}
