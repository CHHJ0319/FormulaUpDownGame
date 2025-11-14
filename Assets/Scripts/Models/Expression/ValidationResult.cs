using System.Collections.Generic;

namespace Models.Expression
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Warnings { get; set; }

        private const string GeneralFailureMessage = "수식을 완성하지 못했습니다.";

        public ValidationResult()
        {
            IsValid = true;
            ErrorMessage = "";
            Warnings = new List<string>();
        }

        public void MarkInvalid(string message)
        {
            IsValid = false;
            ErrorMessage = GeneralFailureMessage;

            if (!string.IsNullOrEmpty(message))
            {
                Warnings.Add(message);
            }
        }
    }
}