using Models.Cards;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI creditsText;
        [SerializeField] private TextMeshProUGUI expressionText;

        [SerializeField] private HandContainer handContainer;
        [SerializeField] private CardButton cardPrefab;
        private List<GameObject> cardsInHand = new List<GameObject>();

        [SerializeField] private Button submitButton;
        [SerializeField] private Button resetButton;

        void OnEnable()
        {
            Events.UIEvents.OnExpressionUpdated += UpdateExpressionText;
            Events.GameEvents.OnSubmitAvailabilityChanged += UpdateSubmitAvailability;


        }

        void OnDisable()
        {
            Events.UIEvents.OnExpressionUpdated -= UpdateExpressionText;
            Events.GameEvents.OnSubmitAvailabilityChanged -= UpdateSubmitAvailability;
        }

        public void Initialize()
        {
            submitButton.onClick.AddListener(() => Events.GameEvents.InvokeSubmit());
            resetButton.onClick.AddListener(() => Events.GameEvents.InvokeReset());

            UpdateExpressionText("");
        }

        public void UpdateCreditsText(int credits)
        {
            creditsText.text = $"{credits}";
        }

        public void HandleCardAdded(Card card)
        {
            CardButton newCardButton = Instantiate(cardPrefab, handContainer.transform);
            newCardButton.Initialize(card, true);
            cardsInHand.Add(newCardButton.gameObject);
        }

        public void ResetHand()
        {
            foreach (var card in cardsInHand)
            {
                Destroy(card);
            }
            cardsInHand.Clear();
        }

        public void UpdateExpressionText(string text)
        {
            expressionText.text = string.IsNullOrEmpty(text) ? "..." : text;
        }

        public void UpdateSubmitAvailability(bool canSubmit)
        {
            var colors = submitButton.colors;
            colors.normalColor = canSubmit ? Color.white : Color.gray;
            colors.highlightedColor = canSubmit ? Color.white : Color.gray;
            colors.selectedColor = canSubmit ? Color.white : Color.gray;
            submitButton.colors = colors;
        }
    }
}

