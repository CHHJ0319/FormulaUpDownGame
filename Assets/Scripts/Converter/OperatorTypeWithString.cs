using System.Reflection;
using System.ComponentModel;

namespace Converter
{
    public static class OperatorTypeWithString
    {
        public static string ToSymbolString(Models.Cards.OperatorType type)
        {
            return type switch
            {
                Models.Cards.OperatorType.Add => "+",
                Models.Cards.OperatorType.Subtract => "-",
                Models.Cards.OperatorType.Multiply => "¡¿",
                Models.Cards.OperatorType.Divide => "¡À",
                Models.Cards.OperatorType.SquareRoot => "¡î",
                _ => "?"
            };
        }
    }
}
