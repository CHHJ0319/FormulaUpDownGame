using Models.Cards;
using System;

namespace Events
{
    public static class CardEvents
    {
        public static event Action<Models.Cards.Card> OnCardClicked;
        public static event Action<Models.Cards.Card> OnCardUsed;
        public static event Action<Card, bool> OnCardAdded;

        public static void ClearCarddEvents()
        {
            OnCardClicked = null;
            OnCardUsed = null;
            OnCardAdded = null;
        }

        public static void InvokeCardClicked(Models.Cards.Card card)
        {
            OnCardClicked?.Invoke(card);
        }

        public static void InvokeCardUsed(Models.Cards.Card card)
        {
            OnCardUsed?.Invoke(card);
        }

        public static void AddCardToPlayer(Card card)
        {
            OnCardAdded?.Invoke(card, true);
        }

        public static void AddCardToAI(Card card)
        {
            OnCardAdded?.Invoke(card, false);
        }
    }
}

