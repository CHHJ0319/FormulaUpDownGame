using Models.Cards;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class AiPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI creditsText;

        [SerializeField] private HandContainer handContainer;
        [SerializeField] private CardButton cardPrefab;
        private List<GameObject> cardsInHand = new List<GameObject>();

        public void UpdateCreditsText(int credits)
        {
            creditsText.text = $"{credits}";
        }
        public void HandleCardAdded(Card card)
        {
            CardButton newCardButton = Instantiate(cardPrefab, handContainer.transform);
            newCardButton.Initialize(card, false);
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

    }
}

