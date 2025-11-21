namespace Models.Cards
{
    public abstract class Card
    {
        public bool IsUsed { get; protected set; }

        public abstract string GetDisplayText();
        public abstract Card Clone();
        public void MarkAsUsed()
        {
            IsUsed = true;
        }

        public void MarkAsUnused()
        {
            IsUsed = false;
        }
    }
}
