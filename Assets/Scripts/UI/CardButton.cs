using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Models.Cards;

namespace UI
{
    public class CardButton : MonoBehaviour
    {
        [Header("Cadrd Color")]
        [SerializeField] private Color playerNumberCardColor = Color.white;
        [SerializeField] private Color playerOperatorCardColor = new Color(0.8f, 1f, 0.8f);
        [SerializeField] private Color specialCardColor = new Color(1f, 0.9f, 0.5f);
        [SerializeField] private Color aiCardColor = new Color(0.9f, 0.9f, 1f);

        private TextMeshProUGUI displayText;
        private Image backgroundImage;
        private Button button;

        private Card card;
        private bool isPlayerCard;

        void OnEnable()
        {
            Events.GameEvents.OnResetClicked += ResetCardButton;
            Events.CardEvents.OnCardConsumed += HandleCardUsed;
        }

        void OnDisable()
        {
            Events.GameEvents.OnResetClicked -= ResetCardButton;
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
            if (card == null)
            {
                return;
            }

            this.card = card;
            this.isPlayerCard = isPlayer;

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
        
        private void ResetCardButton()
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
                button.onClick.AddListener(HandleClicked);
            }
            else
            {
                button.interactable = false;
            }
        }

        private void HandleClicked()
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