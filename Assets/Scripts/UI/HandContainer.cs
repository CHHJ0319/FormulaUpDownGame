using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public class HandContainer : MonoBehaviour
{
    private void Reset()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(800f, 200f);
        }
    }
}
