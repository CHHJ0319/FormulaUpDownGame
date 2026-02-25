using UnityEngine;
using UnityEngine.UI;

namespace UI.MenuScene
{
    public class MenuSceneUIController : MonoBehaviour
    {
        [Header("MenuButtons")]
        public Button storyModeButton;
        public Button challengeModeButton;
        public Button tutorialButton;
        public Button settingButton;
        public Button quitButton;

        void Start()
        {
            UIManager.Instance.SetMenuSceneUIController(this);

            storyModeButton.onClick.AddListener(OnStoryModeButtonClick);
            challengeModeButton.onClick.AddListener(OnChallengeModeButtonClick);
            tutorialButton.onClick.AddListener(OnTutorialModeButtonClick);
            settingButton.onClick.AddListener(OnSettingModeButtonClick);
            quitButton.onClick.AddListener(OnQuitModeButtonClick);
        }

        private void OnStoryModeButtonClick()
        {
            
        }

        private void OnChallengeModeButtonClick()
        {
            Util.SceneLoader.LoadSceneByName("GameScene");
        }

        private void OnTutorialModeButtonClick()
        {
            
        }

        private void OnSettingModeButtonClick()
        {
            
        }

        private void OnQuitModeButtonClick()
        {
            GameManager.Instance.QuitGame();
        }
    }
}