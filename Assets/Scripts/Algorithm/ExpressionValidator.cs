using System.Collections.Generic;
using UnityEngine;

namespace Algorithm
{
    public static class ExpressionValidator
    {

        public static Models.Expression.ValidationResult Validate(Models.Expression.Expression expression, Actors.Hand hand)
        {
            Models.Expression.ValidationResult result = new Models.Expression.ValidationResult();

            if (!IsNotEmpty(expression, result))
                return result;

            if (!IsValidSequence(expression, result))
                return result;

            if (!AreAllNumberCardsUsed(expression, hand, result))
                return result;

            if (!AreAllSpecialCardsUsed(expression, hand, result))
                return result;

            return result;
        }

        private static bool IsNotEmpty(in Models.Expression.Expression expression, Models.Expression.ValidationResult result)
        {
            if (expression.IsEmpty())
            {
                result.MarkInvalid("수식이 비어있습니다.");
                return false;
            }
            else
            {
                return true;

            }
        }

        private static bool IsValidSequence(in Models.Expression.Expression expression, Models.Expression.ValidationResult result)
        {
            if (!expression.IsValidSequence())
            {
                result.MarkInvalid("수식이 완성되지 않았습니다.");
                return false;
            }
            return true;
        }

        private static bool AreAllNumberCardsUsed(Models.Expression.Expression expression, Actors.Hand hand, Models.Expression.ValidationResult result)
        {
            // 손패의 숫자별 개수 세기
            Dictionary<int, int> availableNumbers = new Dictionary<int, int>();
            foreach (var card in hand.NumberCards)
            {
                if (!availableNumbers.ContainsKey(card.Value))
                    availableNumbers[card.Value] = 0;
                availableNumbers[card.Value]++;
            }

            // 수식에서 사용한 숫자별 개수 세기
            Dictionary<int, int> usedNumbers = new Dictionary<int, int>();
            foreach (var number in expression.Numbers)
            {
                int intNumber = Mathf.RoundToInt(number);
                if (!usedNumbers.ContainsKey(intNumber))
                    usedNumbers[intNumber] = 0;
                usedNumbers[intNumber]++;
            }

            // 비교
            foreach (var kvp in availableNumbers)
            {
                int number = kvp.Key;
                int available = kvp.Value;
                int used = usedNumbers.ContainsKey(number) ? usedNumbers[number] : 0;

                if (used < available)
                {
                    result.MarkInvalid($"숫자 {number}을(를) {available - used}장 더 사용해야 합니다.");
                    return false;
                }
                else if (used > available)
                {
                    result.MarkInvalid($"숫자 {number}을(를) {used - available}장 초과 사용했습니다.");
                    return false;
                }
            }

            return true;
        }

        private static bool AreAllSpecialCardsUsed(Models.Expression.Expression expression, Actors.Hand hand, Models.Expression.ValidationResult result)
        {
            if (!AreAllSquareRootUsed(expression, hand, result))
                return false;

            if (!AreAllMultiplyUsed(expression, hand, result))
                return false;

            return true;
        }

        private static bool AreAllSquareRootUsed(Models.Expression.Expression expression, Actors.Hand hand, Models.Expression.ValidationResult result)
        {
            int requiredCount = hand.GetSquareRootCount();
            int usedCount = 0;

            foreach (bool hasRoot in expression.HasSquareRoot)
            {
                if (hasRoot) usedCount++;
            }

            if (usedCount < requiredCount)
            {
                result.MarkInvalid($"√를 {requiredCount - usedCount}개 더 사용해야 합니다.");
                return false;
            }
            else if (usedCount > requiredCount)
            {
                result.MarkInvalid($"√를 {usedCount - requiredCount}개 초과 사용했습니다.");
                return false;
            }

            return true;
        }

        private static bool AreAllMultiplyUsed(Models.Expression.Expression expression, Actors.Hand hand, Models.Expression.ValidationResult result)
        {
            int requiredCount = hand.GetMultiplyCount();
            int usedCount = 0;

            foreach (var op in expression.Operators)
            {
                if (op.Type == Operator.OperatorType.Multiply)
                    usedCount++;
            }

            if (usedCount < requiredCount)
            {
                result.MarkInvalid($"×를 {requiredCount - usedCount}개 더 사용해야 합니다.");
                return false;
            }
            else if (usedCount > requiredCount)
            {
                result.MarkInvalid($"×를 {usedCount - requiredCount}개 초과 사용했습니다.");
                return false;
            }

            return true;
        }
    }
}

