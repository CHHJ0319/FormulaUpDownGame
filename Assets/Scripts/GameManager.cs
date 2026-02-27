using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string CurrentScene { get; private set; }

    private Models.GameConfig config;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            GetCurrentSceneName();
            config = Models.GameConfig.Default();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        Events.GameEvents.OnQuitGame += QuitGame;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    
        Events.GameEvents.OnQuitGame -= QuitGame;
    }

    private void GetCurrentSceneName()
    {
        CurrentScene = SceneManager.GetActiveScene().name;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetCurrentSceneName();

        if(CurrentScene == "GameScene")
        {
            RoundManager.Instance.Initialize(config);
            StartRound();
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void StartRound()
    {
        ActorManager.Instance.SetCredits(config.StartingCredits, config.StartingCredits);
        RoundManager.Instance.StartNewRound();
    }
}
