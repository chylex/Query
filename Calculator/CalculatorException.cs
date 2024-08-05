using System;

namespace Calculator;

sealed class CalculatorException(string message) : Exception(message);
