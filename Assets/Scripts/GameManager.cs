using Controllers;
using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        private Models.GameConfig config;

        private Models.Cards.Deck Deck;

        void Awake()
        {
            config = Models.GameConfig.Default();
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
            ActorManager.Initialize();
            RoundManager.Instance.Initialize(config);
            
            StartGame();
        }

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
