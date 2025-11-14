using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Hand
    {
        public List<Cards.NumberCard> NumberCards { get; private set; }
        public List<Cards.OperatorCard> OperatorCards { get; private set; }
        public List<Cards.SpecialCard> SpecialCards { get; private set; }
        public List<Algorithm.Operator.OperatorType> DisabledOperators { get; private set; }

        public Hand()
        {
            NumberCards = new List<Cards.NumberCard>();
            OperatorCards = new List<Cards.OperatorCard>();
            SpecialCards = new List<Cards.SpecialCard>();
            DisabledOperators = new List<Algorithm.Operator.OperatorType>();
        }

        public void Clear()
        {
            NumberCards.Clear();
            OperatorCards.Clear();
            SpecialCards.Clear();
            DisabledOperators.Clear();
        }

        public void AddCard(Cards.Card card)
        {
            if (card is Cards.NumberCard num)
            {
                NumberCards.Add(num);
            }
            else if (card is Cards.OperatorCard op) 
            {
                OperatorCards.Add(op);
            }
            else if (card is Cards.SpecialCard sp)
            {
                SpecialCards.Add(sp);
            }
        }

        public int GetMultiplyCount()
        {
            return SpecialCards.Count(c => c.Type == Algorithm.Operator.OperatorType.Multiply);
        }

        public int GetSquareRootCount()
        {
            return SpecialCards.Count(c => c.Type == Algorithm.Operator.OperatorType.SquareRoot);
        }

        public bool IsOperatorEnabled(Algorithm.Operator.OperatorType op)
        {
            return !DisabledOperators.Contains(op);
        }

        public int GetTotalCardCount()
        {
            return NumberCards.Count + OperatorCards.Count + SpecialCards.Count;
        }
    }
}