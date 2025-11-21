using Controllers;
using MathHighLow.Controllers;
using System.Collections;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    private static Controllers.PlayerController player;
    private static AIController ai;

    private static float dealInterval;

    public static void Initialize(float interval)
    {
        dealInterval = interval;
    }

    public static void SetPlayer(Controllers.PlayerController controller)
    {
        player = controller;
    }

    public static void SetAi(AIController controller)
    {
        ai = controller;
    }

    public static void ExecuteAITurn(Models.Hand hand, int targetScore)
    {
        ai.PlayTurn(hand, targetScore);
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

    public static void AddCardToPlayer(Models.Cards.Card card)
    {
        player.AddCard(card);
        Events.GameEvents.InvokeCardAdded(card, true);
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
}
