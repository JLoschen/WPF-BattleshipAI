using System.Windows;
using Battleship.Core;

namespace Battleship.TestingWindow.UserControls
{
    public partial class GameBoard 
    {
        public GameBoard()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FleetProperty = DependencyProperty.Register("Fleet", typeof(Fleet), typeof(GameBoard), new PropertyMetadata(null));

        public Fleet Fleet
        {
            get { return (Fleet)GetValue(FleetProperty); }
            set { SetValue(FleetProperty, value); }
        }
    }
}
