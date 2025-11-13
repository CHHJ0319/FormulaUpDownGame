using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
[RequireComponent (typeof(HorizontalLayoutGroup))]
public class HandContainer : MonoBehaviour
{
    private void Reset()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(800f, 200f);
        }

        HorizontalLayoutGroup layout = GetComponent<HorizontalLayoutGroup>();
        if (layout != null)
        {
            layout.spacing = 5f;
            layout.childForceExpandWidth = false;
        }
    }
}
