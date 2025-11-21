using System;

namespace Events
{
    public static class ButtonEvents
    {
        public static event Action OnResetButtonClicked;

        public static void ClearEvents()
        {
            OnResetButtonClicked = null;
        }

        public static void ResetPlayerHand()
        {
            OnResetButtonClicked?.Invoke();
        }
    }
}

