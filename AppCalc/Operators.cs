namespace AppCalc{
    internal static class Operators{
        internal static readonly string[] With2Operands = { "+", "-", "*", "/", "%", "^" };

        internal static int GetPrecedence(string token){
            switch(token){
                case "^":
                    return 4;

                case "*":
                case "/":
                case "%":
                    return 3;

                case "+":
                case "-":
                    return 2;

                default:
                    return 1;
            }
        }

        internal static bool IsRightAssociative(string token){
            return token == "^";
        }
    }
}
