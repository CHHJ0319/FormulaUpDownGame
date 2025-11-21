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
        private List<CardButton> cardsInHand = new List<CardButton>();

        [SerializeField] private Button submitButton;
        [SerializeField] private Button resetButton;

        public void Initialize()
        {
            submitButton.onClick.AddListener(() => Events.GameEvents.InvokeSubmit());
            resetButton.onClick.AddListener(() => Events.ButtonEvents.ResetPlayerHand());

            UpdateExpressionText("");
        }

        public void AddCard(Card card)
        {
            CardButton newCardButton = Instantiate(cardPrefab, handContainer.transform);
            newCardButton.Initialize(card, true);
            cardsInHand.Add(newCardButton);
        }

        public void UpdateExpressionText(string text)
        {
            expressionText.text = string.IsNullOrEmpty(text) ? "..." : text;
        }

        public void ResetCardInHandUsage()
        {
            foreach (var card in cardsInHand)
            {
                card.ResetCardButton();
            }

            UpdateExpressionText("");
        }

        public void UpdateCreditsText(int credits)
        {
            creditsText.text = $"{credits}";
        }

        public void ResetHand()
        {
            foreach (var card in cardsInHand)
            {
                Destroy(card.gameObject);
            }
            cardsInHand.Clear();
        }

        public void UpdateSubmitButton(bool canSubmit)
        {
            var colors = submitButton.colors;
            colors.normalColor = canSubmit ? Color.white : Color.gray;
            colors.highlightedColor = canSubmit ? Color.white : Color.gray;
            colors.selectedColor = canSubmit ? Color.white : Color.gray;
            submitButton.colors = colors;
        }
    }
}

