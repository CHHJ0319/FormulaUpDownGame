namespace Models.Cards
{
    public class OperatorCard : Card
    {
        public Algorithm.Operator Operator { get;}

        public OperatorCard(Algorithm.Operator op)
        {
            Operator = op;
            MarkAsUnused();
        }

        public override string GetDisplayText()
        {
            return Operator.Type switch
            {
                Algorithm.Operator.OperatorType.Add => "+",
                Algorithm.Operator.OperatorType.Subtract => "-",
                Algorithm.Operator.OperatorType.Multiply => "×",
                Algorithm.Operator.OperatorType.Divide => "÷",
                _ => "?"
            };
        }

        public override string GetCardType()
        {
            return "Operator";
        }

        public override Card Clone()
        {
            return new OperatorCard(Operator);
        }
    }
}
