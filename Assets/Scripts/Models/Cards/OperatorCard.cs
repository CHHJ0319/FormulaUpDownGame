namespace Models.Cards
{
    public class OperatorCard : Card
    {
        public Algorithm.Operator Operator { get;}

        public OperatorCard(Algorithm.Operator.OperatorType type)
        {
            Operator = new Algorithm.Operator(type);
            MarkAsUnused();
        }

        public override string GetDisplayText()
        {
            return Converter.OperatorTypeWithString.ToSymbolString(Operator.Type);
        }

        public override Card Clone()
        {
            return new OperatorCard(Operator.Type);
        }
    }
}
