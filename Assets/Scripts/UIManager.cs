using Models.Cards;
using System.Collections;
using TMPro;
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

    void Start()
    {

        //resultPanel.Hide();
        //playerPanel.Initialize();
        //bettingPanel.Initialize();
    }

    void OnEnable()
    {
        Events.CardEvents.OnCardAdded += HandleCardAdded;
        Events.UIEvents.OnExpressionUpdated += UpdateExpressionText;
        Events.ButtonEvents.OnResetButtonClicked += HandleResetButtonClicked;
        Events.GameEvents.OnSubmitAvailabilityChanged += UpdateSubmitAvailability;

        Events.GameEvents.OnScoreChanged += UpdateScoreText;
        Events.GameEvents.OnRoundStarted += HandleRoundStarted;
        //Events.GameEvents.OnBetChanged += bettingPanel.UpdateBetText;

        Events.RoundEvents.OnTargetScoreSet += OnTargetSet;
    }

    void OnDisable()
    {
        Events.CardEvents.OnCardAdded -= HandleCardAdded;
        Events.UIEvents.OnExpressionUpdated -= UpdateExpressionText;
        Events.ButtonEvents.OnResetButtonClicked -= HandleResetButtonClicked;
        Events.GameEvents.OnSubmitAvailabilityChanged -= UpdateSubmitAvailability;

        Events.GameEvents.OnScoreChanged -= UpdateScoreText;
        Events.GameEvents.OnRoundStarted -= HandleRoundStarted;
        //Events.GameEvents.OnBetChanged -= bettingPanel.UpdateBetText;

        Events.RoundEvents.OnTargetScoreSet -= OnTargetSet;
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

    private void HandleCardAdded(Card card, bool isPlayer)
    {
        if (isPlayer)
        {
            //playerPanel.AddCard(card);
        }
        else
        {
            //aiPanel.AddCard(card);
        }
    }

    private void UpdateExpressionText(string text)
    {
        //playerPanel.UpdateExpressionText(text);
    }

    private void HandleResetButtonClicked()
    {
       // playerPanel.ResetCardInHandUsage();
    }

    public void UpdateSubmitAvailability(bool canSubmit)
    {
        //playerPanel.UpdateSubmitButton(canSubmit);
    }

    private void UpdateScoreText(int playerScore, int aiScore)
    {
        //playerPanel.UpdateCreditsText(playerScore);
        //aiPanel.UpdateCreditsText(aiScore);
    }

    private void HandleRoundStarted()
    {
        //playerPanel.ResetHand();
        //aiPanel.ResetHand();

        //resultPanel.Hide();
        //playerPanel.UpdateExpressionText("");
        //playerPanel.UpdateSubmitButton(false);
    }

    public static void ShowWinner(string winner)
    {
        // = $"게임 종료! 최종 승자: {winner}";
    }

    private void OnTargetSet(int score)
    {
        StopAllCoroutines();
        StartCoroutine(PlayTargetScoreSequence(score));
    }

    public IEnumerator PlayTargetScoreSequence(int score)
    {
        bool isSlotFinished = false;

        //slotMachine.PlaySlot(score, () =>
        //{
        //    isSlotFinished = true;
        //});

        yield return new WaitUntil(() => isSlotFinished);

        //targetScorePanel.UpdateTargetScoreText(score);
    }
 
}
