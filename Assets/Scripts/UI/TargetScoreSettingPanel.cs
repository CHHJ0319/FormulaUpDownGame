using TMPro;
using Unity.VisualScripting;
using UnityEngine;


namespace UI
{
    public class TargetScorePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI targetScoreText;

        public  void UpdateTargetScoreText(int targetValue)
        {
            targetScoreText.text = "" + targetValue;
        }
    }
}
