using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controllers
{
    public class AIController : MonoBehaviour
    {
        public Models.Hand Hand { get; private set; }
        public int Credits { get; set; }

        private Models.Expression.Expression bestExpression;

        private void Awake()
        {
            Initialize();

            ActorManager.SetAi(this);
        }

        public void HandleRoundStarted()
        {
            ResetHand();
        }

        private void Initialize()
        {
            Hand = new Models.Hand();
        }

        private void ResetHand()
        {
            Hand.Clear();
        }

        public void AddCard(Models.Cards.Card card)
        {
            if (Hand == null)
            {
                return;
            }

            Hand.AddCard(card);
        }

        public void PlayTurn(int target)
        {
            bestExpression = Algorithm.FormulaApproximator.FindBestExpression(target, Hand);

            var validation = Algorithm.ExpressionValidator.Validate(bestExpression, Hand);
            if (!validation.IsValid)
            {
                bestExpression = BuildFallbackExpression(Hand);

                var fallbackValidation = Algorithm.ExpressionValidator.Validate(bestExpression, Hand);
                if (!fallbackValidation.IsValid)
                {
                    Debug.LogWarning($"[AI] 대체 수식도 규칙을 만족하지 못했습니다: {fallbackValidation.ErrorMessage}");
                }
            }
        }

        public Models.Expression.Expression GetExpression()
        {
            return bestExpression ?? new Models.Expression.Expression();
        }

        private Models.Expression.Expression BuildFallbackExpression(Models.Hand hand)
        {
            Models.Expression.Expression fallback = new Models.Expression.Expression();

            var numbers = hand.NumberCards.Select(card => card.Value).ToList();
            if (numbers.Count == 0)
            {
                return fallback;
            }

            int sqrtRemaining = hand.GetSquareRootCount();
            int multiplyRemaining = Mathf.Min(hand.GetMultiplyCount(), Mathf.Max(0, numbers.Count - 1));

            Queue<Algorithm.Operator> operatorQueue = new Queue<Algorithm.Operator>(
                hand.OperatorCards.Select(card => card.Operator));

            for (int i = 0; i < numbers.Count; i++)
            {
                bool applySquareRoot = false;
                if (sqrtRemaining > 0)
                {
                    applySquareRoot = true;
                    sqrtRemaining--;
                }

                fallback.AddNumber(numbers[i], applySquareRoot);

                if (i < numbers.Count - 1)
                {
                    Algorithm.Operator opToAdd;

                    if (multiplyRemaining > 0)
                    {
                        opToAdd = new Algorithm.Operator(Algorithm.Operator.OperatorType.Multiply);
                        multiplyRemaining--;
                    }
                    else if (operatorQueue.Count > 0)
                    {
                        opToAdd = operatorQueue.Dequeue();
                    }
                    else
                    {
                        opToAdd = new Algorithm.Operator(Algorithm.Operator.OperatorType.Add);
                    }

                    fallback.AddOperator(opToAdd);
                }
            }

            return fallback;
        }
    }
}
