using System;
using Models.Cards;

namespace Events
{
    public static class GameEvents
    {
        public static event Action<Algorithm.Operator.OperatorType> OnOperatorSelected;
        public static event Action OnSquareRootClicked;
        public static event Action<Algorithm.Operator.OperatorType> OnOperatorDisabled;

        public static event Action OnSubmitClicked;

        public static event Action<int> OnTargetSelected;
        public static event Action<int> OnBetChanged;

        public static event Action<bool> OnSubmitAvailabilityChanged; // canSubmit

        public static void ClearAllEvents()
        {
            CardEvents.ClearCarddEvents();
            OnOperatorSelected = null;
            OnSquareRootClicked = null;
            OnOperatorDisabled = null;
            OnSubmitClicked = null;
            OnTargetSelected = null;
            OnBetChanged = null;
            OnSubmitAvailabilityChanged = null;
        }

        public static void InvokeOperatorSelected(Algorithm.Operator.OperatorType op)
        {
            OnOperatorSelected?.Invoke(op);
        }

        public static void InvokeSquareRootClicked()
        {
            OnSquareRootClicked?.Invoke();
        }

        public static void InvokeSubmit()
        {
            OnSubmitClicked?.Invoke();
        }

        public static void InvokeBetChanged(int bet)
        {
            OnBetChanged?.Invoke(bet);
        }

        public static void InvokeOperatorDisabled(Algorithm.Operator.OperatorType op)
        {
            OnOperatorDisabled?.Invoke(op);
        }

        public static void InvokeSubmitAvailabilityChanged(bool canSubmit)
        {
            OnSubmitAvailabilityChanged?.Invoke(canSubmit);
        }
    }
}