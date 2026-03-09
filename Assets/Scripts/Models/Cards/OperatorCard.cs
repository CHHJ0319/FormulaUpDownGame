namespace Models.Cards
{
    public class OperatorCard : Card
    {
        public Algorithm.Operator Operator { get; private set; }

        public OperatorCard(Algorithm.Operator.OperatorType type)
        {
            Operator = new Algorithm.Operator(type);
            MarkAsUnused();
        }

        public override Card Clone()
        {
            return new OperatorCard(Operator.Type);
        }
    }
}
