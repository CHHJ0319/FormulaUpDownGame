using System.Collections.Generic;
using UnityEngine;

namespace Algorithm
{
    public static class ExpressionEvaluator
    {
        public static Models.Expression.EvaluationResult Evaluate(Models.Expression.Expression expression)
        {
            Models.Expression.EvaluationResult result = new Models.Expression.EvaluationResult();

            List<float> processedNumbers = ApplySquareRoots(expression);

            float finalValue = CalculateWithPriority(processedNumbers, expression.Operators, result);
            if (!result.Success)
                return result;

            result.Value = finalValue;
            return result;
        }

        private static List<float> ApplySquareRoots(in Models.Expression.Expression expression)
        {
            List<float> sqrtResults = new List<float>();

            for (int i = 0; i < expression.Numbers.Count; i++)
            {
                float number = expression.Numbers[i];

                if (expression.HasSquareRoot[i])
                {
                    number = Mathf.Sqrt(number);
                }

                sqrtResults.Add(number);
            }

            return sqrtResults;
        }

        private static float CalculateWithPriority(List<float> numbers, List<Operator> operators, Models.Expression.EvaluationResult result)
        {
            Stack<float> numberStack = new Stack<float>();
            Stack<Operator> operatorStack = new Stack<Operator>();

            numberStack.Push(numbers[0]);
            for (int i = 0; i < operators.Count; i++)
            {
                Operator currentOpr = operators[i];
                float nextNumber = numbers[i + 1];

                while (operatorStack.Count > 0 && operatorStack.Peek().GetPriority() >= currentOpr.GetPriority())
                {
                    if (numberStack.Count >= 2)
                    {
                        if (!TryExecuteOperation(numberStack, operatorStack, result))
                            return 0;
                    }
                }

                operatorStack.Push(currentOpr);
                numberStack.Push(nextNumber);
            }

            while (operatorStack.Count > 0)
            {
                if (!TryExecuteOperation(numberStack, operatorStack, result))
                    return 0;
            }

            return numberStack.Pop();
        }

        private static bool TryExecuteOperation(Stack<float> numberStack, Stack<Operator> operatorStack, Models.Expression.EvaluationResult result)
        {
            float right = numberStack.Pop();
            float left = numberStack.Pop();
            Algorithm.Operator opr = operatorStack.Pop();

            float res = opr.Calculate(left, right, result);
            if (!result.Success)
                return false;

            numberStack.Push(res);
            return true;
        }
    }
}
