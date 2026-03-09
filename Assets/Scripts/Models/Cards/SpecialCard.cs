namespace Models.Cards
{   
    public class SpecialCard : Card
    {
        public Algorithm.Operator Operator { get; set; }

        public SpecialCard(Algorithm.Operator.OperatorType type)
        {
            Operator = new Algorithm.Operator(type);
            MarkAsUnused();
        }

        public override Card Clone()
        {
            return new SpecialCard(Operator.Type);
        }
    }
}
