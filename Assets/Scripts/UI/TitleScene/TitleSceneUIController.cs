using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TitleScene
{
    public class TitleSceneUIController : MonoBehaviour
    {

        void Awake()
        {

        }

        void Start()
        {
            UIManager.Instance.SetTitleSceneUIController(this);
        }
    }
}