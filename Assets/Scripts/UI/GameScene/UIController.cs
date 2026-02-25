using Models.Cards;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.GameScene
{
    public class UIController : MonoBehaviour
    {
        public Transform timer;
        public TextMeshProUGUI statusText;

        public SlotMachineUI slotMachine;
        public ResultPanel resultPanel;
        public BettingPanel bettingPanel;

        [Header("Actor UI")]
        public PlayerPanel playerPanel;
        public AiPanel aiPanel;

        void Awake()
        {
            Initialize();
            UIManager.Instance.SetGameSceneUIController(this);
        }

        private void Initialize()
        {
            resultPanel.Hide();
            playerPanel.Initialize();
            bettingPanel.Initialize();
        }

        public void UpdateTimer(float currentTime, float maxTime)
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

        public void UpdateStatusText(string message)
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

        public IEnumerator PlayTargetScoreSequence(int score)
        {
            bool isSlotFinished = false;

            slotMachine.PlaySlot(score, () =>
            {
                isSlotFinished = true;
            });

            yield return new WaitUntil(() => isSlotFinished);
        }

        public void UpdateBettingText(int bet)
        {
            if (bettingPanel != null)
            {
                bettingPanel.UpdateBetText(bet);
            }
        }

        public void InitializeRound()
        {
            if(playerPanel == null
               || aiPanel == null) return;

            playerPanel.ResetHand();
            playerPanel.UpdateExpression("");
            playerPanel.UpdateSubmitButton(false);

            aiPanel.ResetHand();

            resultPanel.Hide();            
        }

        public void AddCardInPlayerHand(Card card)
        {
            playerPanel.AddCard(card);
        }

        public void AddCardInAIHand(Card card)
        {
            aiPanel.AddCard(card);
        }

        public void UpdateSubmitAvailability(bool canSubmit)
        {
            playerPanel.UpdateSubmitButton(canSubmit);
        }

        public void UpdateExpression(string text)
        {
            playerPanel.UpdateExpression(text);
        }

        public void ResetCardInPlayerHandUsage()
        {
            playerPanel.ResetCardInHandUsage();
        }

        public void UpdateCredits(int playerCredits, int aiCredits)
        {
            playerPanel.UpdateCreditsText(playerCredits);
            aiPanel.UpdateCreditsText(aiCredits);
        }

        public void ShowRoundResult(string summary, string detail)
        {
            resultPanel.ShowRoundResult(summary, detail);
        }
    }
}
