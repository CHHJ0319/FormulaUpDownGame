namespace Models
{
    public class GameConfig
    {
        public float DealInterval = 0.2f;         
        public float SubmissionUnlockTime = 5f;  
        public float RoundDuration = 120f;          
        public float ResultsDisplayDuration = 5f;          

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
    } 
}
