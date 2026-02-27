using System;
using Models.Cards;

namespace Events
{
    public static class GameEvents
    {
        public static event Action OnSubmitClicked;
        public static event Action<int> OnBetChanged;

        public static event Action OnQuitGame;

        public static void ClearAllEvents()
        {
            OnSubmitClicked = null;
            OnBetChanged = null;
        }

        public static void InvokeSubmit()
        {
            OnSubmitClicked?.Invoke();
        }

        public static void InvokeBetChanged(int bet)
        {
            OnBetChanged?.Invoke(bet);
        }

        public static void QuitGame()
        {
            OnQuitGame?.Invoke();
        }
    }
}