using MathHighLow.Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetScoreSettingPanel : MonoBehaviour
{
    [SerializeField] private List<Button> targetScoreButtons;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.blue;

    void Start()
    {
        if (targetScoreButtons.Count > 0 && targetScoreButtons[0] != null)
            targetScoreButtons[0].onClick.AddListener(() => SetTargetScore(0, 1));
        if (targetScoreButtons.Count > 1 && targetScoreButtons[1] != null)
            targetScoreButtons[1].onClick.AddListener(() => SetTargetScore(1, 20));
    }

    private void SetTargetScore(int buttonIndex, int targetValue)
    {
        ChangeButtonsColor(buttonIndex);

        GameEvents.InvokeTargetSelected(targetValue);
    }

    private void ChangeButtonsColor(int buttonIndex)
    {
        for (int i = 0; i < targetScoreButtons.Count; i++)
        {
            if (targetScoreButtons[i] == null) continue;

            ColorBlock colors = targetScoreButtons[i].colors;
            colors.normalColor = (i == buttonIndex) ? selectedColor : normalColor;
            colors.selectedColor = (i == buttonIndex) ? selectedColor : normalColor;

            targetScoreButtons[i].colors = colors;
        }
    }
}
