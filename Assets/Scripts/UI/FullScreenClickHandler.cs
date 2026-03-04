using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FullScreenClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            StartCoroutine(Util.SceneLoader.LoadSceneByName("MenuScene"));
        }
    }
}
