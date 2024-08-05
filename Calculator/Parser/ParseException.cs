using System;

namespace Calculator.Parser;

sealed class ParseException(string message) : Exception(message);
