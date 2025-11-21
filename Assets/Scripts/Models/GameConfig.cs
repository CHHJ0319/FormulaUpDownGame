using UnityEngine;

namespace Models
{
    public class GameConfig
    {
        public float DealInterval = 0.2f;         
        public float SubmissionUnlockTime = 5f;  
        public float RoundDuration = 120f;          
        public float ResultsDisplayDuration = 5f;  

        public int InitialCardCount = 3;           

        public int NumberCardCopiesPerRound = 4;
        public int SpecialCardsPerRound = 10;
        public int MaxNumberCardsPerRound = 3;

        public int StartingCredits = 20;            
        public int MinBet = 1;                      
        public int MaxBet = 5;                     

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
            };
        }
    } 
}
