using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
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
                    if (col < 10)
                        PatrolHorizontal.SetGridPosition(col, row);
                    if(row < 9)
                        PatrolVertical.SetGridPosition(col, row);
                    break;
                case GameState.HumanPlayerPlacingDestroyer:
                    if (col < 9)
                        DestroyerHorizontal.SetGridPosition(col, row);
                    if (row < 8)
                        DestroyerVertical.SetGridPosition(col, row);
                    break;
                case GameState.HumanPlayerPlacingSubmarine:
                    if (col < 9)
                        SubmarineHorizontal.SetGridPosition(col, row);
                    if (row < 8)
                        SubmarineVertical.SetGridPosition(col, row);
                    break;
                case GameState.HumanPlayerPlacingBattleship:
                    if (col < 8)
                        BattleshipHorizontal.SetGridPosition(col, row);
                    if (row < 7)
                        BattleshipVertical.SetGridPosition(col, row);
                    break;
                case GameState.HumanPlayerPlacingAircraftCarrier:
                    if ( col < 7)
                        AircraftCarrierHorizontal.SetGridPosition(col, row);
                    if (row < 6)
                        AircraftCarrierVertical.SetGridPosition(col, row);
                    break;
            }
        }
    }

    namespace GridExtensions
    {
        public static class MyExtensions
        {
            public static void SetGridPosition(this Ellipse element, int col, int row)
            {
                Grid.SetColumn(element, col);
                Grid.SetRow(element, row);
            }
        }
    }
}
