using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Models
{
    public class Expression
    {
        public List<float> Numbers { get; private set; }
        public List<Cards.OperatorType> Operators { get; private set; }
        public List<bool> HasSquareRoot { get; private set; }

        public Expression()
        {
            Numbers = new List<float>();
            Operators = new List<Cards.OperatorType>();
            HasSquareRoot = new List<bool>();
        }

        public void AddNumber(float number, bool hasSquareRoot = false)
        {
            Numbers.Add(number);
            HasSquareRoot.Add(hasSquareRoot);
        }

        public void AddOperator(Cards.OperatorType op)
        {
            Operators.Add(op);
        }

        public void RemoveLast()
        {
            if (Operators.Count > 0 && Operators.Count == Numbers.Count - 1)
            {
                Operators.RemoveAt(Operators.Count - 1);
            }
            else if (Numbers.Count > 0)
            {
                Numbers.RemoveAt(Numbers.Count - 1);
                HasSquareRoot.RemoveAt(HasSquareRoot.Count - 1);
            }
        }

        public void Clear()
        {
            Numbers.Clear();
            Operators.Clear();
            HasSquareRoot.Clear();
        }


        public bool IsComplete()
        {
            return Numbers.Count > 0 && Operators.Count == Numbers.Count - 1;
        }

        public bool IsEmpty()
        {
            return Numbers.Count == 0;
        }


        public bool ExpectingNumber()
        {
            return Numbers.Count == Operators.Count;
        }

        public override string ToString()
        {
            if (Numbers.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Numbers.Count; i++)
            {
                if (HasSquareRoot[i])
                {
                    sb.Append("√");
                }
                sb.Append(Numbers[i].ToString("0.##"));

                if (i < Operators.Count)
                {
                    sb.Append(" ");
                    sb.Append(Converter.OperatorTypeWithString.ToSymbolString(Operators[i]));
                    sb.Append(" ");
                }
            }

            return sb.ToString();
        }

        public Expression Clone()
        {
            Expression clone = new Expression();
            clone.Numbers.AddRange(Numbers);
            clone.Operators.AddRange(Operators);
            clone.HasSquareRoot.AddRange(HasSquareRoot);
            return clone;
        }
    }
}
