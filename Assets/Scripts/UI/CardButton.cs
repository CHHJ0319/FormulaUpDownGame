using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Models.Cards;

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
            Events.GameEvents.OnRoundStarted += ResetCartButton;
            Events.GameEvents.OnResetClicked += ResetCartButton;
            Events.CardEvents.OnCardConsumed += HandleCardUsed;
        }

        void OnDisable()
        {
            Events.GameEvents.OnRoundStarted -= ResetCartButton;
            Events.GameEvents.OnResetClicked -= ResetCartButton;
            Events.CardEvents.OnCardConsumed -= HandleCardUsed;
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

        private void HandleCardUsed(Card usedCard)
        {
            if (usedCard == card)
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
            else if (isPlayerCard && card is SpecialCard specialCard)
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
            if (isPlayerCard)
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

            if (card.IsUsed)
            {
                return;
            }

            Events.CardEvents.InvokeCardClicked(this.card);
        }
    }
}