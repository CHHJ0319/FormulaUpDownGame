using UnityEngine;

namespace Models
{
    public class GameConfig
    {
        public float DealInterval = 0.2f;           // 카드 분배 간격
        public float SubmissionUnlockTime = 5f;    // 제출 잠금 시간
        public float RoundDuration = 180f;          // 라운드 제한 시간
        public float ResultsDisplayDuration = 10f;  // 결과 표시 시간

        public int InitialCardCount = 3;           

        public int NumberCardCopiesPerRound = 4;
        public int MultiplySpecialCardsPerRound = 5;       
        public int SquareRootSpecialCardsPerRound = 5;     

        public int StartingCredits = 20;            // 초기 자금
        public int MinBet = 1;                      // 최소 베팅
        public int MaxBet = 5;                      // 최대 베팅
        public int[] TargetValues = { 1, 20 };      // 목표값 옵션

        public static GameConfig Default()
        {
            return new GameConfig();
        }

        public static GameConfig SetEasyMode()
        {
            return new GameConfig
            {
                RoundDuration = 300f,
                SubmissionUnlockTime = 20f,
                StartingCredits = 30,
                TargetValues = new[] { 5, 10 }
            };
        }

        public static GameConfig SetHardMode()
        {
            return new GameConfig
            {
                RoundDuration = 120f,
                SubmissionUnlockTime = 40f,
                StartingCredits = 15,
                MinBet = 2,
                MaxBet = 10,
                TargetValues = new[] { 1, 50, 100 }
            };
        }
    } 
}
