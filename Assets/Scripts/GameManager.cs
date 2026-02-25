using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string CurrentScene { get; private set; }

    private Models.GameConfig config;

    private Models.Cards.Deck Deck;

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

        Events.GameEvents.OnRoundEnded += HandleRoundEnded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Events.GameEvents.OnRoundEnded -= HandleRoundEnded;
    }

    void Start()
    {
        ActorManager.Initialize();
        //RoundManager.Instance.Initialize(config);
            
        //StartGame();
    }

    private void GetCurrentSceneName()
    {
        CurrentScene = SceneManager.GetActiveScene().name;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetCurrentSceneName();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// /////////////////////////////////////////////////////
    /// </summary>

    private void StartGame()
    {

        ActorManager.SetPlayerCredits(config.StartingCredits, config.StartingCredits);

        RoundManager.Instance.StartNewRound();

    }

    private void HandleRoundEnded(Models.Round.RoundResult result)
    {

        ActorManager.SetPlayerCredits(result.PlayerScoreChange, result.AIScoreChange);

        if (ActorManager.IsPlayerNegativeBalance())
        {
            UIManager.ShowWinner("AI");
        }
        else if (ActorManager.IsAINegativeBalance())
        {
            UIManager.ShowWinner("Player");
        }

    }
}
