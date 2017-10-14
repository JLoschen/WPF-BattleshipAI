using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Battleship.Core;
using Battleship.TestingWindow.GridExtensions;

namespace Battleship.TestingWindow
{
    public partial class CaptainDebugWindow
    {
        public CaptainDebugWindow()
        {
            InitializeComponent();
        }
        
        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            //var border = sender as Border;
            var cell = sender as Cell;
            var vm = DataContext as CaptainDebugViewModel;
            if (cell == null || vm == null) return;
            var row = cell.LocationId / 10; 
            var col = cell.LocationId % 10 +1;

            switch (vm.GameState)
            {
                case GameState.HumanPlayerPlacingPatrol:
                    SetShipsPosition(PatrolHorizontal, PatrolVertical, col, row, 10);
                    SetShipsPosition(DestroyerHorizontal, DestroyerVertical, col, row, 9);
                    SetShipsPosition(SubmarineHorizontal, SubmarineVertical, col, row, 9);
                    SetShipsPosition(BattleshipHorizontal, BattleshipVertical, col, row, 8);
                    SetShipsPosition(AircraftCarrierHorizontal, AircraftCarrierVertical, col, row, 7);
                    break;
                case GameState.HumanPlayerPlacingDestroyer:
                    SetShipsPosition(DestroyerHorizontal, DestroyerVertical, col, row, 9);
                    SetShipsPosition(SubmarineHorizontal, SubmarineVertical, col, row, 9);
                    SetShipsPosition(BattleshipHorizontal, BattleshipVertical, col, row, 8);
                    SetShipsPosition(AircraftCarrierHorizontal, AircraftCarrierVertical, col, row, 7);
                    break;
                case GameState.HumanPlayerPlacingSubmarine:
                    SetShipsPosition(SubmarineHorizontal, SubmarineVertical, col, row, 9);
                    SetShipsPosition(BattleshipHorizontal, BattleshipVertical, col, row, 8);
                    SetShipsPosition(AircraftCarrierHorizontal, AircraftCarrierVertical, col, row, 7);
                    break;
                case GameState.HumanPlayerPlacingBattleship:
                    SetShipsPosition(BattleshipHorizontal, BattleshipVertical, col, row, 8);
                    SetShipsPosition(AircraftCarrierHorizontal, AircraftCarrierVertical, col, row, 7);
                    break;
                case GameState.HumanPlayerPlacingAircraftCarrier:
                    SetShipsPosition(AircraftCarrierHorizontal, AircraftCarrierVertical, col, row, 7);
                    break;
            }
        }

        private void SetShipsPosition(FrameworkElement shipHorizontal, FrameworkElement shipVertical,  int col, int row, int max)
        {
            if (col < max)
                shipHorizontal.SetGridPosition(col, row);
            if (row < max - 1)// extra grid row so minus 1
                shipVertical.SetGridPosition(col, row);
        }

        //TODO create place ship on board method
        private void Ready_OnClick(object sender, RoutedEventArgs e)
        {
            //HidePreviousShips();
            var vm = DataContext as CaptainDebugViewModel;
            var aiFleet = vm.CreateFleet().GetFleet();

            var ptBoat = aiFleet[Constants.PatrolBoat];
            var destroyer = aiFleet[Constants.Destroyer];
            var submarine = aiFleet[Constants.Submarine];
            var battleship = aiFleet[Constants.Battleship];
            var airCraftCarrier = aiFleet[Constants.AircraftCarrier];

            if (ptBoat.IsVertical)
            {
                AIPatrolVertical.SetGridPosition(ptBoat.Location);
            }
            else
            {
                AIPatrolHorizontal.SetGridPosition(ptBoat.Location);
            }

            if (destroyer.IsVertical)
            {
                AIDestroyerVertical.SetGridPosition(destroyer.Location);
            }
            else
            {
                AIDestroyerHorizontal.SetGridPosition(destroyer.Location);
            }

            if (submarine.IsVertical)
            {
                AISubmarineVertical.SetGridPosition(submarine.Location);
            }
            else
            {
                AISubmarineHorizontal.SetGridPosition(submarine.Location);
            }

            if (battleship.IsVertical)
            {
                AIBattleshipVertical.SetGridPosition(battleship.Location);
            }
            else
            {
                AIBattleshipHorizontal.SetGridPosition(battleship.Location);
            }

            if (airCraftCarrier.IsVertical)
            {
                AIAircraftCarrierVertical.SetGridPosition(airCraftCarrier.Location);
            }
            else
            {
                AIAircraftCarrierHorizontal.SetGridPosition(airCraftCarrier.Location);
            }

            if (ShowShips.IsChecked.Value)
            {

            }
        }

        private void HidePreviousShips()
        {
            AIPatrolHorizontal.Visibility = Visibility.Hidden;
            AIPatrolVertical.Visibility = Visibility.Hidden;
            AIDestroyerHorizontal.Visibility = Visibility.Hidden;
            AIDestroyerVertical.Visibility = Visibility.Hidden;
            AISubmarineHorizontal.Visibility = Visibility.Hidden;
            AISubmarineVertical.Visibility = Visibility.Hidden;
            AIBattleshipHorizontal.Visibility = Visibility.Hidden;
            AIBattleshipVertical.Visibility = Visibility.Hidden;
            AIAircraftCarrierHorizontal.Visibility = Visibility.Hidden;
            AIAircraftCarrierVertical.Visibility = Visibility.Hidden;

        }
    }

    namespace GridExtensions
    {
        public static class MyExtensions
        {
            public static void SetGridPosition(this FrameworkElement element, int col, int row)
            {
                Grid.SetColumn(element, col);
                Grid.SetRow(element, row);
            }
            public static void SetGridPosition(this FrameworkElement element, Coordinate coord)
            {
                Grid.SetColumn(element, coord.X +1 );
                Grid.SetRow(element, coord.Y);
            }
        }
    }
}
