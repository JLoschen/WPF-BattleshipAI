using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Battleship.Core;
using GalaSoft.MvvmLight;

namespace Battleship.Main
{
    public class Captain:ViewModelBase
    {
        public Captain()
        {
            ShowShipPlacement.CollectionChanged += (o, e) => UpdateGui();
        }

        public bool HasPlayed(string opponent)
        {
            return CaptainStatistics.ContainsKey(opponent);
        }

        public string CaptainName { get; set; }
        public string AssemblyQualifiedName { get; set; }
        public bool IsSelected { get; set; }
        public long TotalAttacks { get; set; }
        //public int[,] AllAttacks { get; set; }
        public int[] AllAttacks { get; set; }
        public float[] AttackHeat { get; set; }//Attack percent on each tile
        public float[] PlacementHeat { get; set; }//Placement percent on each tile
        public int[] AllPlacements { get; set; }
        //public int[] PatrolPlacements { get; set; }
        //public int[] DestroyerPlacements { get; set; }
        //public int[] SubmarinePlacements { get; set; }
        //public int[] BattleshipPlacements { get; set; }
        //public int[] AircraftCarrierPlacements { get; set; }
        public int [,,] ShipPlacements { get; set; } //3 dimensional array ship,x,y

        public /*bool[]*/ ObservableCollection<bool> ShowShipPlacement { get; set; } = new ObservableCollection<bool>
        {
            true,
            true,
            true,
            true,
            true
        };
        public IDictionary<string, CaptainStatistics> CaptainStatistics { get; set; }
        //public ObservableDictionary
        public int Score
        {
            get { return _score; }
            set { Set(ref _score, value); }
        }
        private int _score;

        public CaptainStatistics SelectedOpponentStatistics 
        { 
            get{ return _selectedOpponentStatistics; } 
            set{ Set(ref _selectedOpponentStatistics, value); } 
        } 
        private CaptainStatistics _selectedOpponentStatistics;

        public void UpdateGui()
        {
            for (int i = 0; i < 100; i++)
            {
                AllPlacements[i] = 0;
            }
            for (int ship = 0; ship < 5; ship++)
            {
                if (!ShowShipPlacement[ship]) continue;
                for (var x = 0; x < 10; x++)
                {
                    for (var y = 0; y < 10; y++)
                    {
                        AllPlacements[x*10 + y] += ShipPlacements[ship, x, y];
                    }
                }
            }

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
