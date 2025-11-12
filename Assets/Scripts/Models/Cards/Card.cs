using UnityEngine;

namespace Models.Cards
{
    public abstract class Card
    {
        public bool IsConsumed { get; protected set; }

        public abstract string GetDisplayText();
        public abstract string GetCardType();
        public abstract Card Clone();
    }
}
