using UnityEngine;

namespace UI.MenuScene
{
    public class MenuSceneUIController : MonoBehaviour
    {
        void Start()
        {
            UIManager.Instance.SetMenuSceneUIController(this);
        }
    }
}