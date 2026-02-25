using System.Collections;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    private Models.GameConfig config;
    private Models.Cards.Deck Deck;

    private int targetScore;
    private int currentBet;

    private enum RoundPhase
    {
        Idle,
        Standby,
        Dealing,
        Main,
        Evaluating,
        Results
    }
    private RoundPhase currentPhase;

    private bool playerSubmitted;
    private float roundTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        Events.GameEvents.OnSubmitClicked += HandleSubmitClicked;
        Events.GameEvents.OnBetChanged += HandleBetChanged;
    }

    void OnDisable()
    {
        Events.GameEvents.OnSubmitClicked -= HandleSubmitClicked;
        Events.GameEvents.OnBetChanged -= HandleBetChanged;
    }

    public void Initialize(Models.GameConfig gameConfig)
    {
        config = gameConfig;

        Deck = new Models.Cards.Deck(config.NumberCardCopiesPerRound, config.SpecialCardsPerRound);
    }

    public void StartNewRound()
    {
        if (currentPhase != RoundPhase.Idle)
        {
            return;
        }

        StartCoroutine(RoundLoopRoutine());
    }

    private void HandleSubmitClicked()
    {
        if (currentPhase != RoundPhase.Main)
        {
            return;
        }

        bool timeUnlocked = roundTimer >= config.SubmissionUnlockTime;
        bool isAllSpecialCardsUsed = ActorManager.Instance.IsAllSpecialCardsUsed();
        bool isAllNumberCardsUsed = ActorManager.Instance.IsAllNumberCardsUsed();

        bool isSubmitAvailable = timeUnlocked && isAllSpecialCardsUsed && isAllNumberCardsUsed;

        if (!isAllNumberCardsUsed)
        {
            Events.UIEvents.InvokeStatusTextUpdated("제출하려면 받은  숫자 카드를 모두 사용해야 합니다");
            return;
        }
        else if (!isAllSpecialCardsUsed)
        {
            Events.UIEvents.InvokeStatusTextUpdated("제출하려면 받은 √와 × 특수 카드를 모두 사용해야 합니다");
            return;
        }
        else if (!timeUnlocked && isSubmitAvailable)
        {
            Events.UIEvents.InvokeStatusTextUpdated("5초 후 제출 버튼이 활성화됩니다");
            return;
        }

        playerSubmitted = true;

    }

    private void HandleBetChanged(int bet)
    {
        currentBet = bet;
    }

    private IEnumerator RoundLoopRoutine()
    {
        currentPhase = RoundPhase.Standby;
        yield return StartCoroutine(StartStandbyPhase());

        currentPhase = RoundPhase.Dealing;
        yield return StartCoroutine(StartDealingPhase());

        currentPhase = RoundPhase.Main;
        yield return StartCoroutine(StartMainPhase());

        currentPhase = RoundPhase.Evaluating;
        Models.Round.RoundResult result = StartEvaluatePhase();

        currentPhase = RoundPhase.Results;
        yield return StartCoroutine(ResultsPhase(result));

        currentPhase = RoundPhase.Idle;
        StartNewRound();
    }

    private IEnumerator StartStandbyPhase()
    {
        ActorManager.Instance.InitializeRound();
        UIManager.Instance.InitializeRound();

        ChangeBet(config.MinBet);
        SetTargetScore();

        playerSubmitted = false;
        roundTimer = 0f;

        yield return null;
    }

    private void ChangeBet(int bet)
    {
        currentBet = bet;

        Events.GameEvents.InvokeBetChanged(currentBet);
    }

    private IEnumerator StartDealingPhase()
    {
        Deck.BuildDeck();

        Events.UIEvents.InvokeStatusTextUpdated("카드를 분배합니다...");

        yield return StartCoroutine(DealCardsToPlayer());

        yield return StartCoroutine(DealCardsToAI());
    }

    private IEnumerator StartMainPhase()
    {
        Events.UIEvents.InvokeStatusTextUpdated("2분 안에 수식을 완성시키고, 저와 플레이어님 중 누가 이길지 예측해 배팅해 보세요");

        while (roundTimer < config.RoundDuration)
        {
            if (playerSubmitted)
            {
                yield break;
            }

            roundTimer += Time.deltaTime;
            UIManager.Instance.UpdateTimer(roundTimer, config.RoundDuration);

            bool timeUnlocked = roundTimer >= config.SubmissionUnlockTime;
            bool isAllSpecialCardsUsed = ActorManager.Instance.IsAllSpecialCardsUsed();
            bool isAllNumberCardsUsed = ActorManager.Instance.IsAllNumberCardsUsed();

            bool isSubmitAvailable = timeUnlocked && isAllSpecialCardsUsed && isAllNumberCardsUsed;

                
            if(isSubmitAvailable)
            {
                Events.UIEvents.InvokeStatusTextUpdated("수식을 완성하면 제출 버튼을 눌러주세요");
            }

            UIManager.Instance.UpdateSubmitAvailability(isSubmitAvailable);

            yield return null;
        }
    }

    private void SetTargetScore()
    {
        targetScore = Random.Range(0, 21);

        UIManager.Instance.SetTargetScore(targetScore);
    }

    private IEnumerator DealCardsToPlayer()
    {
        foreach (var op in Algorithm.Operator.BasicOperators)
        {
            Models.Cards.OperatorCard operatorCard = new Models.Cards.OperatorCard(op);
            ActorManager.Instance.AddCardInPlayerHand(operatorCard);

            yield return new WaitForSeconds(config.DealInterval);
        }

        int numberCardsDrawn = 0;
        int specialCardsDrawn = 0;
        while (numberCardsDrawn < config.MaxNumberCardsPerRound)
        {
            Models.Cards.Card drawnCard = Deck.Draw();

            if (drawnCard is Models.Cards.NumberCard)
            {
                numberCardsDrawn++;
            }
            else if (drawnCard is Models.Cards.SpecialCard)
            {
                if(specialCardsDrawn >= config.MaxNumberCardsPerRound - 1)
                {
                    continue;
                }
                specialCardsDrawn++;
            }

            ActorManager.Instance.AddCardInPlayerHand(drawnCard);

            yield return new WaitForSeconds(config.DealInterval);
        }
    }

    private IEnumerator DealCardsToAI()
    {

        foreach (var op in Algorithm.Operator.BasicOperators)
        {
            Models.Cards.OperatorCard operatorCard = new Models.Cards.OperatorCard(op);
            ActorManager.Instance.AddCardInAIHand(operatorCard);

            yield return new WaitForSeconds(config.DealInterval);
        }

        int numberCardsDrawn = 0;
        int specialCardsDrawn = 0;
        while (numberCardsDrawn < config.MaxNumberCardsPerRound)
        {
            Models.Cards.Card drawnCard = Deck.Draw();

            if (drawnCard is Models.Cards.NumberCard)
            {
                numberCardsDrawn++;
            }
            else if (drawnCard is Models.Cards.SpecialCard)
            {
                if (specialCardsDrawn >= config.MaxNumberCardsPerRound)
                {
                    continue;
                }
                specialCardsDrawn++;
            }

            ActorManager.Instance.AddCardInAIHand(drawnCard);

            yield return new WaitForSeconds(config.DealInterval);
        }
    }

    private IEnumerator ResultsPhase(Models.Round.RoundResult result)
    {
        UIManager.Instance.ShowRoundResult(result);

        ActorManager.Instance.SetCredits(result.PlayerScoreChange, result.AIScoreChange);
        
        if (ActorManager.Instance.IsPlayerNegativeBalance())
        {
            UIManager.ShowWinner("AI");
        }
        else if (ActorManager.Instance.IsAINegativeBalance())
        {
            UIManager.ShowWinner("Player");
        }

        Events.UIEvents.InvokeStatusTextUpdated("수식 결과를 확인하세요");

        float resultsTimer = 0f;

        while (resultsTimer < config.ResultsDisplayDuration)
        {
            resultsTimer += Time.deltaTime;

            UIManager.Instance.UpdateTimer(resultsTimer, config.ResultsDisplayDuration);

            yield return null;
        }
    }

    private Models.Round.RoundResult StartEvaluatePhase()
    {
        var playerValidation = ActorManager.Instance.ValidatePlayerExpression();
        var playerEvaluation = playerValidation.IsValid
            ? ActorManager.Instance.EvaluatePlayerExpression()
            : new Models.Expression.EvaluationResult { Success = false, ErrorMessage = playerValidation.ErrorMessage };

        ActorManager.Instance.ExecuteAITurn(targetScore);
        var aiEvaluation = ActorManager.Instance.EvaluateAiExpression();

        return CreateRoundResult(playerEvaluation, aiEvaluation);
    }

    private Models.Round.RoundResult CreateRoundResult(Models.Expression.EvaluationResult playerEval, Models.Expression.EvaluationResult aiEval)
    {
        Models.Round.RoundResult result = new Models.Round.RoundResult
        {
            TargetScore = targetScore,
            BetAmount = currentBet,
            PlayerExpression = playerEval.Success ? ActorManager.Instance.GetPlayerExpression().ToString() : "-",
            PlayerValue = playerEval.Success ? playerEval.Value : float.NaN,
            PlayerError = playerEval.Success ? "" : playerEval.ErrorMessage,
            AIExpression = aiEval.Success ? ActorManager.Instance.GetAiExpression().ToString() : "-",
            AIValue = aiEval.Success ? aiEval.Value : float.NaN,
            AIError = aiEval.Success ? "" : aiEval.ErrorMessage
        };

        result.PlayerDifference = playerEval.Success ? Mathf.Abs(result.PlayerValue - result.TargetScore) : float.PositiveInfinity;
        result.AIDifference = aiEval.Success ? Mathf.Abs(result.AIValue - result.TargetScore) : float.PositiveInfinity;

        if (result.PlayerDifference == float.PositiveInfinity && result.AIDifference == float.PositiveInfinity)
        {
            result.Winner = "Invalid";
            result.PlayerScoreChange = 0;
        }
        else if (Mathf.Approximately(result.PlayerDifference, result.AIDifference))
        {
            result.Winner = "Draw";
            result.PlayerScoreChange = 0;
        }
        else if (result.PlayerDifference < result.AIDifference)
        {
            result.Winner = "Player";
            result.PlayerScoreChange = currentBet;
        }
        else
        {
            result.Winner = "AI";
            result.PlayerScoreChange = -currentBet;
        }

        result.AIScoreChange = -result.PlayerScoreChange;
        return result;
    }
}