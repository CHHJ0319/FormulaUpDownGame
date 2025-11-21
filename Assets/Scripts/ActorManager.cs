using UI;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    private static Actors.PlayerController player;
    private static Actors.AIController ai;

    void OnEnable()
    {
        Events.GameEvents.OnRoundStarted += HandleRoundStarted;
        Events.CardEvents.OnCardAdded += AddCard;
        Events.ButtonEvents.OnResetButtonClicked += HandleResetButtonClicked;
        Events.CardEvents.OnCardClicked += HandleCardClicked;
    }

    void OnDisable()
    {
        Events.GameEvents.OnRoundStarted -= HandleRoundStarted;
        Events.CardEvents.OnCardAdded -= AddCard;
        Events.ButtonEvents.OnResetButtonClicked -= HandleResetButtonClicked;
        Events.CardEvents.OnCardClicked -= HandleCardClicked;


    }

    public void HandleRoundStarted()
    {
        player.ResetHand();
        player.Prepare();

        ai.ResetHand();
    }

    public static void AddCard(Models.Cards.Card card, bool isPlayer)
    {
        if (isPlayer)
        {
            player.AddCard(card);

        }
        else
        {
            ai.AddCard(card);

        }
    }

    private static void HandleResetButtonClicked()
    {
        player.Prepare();
    }

    private void HandleCardClicked(Models.Cards.Card card)
    {
        player.HandleCardClicked(card);
    }

    public static void Initialize()
    {
    }

    public static void SetPlayerCredits(int playerCredits, int aiCredits)
    {
        player.Credits += playerCredits;
        ai.Credits += aiCredits;

        Events.GameEvents.InvokeScoreChanged(player.Credits, ai.Credits);
    }

    public static void SetPlayer(Actors.PlayerController controller)
    {
        player = controller;
    }

    public static void SetAi(Actors.AIController controller)
    {
        ai = controller;
    }

    public static void ExecuteAITurn(int targetScore)
    {
        ai.PlayTurn(targetScore);
    }

    public static bool IsAllSpecialCardsUsed()
    {
        return player != null && player.IsAllSpecialCardsUsed();
    }

    public static bool IsAllNumberCardsUsed()
    {
        return player != null && player.IsAllNumberCardsUsed();
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

    public static bool IsPlayerNegativeBalance()
    {
        return player.Credits <= 0;
    }
    public static bool IsAINegativeBalance()
    {
        return ai.Credits <= 0;
    }

}
