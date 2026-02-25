using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public static ActorManager Instance { get; private set; }

    private static Actors.PlayerController player;
    private static Actors.AIController ai;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        Events.ButtonEvents.OnResetButtonClicked += HandleResetButtonClicked;
        Events.CardEvents.OnCardClicked += HandleCardClicked;
    }

    void OnDisable()
    {
        Events.ButtonEvents.OnResetButtonClicked -= HandleResetButtonClicked;
        Events.CardEvents.OnCardClicked -= HandleCardClicked;
    }

    public void StartRound()
    {
        player.ResetHand();
        player.Prepare();

        ai.ResetHand();
    }

    public void AddCardInPlayerHand(Models.Cards.Card card)
    {
        player.AddCard(card);
        UIManager.Instance.AddCardInPlayerHand(card);
    }

    public void AddCardInAIHand(Models.Cards.Card card)
    {
        ai.AddCard(card);
        UIManager.Instance.AddCardInAIHand(card);
    }

    private void HandleResetButtonClicked()
    {
        player.Prepare();
    }

    private void HandleCardClicked(Models.Cards.Card card)
    {
        player.HandleCardClicked(card);
    }

    public void SetCredits(int playerCredits, int aiCredits)
    {
        player.Credits += playerCredits;
        ai.Credits += aiCredits;

        UIManager.Instance.UpdateCredits(player.Credits, ai.Credits);
    }

    public void SetPlayer(Actors.PlayerController controller)
    {
        player = controller;
    }

    public void SetAi(Actors.AIController controller)
    {
        ai = controller;
    }

    public void ExecuteAITurn(int targetScore)
    {
        ai.PlayTurn(targetScore);
    }

    public bool IsAllSpecialCardsUsed()
    {
        return player != null && player.IsAllSpecialCardsUsed();
    }

    public bool IsAllNumberCardsUsed()
    {
        return player != null && player.IsAllNumberCardsUsed();
    }

    

    public Models.Expression.ValidationResult ValidatePlayerExpression()
    {
        return Algorithm.ExpressionValidator.Validate(player.GetExpression(), player.Hand);
    }

    public Models.Expression.EvaluationResult EvaluatePlayerExpression()
    {
        return Algorithm.ExpressionEvaluator.Evaluate(player.GetExpression());
    }

    public Models.Expression.EvaluationResult EvaluateAiExpression()
    {
        return Algorithm.ExpressionEvaluator.Evaluate(ai.GetExpression());
    }

    public Models.Expression.Expression GetPlayerExpression()
    {
        return player.GetExpression();
    }

    public Models.Expression.Expression GetAiExpression()
    {
        return ai.GetExpression();
    }

    public bool IsPlayerNegativeBalance()
    {
        return player.Credits <= 0;
    }
    public bool IsAINegativeBalance()
    {
        return ai.Credits <= 0;
    }

}
