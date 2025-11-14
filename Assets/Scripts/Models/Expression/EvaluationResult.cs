namespace Models.Expression
{
    public class EvaluationResult
    {
        public bool Success { get; set; }
        public float Value { get; set; }
        public string ErrorMessage { get; set; }

        public EvaluationResult()
        {
            Success = true;
            Value = 0;
            ErrorMessage = "";
        }
    }
}