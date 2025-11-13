using TMPro;
using UnityEngine;

namespace UI
{
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resultSummaryText;
        [SerializeField] private TextMeshProUGUI resultDetailText; 

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public  void HandleRoundEnded(Models.Round.RoundResult result)
        {
            resultSummaryText.text = result.GetSummary();
            resultDetailText.text = result.GetDetail();
            gameObject.SetActive(true);
        }
    }
}


