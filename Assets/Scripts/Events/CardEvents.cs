using Models.Cards;
using System;

namespace Events
{
    public static class CardEvents
    {
        public static event Action<Models.Cards.Card> OnCardClicked;
        public static event Action<Models.Cards.Card> OnCardUsed;

        public static void ClearCarddEvents()
        {
            OnCardClicked = null;
            OnCardUsed = null;
        }

        public static void InvokeCardClicked(Models.Cards.Card card)
        {
            OnCardClicked?.Invoke(card);
        }

        public static void InvokeCardUsed(Models.Cards.Card card)
        {
            OnCardUsed?.Invoke(card);
        } 
    }
}

