namespace Models.Cards
{   
    public class SpecialCard : Card
    {
        public Algorithm.Operator.OperatorType Type { get; set; }

        public SpecialCard(Algorithm.Operator.OperatorType type)
        {
            Type = type;
            MarkAsUnused();
        }

        public override string GetDisplayText()
        {
            return Type switch
            {
                Algorithm.Operator.OperatorType.Multiply => "×",
                Algorithm.Operator.OperatorType.SquareRoot => "√",
                _ => "?"
            };
        }

        public override string GetCardType()
        {
            return "Special";
        }

        public override Card Clone()
        {
            return new SpecialCard(Type);
        }
    }
}
