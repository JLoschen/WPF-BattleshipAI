using System.Collections.Generic;
using System.Linq;
using Battleship.Core;
using GalaSoft.MvvmLight;

namespace Battleship
{
    public class Captain:ViewModelBase
    {
        public string CaptainName { get; set; }
        public string AssemblyQualifiedName { get; set; }
        public bool IsSelected { get; set; }
        public long TotalAttacks { get; set; }
        //public int[,] AllAttacks { get; set; }
        public int[] AllAttacks { get; set; }
        public float[] AttackHeat { get; set; }//Attack percent on each tile
        public float[] PlacementHeat { get; set; }//Placement percent on each tile
        public int[] AllPlacements { get; set; }
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

        public void UpdateGui()
        {
            var expectedAttacksPerCell = AllAttacks.Sum(x => x) / 100f;
            var expectedPlacementsPerCell = AllPlacements.Sum(x => x) / 100f;

            for (int i = 0; i < 100; i++)
            {
                AttackHeat[i] = AllAttacks[i] / expectedAttacksPerCell;
                PlacementHeat[i] = AllPlacements[i] / expectedPlacementsPerCell;
            }

            RaisePropertyChanged("AllAttacks");
            RaisePropertyChanged("AttackHeat");
            RaisePropertyChanged("PlacementHeat");
        }
    }
}
