using System;

namespace Events
{
    public static class CardEvents
    {
        public static event Action<Models.Cards.Card> OnCardClicked;
        public static event Action<Models.Cards.Card> OnCardConsumed;


        public static void ClearCarddEvents()
        {
            OnCardClicked = null;
            OnCardConsumed = null;
        }
        public static void InvokeCardClicked(Models.Cards.Card card)
        {
            OnCardClicked?.Invoke(card);
        }
        public static void InvokeCardConsumed(Models.Cards.Card card)
        {
            OnCardConsumed?.Invoke(card);
        }
    }
}

