using System.Windows;

namespace Battleship.TestingWindow
{
    public partial class ShotCircle
    {
        public ShotCircle()
        {
            InitializeComponent();
            (Content as FrameworkElement).DataContext = this;
        }

        public static readonly DependencyProperty CellCodeProperty = DependencyProperty.Register("CellCode", typeof(int), typeof(ShotCircle), new PropertyMetadata(0));

        public int CellCode
        {
            get { return (int)GetValue(CellCodeProperty); }
            set { SetValue(CellCodeProperty, value); }
        }
    }
}
