using UnityEngine;

public class ActorManager : MonoBehaviour
{
    private static Controllers.PlayerController player;
    private static Controllers.AIController ai;

    private static float dealInterval;

    public static void Initialize(float interval)
    {
        dealInterval = interval;
    }

    public static void SetPlayerCredits(int playerCredits, int aiCredits)
    {
        player.Credits += playerCredits;
        ai.Credits += aiCredits;

        Events.GameEvents.InvokeScoreChanged(player.Credits, ai.Credits);
    }

    public static void SetPlayer(Controllers.PlayerController controller)
    {
        player = controller;
    }

    public static void SetAi(Controllers.AIController controller)
    {
        ai = controller;
    }

    public static void ExecuteAITurn(int targetScore)
    {
        ai.PlayTurn(targetScore);
    }

    public static bool IsSpecialCardRequirementMet()
    {
        return player != null && player.HasUsedRequiredSpecialCards();
    }

    public static bool ShouldShowSpecialCardReminder()
    {
        return player != null && player.NeedsSpecialCardUsageReminder();
    }

    public static void AddOperatorCardToPlayer(Algorithm.Operator.OperatorType op)
    {
        Algorithm.Operator opr = new Algorithm.Operator(op);
        Models.Cards.OperatorCard operatorCard = new Models.Cards.OperatorCard(opr);

        AddCardToPlayer(operatorCard);
    }

    public static void AddOperatorCardToAI(Algorithm.Operator.OperatorType op)
    {
        Algorithm.Operator opr = new Algorithm.Operator(op);
        Models.Cards.OperatorCard operatorCard = new Models.Cards.OperatorCard(opr);

        AddCardToAi(operatorCard);
    }

    public static void AddCardToPlayer(Models.Cards.Card card)
    {
        player.AddCard(card);
        Events.GameEvents.InvokeCardAdded(card, true);
    }

    public static void AddCardToAi(Models.Cards.Card card)
    {
        ai.AddCard(card);
        Events.GameEvents.InvokeCardAdded(card, false);
    }

    public static Models.Expression.ValidationResult ValidatePlayerExpression()
    {
        return Algorithm.ExpressionValidator.Validate(player.GetExpression(), player.Hand);
    }

    public static Models.Expression.EvaluationResult EvaluatePlayerExpression()
    {
        return Algorithm.ExpressionEvaluator.Evaluate(player.GetExpression());
    }

    public static Models.Expression.EvaluationResult EvaluateAiExpression()
    {
        return Algorithm.ExpressionEvaluator.Evaluate(ai.GetExpression());
    }

    public static Models.Expression.Expression GetPlayerExpression()
    {
        return player.GetExpression();
    }

    public static Models.Expression.Expression GetAiExpression()
    {
        return ai.GetExpression();
    }
    public static void PreparePlayer()
    {
        player.Prepare();
    }   

    public static bool IsPlayerNegativeBalance()
    {
        return player.Credits <= 0;
    }
    public static bool IsAINegativeBalance()
    {
        return ai.Credits <= 0;
    }

}
