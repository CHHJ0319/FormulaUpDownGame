using UnityEngine;
using MathHighLow.Controllers;

    [RequireComponent(typeof(Controllers.RoundController))]
    [RequireComponent(typeof(Controllers.PlayerController))]
    [RequireComponent(typeof(AIController))]
    public class GameManager : MonoBehaviour
    {
        [HideInInspector] public Controllers.RoundController roundController;
        [HideInInspector] public Controllers.PlayerController playerController;
        [HideInInspector] public AIController aiController;

        private Models.GameConfig config;

        private Models.Cards.Deck Deck;

        private int playerCredits;
        private int aiCredits;

        void Awake()
        {
            config = Models.GameConfig.Default();

            Deck = new Models.Cards.Deck(config.NumberCardCopiesPerRound, config.SpecialCardsPerRound);

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

        private void StartGame()
        {
            //ActorManager
            playerCredits = config.StartingCredits;
            aiCredits = config.StartingCredits;
            Events.GameEvents.InvokeScoreChanged(playerCredits, aiCredits);
            
            roundController.StartNewRound();
        }

        private void HandleRoundEnded(Models.Round.RoundResult result)
        {
            //ActorManager
            playerCredits += result.PlayerScoreChange;
            aiCredits += result.AIScoreChange;
            Events.GameEvents.InvokeScoreChanged(playerCredits, aiCredits);

            if (playerCredits <= 0)
            {
                UIManager.ShowWinner("AI");
            }
            else if (aiCredits <= 0)
            {
                UIManager.ShowWinner("Player");
            }

        }
    }
