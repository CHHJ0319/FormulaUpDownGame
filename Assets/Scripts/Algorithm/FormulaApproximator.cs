using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Algorithm
{
    public static class FormulaApproximator
    {
        private static Models.Hand currentHand;
        private static int targetScore;

        private static Models.Expression.Expression bestExpression;
        private static float bestDistance;
        private static Models.Expression.Expression prioritizedExpression;
        private static float prioritizedDistance;

        private static List<Algorithm.Operator> availableOperators;
        private static int requiredSquareRootCount;
        private static int requiredMultiplyCount;
        private static bool shouldPrioritizeSpecialUsage;

        public static  Models.Expression.Expression FindBestExpression(int target, Models.Hand hand)
        {
            targetScore = target;
            currentHand = hand;

            bestExpression = new Models.Expression.Expression();

            bestDistance = float.PositiveInfinity;
            prioritizedExpression = null;
            prioritizedDistance = float.PositiveInfinity;

            requiredMultiplyCount = currentHand.GetMultiplyCount();
            requiredSquareRootCount = currentHand.GetSquareRootCount();
            shouldPrioritizeSpecialUsage = (requiredSquareRootCount > 0 || requiredMultiplyCount > 0);

            availableOperators = currentHand.OperatorCards
                .Where(op => currentHand.IsOperatorEnabled(op.Operator.Type))
                .Select(op => op.Operator)
                .ToList();

            if (currentHand.NumberCards.Count == 0)
                return new Models.Expression.Expression();

            int totalSlots = Mathf.Max(0, currentHand.NumberCards.Count - 1);
            if (requiredMultiplyCount > totalSlots)
                return new Models.Expression.Expression();

            int baseOperatorSlots = totalSlots - requiredMultiplyCount;
            if (baseOperatorSlots > availableOperators.Count)
                return new Models.Expression.Expression();

            var numbers = currentHand.NumberCards.Select(c => c.Value).ToList();
            PermuteNumbers(numbers, new List<int>(), new Dictionary<int, int>());

            if (shouldPrioritizeSpecialUsage && prioritizedExpression != null)
            {
                return prioritizedExpression.Clone();
            }

            return bestExpression.Clone();
        }

        private static void PermuteNumbers(List<int> remaining, List<int> usedNums, Dictionary<int, int> used)
        {
            if (usedNums.Count == currentHand.NumberCards.Count)
            {
                DistributeSquareRoots(usedNums, 0, new List<int>());
                return;
            }

            var numberCounts = new Dictionary<int, int>();
            foreach (var num in remaining)
            {
                if (!numberCounts.ContainsKey(num))
                    numberCounts[num] = 0;
                numberCounts[num]++;
            }

            foreach (var kvp in numberCounts)
            {
                int number = kvp.Key;

                int usedCount = used.ContainsKey(number) ? used[number] : 0;
                if (usedCount >= kvp.Value)
                    continue;

                usedNums.Add(number);
                used[number] = usedCount + 1;

                PermuteNumbers(remaining, usedNums, used);

                usedNums.RemoveAt(usedNums.Count - 1);
                used[number] = usedCount;
            }
        }

        private static void DistributeSquareRoots(List<int> usedNums, int index, List<int> sqrtCounts)
        {
            if (index == usedNums.Count)
            {
                int totalSqrt = sqrtCounts.Sum();
                if (totalSqrt == requiredSquareRootCount)
                {
                    EnumerateOperators(usedNums, sqrtCounts);
                }
                return;
            }

            int remaining = requiredSquareRootCount - sqrtCounts.Sum();
            int numbersLeft = usedNums.Count - index;

            int maxCountForThisNumber = Mathf.Min(1, remaining);
            int minCountForThisNumber = Mathf.Max(0, remaining - (numbersLeft - 1));

            if (minCountForThisNumber > maxCountForThisNumber)
                return;

            for (int count = maxCountForThisNumber; count >= minCountForThisNumber; count--)
            {
                sqrtCounts.Add(count);
                DistributeSquareRoots(usedNums, index + 1, sqrtCounts);
                sqrtCounts.RemoveAt(sqrtCounts.Count - 1);
            }
        }

        private static void EnumerateOperators(List<int> numbers, List<int> sqrtCounts)
        {
            var operatorPool = new List<Algorithm.Operator>(availableOperators);
            AssignOperators(numbers, sqrtCounts, new List<Algorithm.Operator>(),
                           operatorPool, 0, 0);
        }

        private static void AssignOperators(List<int> numbers, List<int> sqrtCounts,
            List<Algorithm.Operator> operators,
            List<Algorithm.Operator> remainingOperators,
            int index, int multiplyUsed)
        {
            int totalSlots = numbers.Count - 1;
            int multiplyNeeded = currentHand.GetMultiplyCount();
            int slotsRemaining = totalSlots - index;
            int multiplyRemaining = multiplyNeeded - multiplyUsed;

            if (multiplyRemaining > slotsRemaining)
                return;

            if (index == totalSlots)
            {
                if (multiplyUsed == multiplyNeeded)
                {
                    BuildAndEvaluate(numbers, sqrtCounts, operators);
                }
                return;
            }

            if (multiplyUsed < multiplyNeeded)
            {
                operators.Add(new Algorithm.Operator(Algorithm.Operator.OperatorType.Multiply));
                AssignOperators(numbers, sqrtCounts, operators, remainingOperators,
                               index + 1, multiplyUsed + 1);
                operators.RemoveAt(operators.Count - 1);
            }

            for (int i = 0; i < remainingOperators.Count; i++)
            {
                var op = remainingOperators[i];

                operators.Add(op);

                var newRemaining = new List<Algorithm.Operator>(remainingOperators);
                newRemaining.RemoveAt(i);

                AssignOperators(numbers, sqrtCounts, operators, newRemaining,
                               index + 1, multiplyUsed);

                operators.RemoveAt(operators.Count - 1);
            }
        }

        private static void BuildAndEvaluate(List<int> numbers, List<int> sqrtCounts,
            List<Algorithm.Operator> operators)
        {
            Models.Expression.Expression expr = new Models.Expression.Expression();

            for (int i = 0; i < numbers.Count; i++)
            {
                bool hasRoot = sqrtCounts[i] > 0;
                expr.AddNumber(numbers[i], hasRoot);

                if (i < operators.Count)
                {
                    expr.AddOperator(operators[i]);
                }
            }

            var validation = Algorithm.ExpressionValidator.Validate(expr, currentHand);
            if (!validation.IsValid)
                return;

            var evaluation = Algorithm.ExpressionEvaluator.Evaluate(expr);
            if (!evaluation.Success)
                return;

            float distance = Mathf.Abs(evaluation.Value - targetScore);

            if (shouldPrioritizeSpecialUsage && UsesAllRequiredSpecialCards(expr))
            {
                if (distance < prioritizedDistance)
                {
                    prioritizedDistance = distance;
                    prioritizedExpression = expr.Clone();
                }
            }

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestExpression = expr.Clone();
            }
        }

        private static bool UsesAllRequiredSpecialCards(Models.Expression.Expression expr)
        {
            if (expr == null)
                return false;

            int usedRoots = expr.HasSquareRoot.Count(hasRoot => hasRoot);
            int usedMultiply = expr.Operators.Count(op => op.Type == Algorithm.Operator.OperatorType.Multiply);

            return usedRoots == requiredSquareRootCount && usedMultiply == requiredMultiplyCount;
        }
    }
}

