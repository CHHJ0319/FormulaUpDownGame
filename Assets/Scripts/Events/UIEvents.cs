using System;

namespace Events
{
    public static class UIEvents
    {
        public static event Action<string> OnStatusTextUpdated;
        public static event Action<string> OnExpressionUpdated;

        public static void ClearUIEvents()
        {
            OnStatusTextUpdated = null;
            OnExpressionUpdated = null;

        }
        public static void InvokeStatusTextUpdated(string statusMessage)
        {
            OnStatusTextUpdated?.Invoke(statusMessage);
        }

        public static void InvokeExpressionUpdated(string expressionText)
        {
            OnExpressionUpdated?.Invoke(expressionText);
        }
    }
}

