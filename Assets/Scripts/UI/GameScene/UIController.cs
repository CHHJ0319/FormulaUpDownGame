using TMPro;
using UnityEngine;

namespace UI.GameScene
{
    public class UIController : MonoBehaviour
    {
        public Transform timer;
        public TextMeshProUGUI statusText;

        public SlotMachineUI slotMachine;
        public Transform targetScorePanel;
        public ResultPanel resultPanel;
        public BettingPanel bettingPanel;

        [Header("Actor UI")]
        public PlayerPanel playerPanel;
        public AiPanel aiPanel;

        void Start()
        {
            UIManager.Instance.SetGameSceneUIController(this);
        }

        public void UpdateTimerText(float currentTime, float maxTime)
        {
            TextMeshProUGUI timerText = timer.GetChild(0).GetComponent<TextMeshProUGUI>();
            float remainingTime = maxTime - currentTime;

            if (remainingTime < 0)
            {
                timerText.text = "00:00";
            }
            else
            {
                if (remainingTime <= 30)
                {
                    timerText.color = Color.red;
                }
                else
                {
                    timerText.color = Color.white;
                }

                int minutes = Mathf.FloorToInt(remainingTime / 60f);
                int seconds = Mathf.FloorToInt(remainingTime % 60f);

                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }

        private void UpdateStatusText(string message)
        {
            statusText.text = message;

            if (message.Contains("분배"))
            {
                statusText.color = Color.black;
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
                statusText.color = Color.black;
            }
        }

        public void UpdateTargetScoreText(int targetValue)
        {
            TextMeshProUGUI targetScoreText = targetScorePanel.GetChild(0).GetComponent< TextMeshProUGUI>();

            targetScoreText.text = "" + targetValue;
        }
    }
}
