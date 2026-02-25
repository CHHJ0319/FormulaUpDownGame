using TMPro;
using UnityEngine;

namespace UI.GameScene
{
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resultSummaryText;
        [SerializeField] private TextMeshProUGUI resultDetailText; 

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ShowRoundResult(string summary, string detail)
        {
            gameObject.SetActive(true);

            resultSummaryText.text = summary;
            resultDetailText.text = detail;
        }
    }
}


