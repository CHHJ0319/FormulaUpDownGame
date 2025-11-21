using UnityEngine;

namespace Algorithm
{
    public class Operator
    {
        public enum OperatorType
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            SquareRoot
        }

        public static Operator[] BasicOperators = new[] {
            new Operator(OperatorType.Add),
            new Operator(OperatorType.Subtract),
            new Operator(OperatorType.Divide)
        };


        public OperatorType Type { get; }

        public Operator(OperatorType type)
        {
            Type = type;
        }

        public int GetPriority()
        {
            switch (Type)
            {
                case Operator.OperatorType.Multiply:
                case Operator.OperatorType.Divide:
                    return 2;

                case Operator.OperatorType.Add:
                case Operator.OperatorType.Subtract:
                    return 1;

                default:
                    return 0;
            }
        }

        public float Calculate(float left, float right, Models.Expression.EvaluationResult result)
        {
            switch (Type)
            {
                case OperatorType.Add:
                    return left + right;

                case OperatorType.Subtract:
                    return left - right;

                case OperatorType.Multiply:
                    return left * right;

                case OperatorType.Divide:
                    if (Mathf.Approximately(right, 0))
                    {
                        result.Success = false;
                        result.ErrorMessage = "0으로 나눌 수 없습니다.";
                        return 0;
                    }
                    return left / right;

                default:
                    result.Success = false;
                    result.ErrorMessage = "알 수 없는 연산자입니다.";
                    return 0;
            }
        }
    }
}

