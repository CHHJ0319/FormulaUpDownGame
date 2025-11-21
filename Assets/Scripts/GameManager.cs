using UnityEngine;
using MathHighLow.Controllers;

    [RequireComponent(typeof(Controllers.RoundController))]
    public class GameManager : MonoBehaviour
    {
        [HideInInspector] public Controllers.RoundController roundController;
        

        private Models.GameConfig config;

        private Models.Cards.Deck Deck;

        private int playerCredits;
        private int aiCredits;

        void Awake()
        {
            config = Models.GameConfig.Default();

            Deck = new Models.Cards.Deck(config.NumberCardCopiesPerRound, config.SpecialCardsPerRound);

            ActorManager.Initialize(config.DealInterval);

            roundController = GetComponent<Controllers.RoundController>();
            roundController.Initialize(config, Deck);
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
