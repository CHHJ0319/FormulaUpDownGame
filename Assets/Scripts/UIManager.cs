using UnityEngine;
using TMPro;
using Models.Cards;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UI.PlayerPanel playerPanel;
    [SerializeField] private UI.AiPanel aiPanel;
    [SerializeField] private UI.BettingPanel bettingPanel;
    [SerializeField] private UI.ResultPanel resultPanel;
    [SerializeField] private UI.TargetScoreSettingPanel targetScoreSettingPanel;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI statusText;


    #region 이벤트 구독 (OnEnable / OnDisable)

    void OnEnable()
    {
        // 점수
        Events.GameEvents.OnScoreChanged += UpdateScoreText;

        // 라운드 진행
        Events.GameEvents.OnRoundStarted += HandleRoundStarted;
        Events.GameEvents.OnCardAdded += HandleCardAdded;
        Events.GameEvents.OnRoundEnded += resultPanel.HandleRoundEnded;

        // 플레이어 입력
        Events.GameEvents.OnBetChanged += bettingPanel.UpdateBetText;
        Events.GameEvents.OnTimerUpdated += UpdateTimerText;

        // 제출 가능 여부
        Events.GameEvents.OnSubmitAvailabilityChanged += playerPanel.UpdateSubmitAvailability;

        Events.UIEvents.OnStatusTextUpdated += UpdateStatusText;

        // 게임 종료
        Events.GameEvents.OnGameOver += HandleGameOver;
    }

    void OnDisable()
    {
        Events.GameEvents.OnScoreChanged -= UpdateScoreText;
        Events.GameEvents.OnRoundStarted -= HandleRoundStarted;
        Events.GameEvents.OnCardAdded -= HandleCardAdded;
        Events.GameEvents.OnRoundEnded -= resultPanel.HandleRoundEnded;
        Events.GameEvents.OnBetChanged -= bettingPanel.UpdateBetText;
        Events.GameEvents.OnTimerUpdated -= UpdateTimerText;
        Events.GameEvents.OnSubmitAvailabilityChanged -= playerPanel.UpdateSubmitAvailability;
        Events.UIEvents.OnStatusTextUpdated -= UpdateStatusText;
        Events.GameEvents.OnGameOver -= HandleGameOver;
    }

    #endregion

    void Start()
    {
            
        UpdateScoreText(0, 0);
        UpdateTimerText(0, 180);

        resultPanel.Hide();
        playerPanel.Initialize();
        bettingPanel.Initialize();
        targetScoreSettingPanel.Initialize();
    }

    #region 로직 핸들러 (Event -> UI)

    private void UpdateScoreText(int playerScore, int aiScore)
    {
        playerPanel.UpdateCreditsText(playerScore);
        aiPanel.UpdateCreditsText(aiScore);
    }

    private void HandleRoundStarted()
    {
        playerPanel.ResetHand();
        aiPanel.ResetHand();

        resultPanel.Hide();
        playerPanel.UpdateExpressionText("");
        playerPanel.DisableSubmitButton();

        UpdateTimerText(0, 180);
    }

    private void HandleCardAdded(Card card, bool isPlayer)
    {
        if(isPlayer)
        {
            playerPanel.HandleCardAdded(card);
        }
        else
        {
            aiPanel.HandleCardAdded(card);
        }   
    }

    private void UpdateTimerText(float currentTime, float maxTime)
    {
        float remainingTime = maxTime - currentTime;

        if (remainingTime < 0)
        {
            timerText.text = "00:00";
            timerText.color = Color.red;
        }
        else
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";

            // 30초 이하면 빨간색으로 경고
            if (remainingTime <= 30)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }
        }
    }

    private void HandleGameOver(string winner)
    {
        statusText.text = $"게임 종료! 최종 승자: {winner}";
    }

    private void UpdateStatusText(string message)
    {
        statusText.text = message;

        if (message.Contains("분배"))
        {
            statusText.color = Color.white;
        }
        else if (message.Contains("완성하세요"))
        {
            statusText.color = Color.cyan;
        }
        else if (message.Contains("제출"))
        {
            statusText.color = Color.green;
        }
        else if (message.Contains("결과"))
        {
            statusText.color = Color.yellow;
        }
        else
        {
            statusText.color = Color.white;
        }
    }

    #endregion
}
