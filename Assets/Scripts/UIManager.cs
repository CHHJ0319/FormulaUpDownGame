using Models.Cards;
using UI.GameScene;
using UI.MenuScene;
using UI.TitleScene;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private TitleSceneUIController titleSceneUIController;
    private MenuSceneUIController menuSceneUIController;
    private UI.GameScene.UIController gameSceneUIController;

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
        Events.GameEvents.OnBetChanged += UpdateBettingText;
        Events.UIEvents.OnStatusTextUpdated += UpdateStatusText;
        Events.UIEvents.OnExpressionUpdated += UpdateExpression;
        Events.ButtonEvents.OnResetButtonClicked += HandleResetButtonClicked;
    }

    void OnDisable()
    {
        Events.GameEvents.OnBetChanged -= UpdateBettingText;
        Events.UIEvents.OnStatusTextUpdated -= UpdateStatusText;
        Events.UIEvents.OnExpressionUpdated -= UpdateExpression;
        Events.ButtonEvents.OnResetButtonClicked -= HandleResetButtonClicked;
    }

    public void SetTitleSceneUIController(TitleSceneUIController controller)
    {
        titleSceneUIController = controller;
    }

    public void SetMenuSceneUIController(MenuSceneUIController controller)
    {
        menuSceneUIController = controller;
    }

    public void SetGameSceneUIController(UI.GameScene.UIController controller)
    {
        gameSceneUIController = controller;
    }

    public void InitializeRound()
    {
        if(gameSceneUIController != null)
        {
            gameSceneUIController.InitializeRound();
        }
    }

    public void SetTargetScore(int score)
    {
        StopAllCoroutines();
        StartCoroutine(gameSceneUIController.PlayTargetScoreSequence(score));
    }

    public void AddCardInPlayerHand(Models.Cards.Card card)
    {
        gameSceneUIController.AddCardInPlayerHand(card);
    }

    public void AddCardInAIHand(Models.Cards.Card card)
    {
        gameSceneUIController.AddCardInAIHand(card);
    }

    public void UpdateTimer(float currentTime, float maxTime)
    {
        gameSceneUIController.UpdateTimer(currentTime, maxTime); ;
    }

    public void UpdateSubmitAvailability(bool canSubmit)
    {
        gameSceneUIController.UpdateSubmitAvailability(canSubmit);
    }

    private void UpdateBettingText(int bet)
    {
        gameSceneUIController.UpdateBettingText(bet);
    }

    private void UpdateStatusText(string message)
    {
        gameSceneUIController.UpdateStatusText(message);
    }

    private void UpdateExpression(string text)
    {
        gameSceneUIController.UpdateExpression(text);
    }

    private void HandleResetButtonClicked()
    {
        gameSceneUIController.ResetCardInPlayerHandUsage();
    }

    public void UpdateCredits(int playerCredits, int aiCredits)
    {
        gameSceneUIController.UpdateCredits(playerCredits, aiCredits);
    }

    public void ShowRoundResult(Models.Round.RoundResult result)
    {
        gameSceneUIController.ShowRoundResult(result.GetSummary(), result.GetDetail());
    }

    public static void ShowWinner(string winner)
    {
        // = $"게임 종료! 최종 승자: {winner}";
    }
}
