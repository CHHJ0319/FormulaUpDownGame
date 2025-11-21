using System.Collections.Generic;
using System.Linq;

namespace Actors
{
    public class Hand
    {
        public List<Models.Cards.NumberCard> NumberCards { get; private set; }
        public List<Models.Cards.OperatorCard> OperatorCards { get; private set; }
        public List<Models.Cards.SpecialCard> SpecialCards { get; private set; }

        public Hand()
        {
            NumberCards = new List<Models.Cards.NumberCard>();
            OperatorCards = new List<Models.Cards.OperatorCard>();
            SpecialCards = new List<Models.Cards.SpecialCard>();
        }

        public void Clear()
        {
            NumberCards.Clear();
            OperatorCards.Clear();
            SpecialCards.Clear();
        }

        public void AddCard(Models.Cards.Card card)
        {
            if (card is Models.Cards.NumberCard num)
            {
                NumberCards.Add(num);
            }
            else if (card is Models.Cards.OperatorCard op) 
            {
                OperatorCards.Add(op);
            }
            else if (card is Models.Cards.SpecialCard sp)
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

        public void ResetCardsUsage()
        {
            foreach (var card in NumberCards)
            {
                card.MarkAsUnused(); 
            }
            foreach (var card in OperatorCards)
            {
                card.MarkAsUnused();
            }
            foreach (var card in SpecialCards)
            {
                card.MarkAsUnused();
            }
        }
    }
}