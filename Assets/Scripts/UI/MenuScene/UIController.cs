using UnityEngine;
using UnityEngine.UI;

namespace UI.MenuScene
{
    public class UIController : MonoBehaviour
    {
        [Header("MenuButtons")]
        public Button storyModeButton;
        public Button challengeModeButton;
        public Button tutorialButton;
        public Button settingButton;
        public Button quitButton;

        void Start()
        {
            UIManager.Instance.SetUIController(this);

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
            MenuButton button = challengeModeButton.GetComponent<MenuButton>();
            button.PlayClickSound();

            StartCoroutine(Util.SceneLoader.LoadSceneByName("GameScene"));
        }

        private void OnTutorialModeButtonClick()
        {
            
        }

        private void OnSettingModeButtonClick()
        {
            
        }

        private void OnQuitModeButtonClick()
        {
            MenuButton button = challengeModeButton.GetComponent<MenuButton>();
            button.PlayClickSound();

            Events.GameEvents.QuitGame();
        }
    }
}