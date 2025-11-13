using System.Collections.Generic;

namespace Models.Cards
{
    public class Deck
    {
        private readonly GameConfig config;
        private readonly List<Card> cards;

        private System.Random random;

        public Deck(GameConfig config, int? seed = null)
        {
            this.config = config;
            this.cards = new List<Card>();
            this.random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
        }

        public void BuildDeck()
        {
            ResetDeck();
            AddNumberCard();
            AddSpecialCard();
            ShuffleDeck();
        }

        private void ResetDeck()
        {
            cards.Clear();
        }

        private void AddNumberCard() 
        {
            for (int num = 0; num <= 10; num++)
            {
                for (int i = 0; i < config.NumberCardCopiesPerRound; i++)
                {
                    cards.Add(new NumberCard(num));
                }
            }
        }

        private void AddSpecialCard()
        {
            for (int i = 0; i < config.MultiplySpecialCardsPerRound; i++)
            {
                cards.Add(new SpecialCard(OperatorType.Multiply));
            }

            for (int i = 0; i < config.SquareRootSpecialCardsPerRound; i++)
            {
                cards.Add(new SpecialCard(OperatorType.SquareRoot));
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
    }
}