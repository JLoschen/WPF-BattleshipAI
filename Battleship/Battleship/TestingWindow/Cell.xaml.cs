using System.Windows;
using System.Windows.Input;

namespace Battleship.TestingWindow
{
    public partial class Cell 
    {
        public Cell()
        {
            InitializeComponent();
            (Content as FrameworkElement).DataContext = this;
        }

        public static readonly DependencyProperty AttackCodeProperty = DependencyProperty.Register("AttackCode", typeof(int), typeof(Cell), new PropertyMetadata(0));
        public static readonly DependencyProperty LocationIdProperty = DependencyProperty.Register("LocationId", typeof(int), typeof(Cell), new PropertyMetadata(0));
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(Cell));
        public static readonly DependencyProperty RightClickCommandProperty = DependencyProperty.Register("RightClickCommand", typeof(ICommand), typeof(Cell));
        public static readonly DependencyProperty MouseOverHighlightProperty = DependencyProperty.Register("MouseOverHighlight", typeof(bool), typeof(Cell));
        public static readonly DependencyProperty CellCodeProperty = DependencyProperty.Register("CellCode", typeof(int), typeof(Cell));
        public static readonly DependencyProperty GameStateProperty = DependencyProperty.Register("GameState", typeof(GameState), typeof(Cell));

        //private static void OnMouseOverHighlightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if ((bool) e.NewValue)
        //    {

        //    }
        //    else
        //    {

        //    }
        //}

        public int AttackCode
        {
            get { return (int)GetValue(AttackCodeProperty); }
            set { SetValue(AttackCodeProperty, value); }
        }

        public int LocationId
        {
            get { return (int)GetValue(LocationIdProperty); }
            set { SetValue(LocationIdProperty, value); }
        }

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public ICommand RightClickCommand
        {
            get { return (ICommand)GetValue(RightClickCommandProperty); }
            set { SetValue(RightClickCommandProperty, value); }
        }
        public bool CellCodeMouseOverHighlight
        {
            get { return (bool)GetValue(MouseOverHighlightProperty); }
            set { SetValue(MouseOverHighlightProperty, value); }
        }

        public int CellCode
        {
            get { return (int)GetValue(CellCodeProperty); }
            set { SetValue(CellCodeProperty, value); }
        }

        public GameState GameState
        {
            get { return (GameState)GetValue(GameStateProperty); }
            set { SetValue(GameStateProperty, value); }
        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClickCommand?.Execute(LocationId);
        }

        private void UIElement_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            RightClickCommand?.Execute(null);
        }
    }
}
