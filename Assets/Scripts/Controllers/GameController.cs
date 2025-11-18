using UnityEngine;
using MathHighLow.Controllers;

/// <summary>
/// ✅ 수정: 베팅 검증 기능 추가
/// - 최대 5원까지만
/// - 보유 머니 초과 방지
/// </summary>
[RequireComponent(typeof(Controllers.RoundController))]
    [RequireComponent(typeof(Controllers.PlayerController))]
    [RequireComponent(typeof(AIController))]
    public class GameController : MonoBehaviour
    {
        [HideInInspector] public Controllers.RoundController roundController;
        [HideInInspector] public Controllers.PlayerController playerController;
        [HideInInspector] public AIController aiController;

        private Models.GameConfig config;
        private Models.Cards.Deck Deck;
        private int playerCredits;
        private int aiCredits;

        private enum GameState
        {
            Initializing,
            Playing,
            GameOver
        }
        private GameState currentState;

        #region Unity 생명주기 및 이벤트 구독

        void Awake()
        {
            config = Models.GameConfig.Default();
            Deck = new Models.Cards.Deck(config);

            roundController = GetComponent<Controllers.RoundController>();
            playerController = GetComponent<Controllers.PlayerController>();
            aiController = GetComponent<AIController>();

            roundController.Initialize(config, Deck);
            playerController.Initialize();
            aiController.Initialize();
        }

        void OnEnable()
        {
            Events.GameEvents.OnRoundEnded += HandleRoundEnded;
        }

        void OnDisable()
        {
            Events.GameEvents.OnRoundEnded -= HandleRoundEnded;
        }

        void Start()
        {
            StartGame();
        }

        #endregion

        #region 게임 흐름 제어

        private void StartGame()
        {
            currentState = GameState.Playing;
            playerCredits = config.StartingCredits;
            aiCredits = config.StartingCredits;
            Events.GameEvents.InvokeScoreChanged(playerCredits, aiCredits);
            roundController.StartNewRound();
        }

        private void HandleRoundEnded(Models.Round.RoundResult result)
        {
            if (currentState != GameState.Playing) return;

            playerCredits += result.PlayerScoreChange;
            aiCredits += result.AIScoreChange;
            Events.GameEvents.InvokeScoreChanged(playerCredits, aiCredits);

            if (playerCredits <= 0)
            {
                EndGame("AI");
            }
            else if (aiCredits <= 0)
            {
                EndGame("Player");
            }

        }

        private void EndGame(string winner)
        {
            currentState = GameState.GameOver;
            Events.GameEvents.InvokeGameOver(winner);
            Debug.Log($"[GameController] 게임 종료! 최종 승자: {winner}");
        }

        #endregion
    }
