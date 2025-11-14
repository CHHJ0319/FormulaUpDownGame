using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Models.Cards;
using MathHighLow.Services;

namespace UI
{
    public class CardButton : MonoBehaviour
    {
        [Header("카드 색상")]
        [SerializeField] private Color playerNumberCardColor = Color.white;
        [SerializeField] private Color playerOperatorCardColor = new Color(0.8f, 1f, 0.8f); // 연두색
        [SerializeField] private Color specialCardColor = new Color(1f, 0.9f, 0.5f); // 노란색
        [SerializeField] private Color aiCardColor = new Color(0.9f, 0.9f, 1f); // 연한 파란색

        private TextMeshProUGUI displayText;
        private Image backgroundImage;
        private Button button;

        private Card card;
        private bool isPlayerCard;

        void OnEnable()
        {
            GameEvents.OnRoundStarted += ResetCartButton;
            GameEvents.OnResetClicked += ResetCartButton;
            GameEvents.OnCardConsumed += HandleCardConsumed;
            GameEvents.OnSpecialCardConsumed += HandleSpecialCardConsumed;
        }

        void OnDisable()
        {
            GameEvents.OnRoundStarted -= ResetCartButton;
            GameEvents.OnResetClicked -= ResetCartButton;
            GameEvents.OnCardConsumed -= HandleCardConsumed;
            GameEvents.OnSpecialCardConsumed -= HandleSpecialCardConsumed;
        }

        private void Awake()
        {
            button = GetComponent<Button>();
            backgroundImage = GetComponent<Image>();
            displayText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Initialize(Card card, bool isPlayer)
        {
            this.card = card;
            this.isPlayerCard = isPlayer;

            if (card == null)
            {
                return;
            }

            SetCardButtonText();
            SetCardButtonColor();
            SetEffect();
        }

        private void HandleCardConsumed(Card usedCard)
        {
            if (usedCard == this.card)
            {
                if (card is SpecialCard specialCard &&
                    IsClickableSpecial(card) &&
                    !specialCard.IsConsumed)
                {
                    button.interactable = true;
                    backgroundImage.color = specialCardColor;
                    return;
                }

                button.interactable = false;
                backgroundImage.color = Color.gray;
            }
        }

        private void HandleSpecialCardConsumed(SpecialCard consumedCard)
        {
            if (card == consumedCard)
            {
                button.interactable = false;
                backgroundImage.color = Color.gray;
            }
        }

        private void ResetCartButton()
        {
            if (isPlayerCard && (card is NumberCard || card is OperatorCard))
            {
                button.interactable = true;

                if (card is NumberCard)
                {
                    backgroundImage.color = playerNumberCardColor;
                }
                else if (card is OperatorCard)
                {
                    backgroundImage.color = playerOperatorCardColor;
                }
            }
            else if (isPlayerCard && card is SpecialCard specialCard &&
                     IsClickableSpecial(card))
            {
                specialCard.MarkAsUnused();
                button.interactable = true;
                backgroundImage.color = specialCardColor;
            }
        }

        private void SetCardButtonText()
        {
            displayText.text = card.GetDisplayText();
        }

        private void SetCardButtonColor()
        {
            if (!isPlayerCard)
            {
                backgroundImage.color = aiCardColor;
            }
            else if (card is NumberCard)
            {
                backgroundImage.color = playerNumberCardColor;
            }
            else if (card is OperatorCard)
            {
                backgroundImage.color = playerOperatorCardColor;
            }
            else if (card is SpecialCard)
            {
                backgroundImage.color = specialCardColor;
            }
        }

        private void SetEffect()
        {
            button.onClick.RemoveAllListeners();
            if (isPlayerCard && (card is NumberCard || card is OperatorCard || IsClickableSpecial(card)))
            {
                button.interactable = true;
                button.onClick.AddListener(HandleClick);
            }
            else
            {
                button.interactable = false;
            }
        }

        private void HandleClick()
        {
            if (card == null)
            {
                return;
            }

            if (card is SpecialCard specialCard && !IsClickableSpecial(card))
            {
                return;
            }

            GameEvents.InvokeCardClicked(this.card);
        }

        private bool IsClickableSpecial(Card targetCard)
        {
            if (targetCard is SpecialCard specialCard)
            {
                return specialCard.Type == Algorithm.Operator.OperatorType.Multiply ||
                       specialCard.Type == Algorithm.Operator.OperatorType.SquareRoot;
            }

            return false;
        }
    }
}