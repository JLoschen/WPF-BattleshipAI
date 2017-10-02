using System.Collections.Generic;
using Battleship.Core;
using GalaSoft.MvvmLight;

namespace Battleship
{
    public class Captain:ViewModelBase
    {
        public string CaptainName { get; set; }
        public string AssemblyQualifiedName { get; set; }
        public bool IsSelected { get; set; }
        public int[,] AllAttacks { get; set; }
        public int[,] AllPlacements { get; set; }
        public int[,] PatrolPlacements { get; set; }
        public int[,] DestroyerPlacements { get; set; }
        public int[,] SubmarinePlacements { get; set; }
        public int[,] BattleshipPlacements { get; set; }
        public int[,] AircraftCarrierPlacements { get; set; }
        public IDictionary<string, CaptainStatistics> CaptainStatistics { get; set; }
        public int Score
        {
            get { return _score; }
            set { Set(ref _score, value); }
        }
        private int _score;
    }
}
