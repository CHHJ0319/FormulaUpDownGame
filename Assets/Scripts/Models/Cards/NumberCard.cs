using UnityEngine;

namespace Models.Cards
{
    public class NumberCard : Card
    {
        private int minValue = 0;
        private int maxValue = 10;

        public int Value { get; }

        public NumberCard(int value)
        {
            Value = Mathf.Clamp(value, minValue, maxValue);
        }

        public override string GetDisplayText()
        {
            return Value.ToString();
        }

        public override string GetCardType()
        {
            return "Number";
        }

        public override Card Clone()
        {
            return new NumberCard(Value);
        }
    }
}
