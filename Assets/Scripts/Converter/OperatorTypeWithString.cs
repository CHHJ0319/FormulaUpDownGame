namespace Converter
{
    public static class OperatorTypeWithString
    {
        public static string ToSymbolString(Algorithm.Operator.OperatorType type)
        {
            return type switch
            {
                Algorithm.Operator.OperatorType.Add => "+",
                Algorithm.Operator.OperatorType.Subtract => "-",
                Algorithm.Operator.OperatorType.Multiply => "¡¿",
                Algorithm.Operator.OperatorType.Divide => "¡À",
                Algorithm.Operator.OperatorType.SquareRoot => "¡î",
                _ => "?"
            };
        }
    }
}
