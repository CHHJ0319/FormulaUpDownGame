using UnityEngine;

    [RequireComponent(typeof(Controllers.RoundController))]
    public class GameManager : MonoBehaviour
    {
        [HideInInspector] public Controllers.RoundController roundController;
        

        private Models.GameConfig config;

        private Models.Cards.Deck Deck;

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
            ActorManager.SetPlayerCredits(config.StartingCredits, config.StartingCredits);

            roundController.StartNewRound();

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
