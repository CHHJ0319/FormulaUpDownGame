using System.Collections.Generic;
using UnityEngine;

namespace Models.Cards
{
    public class Deck
    {
        private readonly List<Card> cards;

        private int numberCardCopiesPerRound;
        private int specialCardsPerRound;

        private System.Random random;

        public Deck(int numCounts, int specialCounts, int? seed = null)
        {
            numberCardCopiesPerRound = numCounts;
            specialCardsPerRound = specialCounts;

            cards = new List<Card>();
            random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
        }

        public void BuildDeck()
        {
            ResetDeck();
            AddNumberCard();
            AddSpecialCard();
            ShuffleDeck();
        }

        public Card Draw()
        {
            if (cards.Count == 0)
            {
                BuildDeck();
            }

            int lastIndex = cards.Count - 1;
            Card card = cards[lastIndex];
            cards.RemoveAt(lastIndex);

            return card.Clone();
        }

        private void ResetDeck()
        {
            cards.Clear();
        }

        private void AddNumberCard() 
        {
            for (int num = 0; num <= 10; num++)
            {
                for (int i = 0; i < numberCardCopiesPerRound; i++)
                {
                    cards.Add(new NumberCard(num));
                }
            }
        }

        private void AddSpecialCard()
        {
            for (int i = 0; i < specialCardsPerRound; i++)
            {
                if (Random.value < 0.5f)
                {
                    cards.Add(new SpecialCard(Algorithm.Operator.OperatorType.Multiply));

                }
                else
                {
                    cards.Add(new SpecialCard(Algorithm.Operator.OperatorType.SquareRoot));

                }
            }
        }

        private void ShuffleDeck()
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int randomIndex = random.Next(i + 1);

                Card temp = cards[i];
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }
        }
    }
}