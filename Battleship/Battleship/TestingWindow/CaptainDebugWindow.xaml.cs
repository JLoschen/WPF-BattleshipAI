using System;
using System.Windows;
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
            var row = Grid.GetRow(cell);
            var col = Grid.GetColumn(cell);

            switch (vm.GameState)
            {
                case GameState.HumanPlayerPlacingPatrol:
                    if (!vm.IsSettingVertical && col < 10)
                        PatrolHorizontal.SetGridPosition(col, row);
                    else if(row < 9)
                        PatrolVertical.SetGridPosition(col, row);
                    break;
                case GameState.HumanPlayerPlacingDestroyer:
                    if (!vm.IsSettingVertical && col < 9)
                        DestroyerHorizontal.SetGridPosition(col, row);
                    else if (row < 8)
                        DestroyerVertical.SetGridPosition(col, row);
                    break;
                case GameState.HumanPlayerPlacingSubmarine:
                    if (!vm.IsSettingVertical && col < 9)
                        SubmarineHorizontal.SetGridPosition(col, row);
                    else if (row < 9)
                        SubmarineVertical.SetGridPosition(col, row);
                    break;
                case GameState.HumanPlayerPlacingBattleship:
                    if (!vm.IsSettingVertical && col < 8)
                        BattleshipHorizontal.SetGridPosition(col, row);
                    else if (row < 7)
                        BattleshipVertical.SetGridPosition(col, row);
                    break;
                case GameState.HumanPlayerPlacingAircraftCarrier:
                    if (!vm.IsSettingVertical && col < 7)
                        AircraftCarrierHorizontal.SetGridPosition(col, row);
                    else if (row < 6)
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
