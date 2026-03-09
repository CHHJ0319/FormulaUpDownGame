using UnityEngine;

namespace Models.Cards
{
    public class NumberCard : Card
    {
        private int minValue = 0;
        private int maxValue = 9;

        public int Value { get; }

        public NumberCard(int value)
        {
            Value = Mathf.Clamp(value, minValue, maxValue);
            MarkAsUnused();
        }

        public override Card Clone()
        {
            return new NumberCard(Value);
        }
    }
}
