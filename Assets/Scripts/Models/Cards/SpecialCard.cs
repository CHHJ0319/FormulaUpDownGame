namespace Models.Cards
{   
    public class SpecialCard : Card
    {
        public OperatorType Type { get; set; }

        public SpecialCard(OperatorType type)
        {
            Type = type;
            IsConsumed = false;
        }

        public override string GetDisplayText()
        {
            return Type switch
            {
                OperatorType.Multiply => "×",
                OperatorType.SquareRoot => "√",
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

        public void MarkAsUsed()
        {
            IsConsumed = true;
        }

        public void MarkAsUnused()
        {
            IsConsumed = false;
        }
    }
}
