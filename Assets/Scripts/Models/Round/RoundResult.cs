using UnityEngine;

namespace Models.Round
{
    public class RoundResult
    {
        public int TargetScore { get; set; }
        public int BetAmount { get; set; }

        public string PlayerExpression { get; set; }
        public float PlayerValue { get; set; }
        public float PlayerDifference { get; set; }
        public string PlayerError { get; set; }


        public string AIExpression { get; set; }
        public float AIValue { get; set; }
        public float AIDifference { get; set; }
        public string AIError { get; set; }

        public string Winner { get; set; }

        public int PlayerScoreChange { get; set; }
        public int AIScoreChange { get; set; }

        public RoundResult()
        {
            PlayerExpression = "";
            AIExpression = "";
            PlayerError = "";
            AIError = "";
            Winner = "Invalid";
        }

        public string GetSummary()
        {
            return Winner switch
            {
                "Player" => $"플레이어 승리! (+${BetAmount})",
                "AI" => $"AI 승리! (-${BetAmount})",
                "Draw" => "무승부",
                _ => "라운드 무효"
            };
        }

        public string GetDetail()
        {
            string detail = $"목표 점수: {TargetScore} | 베팅 금액: ${BetAmount}\n";

            if (!string.IsNullOrEmpty(PlayerError))
            {
                detail += $"플레이어: {PlayerError}\n";
            }
            else
            {
                detail += $"플레이어: {PlayerExpression} = {PlayerValue:F2} (차이: {PlayerDifference:F2})\n";
            }

            if (!string.IsNullOrEmpty(AIError))
            {
                detail += $"AI: {AIError}";
            }
            else
            {
                detail += $"AI: {AIExpression} = {AIValue:F2} (차이: {AIDifference:F2})";
            }

            return detail;
        }
    }
}
