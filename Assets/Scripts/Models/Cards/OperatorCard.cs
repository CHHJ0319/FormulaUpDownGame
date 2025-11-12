namespace Models.Cards
{
    public class OperatorCard : Card
    {
        public OperatorType Operator { get;}

        public OperatorCard(OperatorType op)
        {
            Operator = op;
        }

        public override string GetDisplayText()
        {
            return Operator switch
            {
                OperatorType.Add => "+",
                OperatorType.Subtract => "-",
                OperatorType.Multiply => "×",
                OperatorType.Divide => "÷",
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
