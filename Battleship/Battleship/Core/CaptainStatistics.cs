namespace Battleship.Core
{
    public class CaptainStatistics
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Hits { get; set; }
        public int Misses { get; set; }
        public int WinAttacks { get; set; }
        public int LossAttacks { get; set; }
        public float Accuracy => (float)Hits /(Hits + Misses);
        public float AverageAttacksForWin => (float) WinAttacks/Wins;
        public float AverageAttacksForLoss => (float) LossAttacks/Losses;
    }
}
