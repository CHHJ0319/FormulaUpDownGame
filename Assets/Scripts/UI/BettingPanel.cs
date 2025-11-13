using MathHighLow.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class BettingPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI betAmountText;
        [SerializeField] private Button increaseBetButton;
        [SerializeField] private Button decreaseBetButton;
     
        private int betAmount;

        void Start()
        {
            increaseBetButton.onClick.AddListener(HandleBetIncrease);
            decreaseBetButton.onClick.AddListener(HandleBetDecrease);
        }

        private void HandleBetIncrease()
        {
            betAmount++;

            if (betAmount > 5)
            {
                betAmount = 5;
            }

            GameEvents.InvokeBetChanged(betAmount);
        }

        private void HandleBetDecrease()
        {
            betAmount--;
            if (betAmount < 1)
            {
                betAmount = 1;
            }
            GameEvents.InvokeBetChanged(betAmount);
        }

        public  void UpdateBetText(int newBet)
        {
            betAmount = newBet;
            betAmountText.text = $"${betAmount}";

            if (betAmount >= 5)
            {
                increaseBetButton.interactable = false;
            }
            else
            {
                increaseBetButton.interactable = true;
            }

            if (betAmount <= 1)
            {
                decreaseBetButton.interactable = false;
            }
            else
            {
                decreaseBetButton.interactable = true;
            }
        }
    }
}

