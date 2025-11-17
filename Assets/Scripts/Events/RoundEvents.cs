using System;

namespace Events
{
    public static class RoundEvents
    {
        public static event Action<int> OnTargetScoreSet;

        public static void ClearEvents()
        {
            OnTargetScoreSet = null;
        }

        public static void InvokeTargetScoreSet(int score)
        {
            OnTargetScoreSet?.Invoke(score);
        }
    }
}

