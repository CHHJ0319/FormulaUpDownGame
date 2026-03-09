using Algorithm;
using Models.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CardButton : MonoBehaviour
    {
        public Sprite[] backgroundImageList;
        public Sprite[] numberImageList;
        public Sprite[] OperatorImageList;

        [Header("Cadrd Color")]
        [SerializeField] private Color aiCardColor = new Color(0.9f, 0.9f, 1f);

        public Image cardImage;

        private TextMeshProUGUI displayText;
        private Image backgroundImage;
        private Button button;

        private Card card;
        private bool isPlayerCard;

        void OnEnable()
        {
            Events.CardEvents.OnCardUsed += HandleCardUsed;
        }

        void OnDisable()
        {
            Events.CardEvents.OnCardUsed -= HandleCardUsed;
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

            cardImage.gameObject.SetActive(true);
            if (card is NumberCard numberCard)
            {
                int num = numberCard.Value;
                SetNumber(num);
            }
            else if (card is OperatorCard operatorCard)
            {
                Algorithm.Operator opt = operatorCard.Operator;
                SetOperator(opt);
            }
            else if (card is SpecialCard specialCard)
            {
                Algorithm.Operator opt = specialCard.Operator;
                SetOperator(opt);
            }

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
        
        public void ResetCardButton()
        {
            button.interactable = true;
            card.MarkAsUnused();

            SetCardButtonColor();
        }

        private void SetNumber(int value)
        {
            cardImage.sprite = numberImageList[value];
        }

        private void SetOperator(Algorithm.Operator type)
        {
            if (type.Type == Algorithm.Operator.OperatorType.Add)
            {
                cardImage.sprite = OperatorImageList[0];
            }
            else if (type.Type == Algorithm.Operator.OperatorType.Subtract)
            {
                cardImage.sprite = OperatorImageList[1];
            }
            else if (type.Type == Algorithm.Operator.OperatorType.Multiply)
            {
                cardImage.sprite = OperatorImageList[2];
            }
            else if (type.Type == Algorithm.Operator.OperatorType.Divide)
            {
                cardImage.sprite = OperatorImageList[3];
            }
            else if (type.Type == Algorithm.Operator.OperatorType.SquareRoot)
            {
                cardImage.sprite = OperatorImageList[4];
            }

        }

        private void SetCardButtonColor()
        {
            if (!isPlayerCard)
            {
                backgroundImage.color = aiCardColor;
            }
            else if (card is NumberCard)
            {
                backgroundImage.sprite = backgroundImageList[0];
            }
            else if (card is OperatorCard)
            {
                backgroundImage.sprite = backgroundImageList[1];
            }
            else if (card is SpecialCard)
            {
                backgroundImage.sprite = backgroundImageList[2];
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