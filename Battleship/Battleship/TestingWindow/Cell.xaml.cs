using System.Windows;
using System.Windows.Input;

namespace Battleship.TestingWindow
{
    public partial class Cell 
    {
        public Cell()
        {
            InitializeComponent();
            (Content as FrameworkElement).DataContext = this;//found at the URL mentioned above.
        }

        public static readonly DependencyProperty AttackCodeProperty = DependencyProperty.Register("AttackCode", typeof(int), typeof(Cell), new PropertyMetadata(0));
        public static readonly DependencyProperty LocationIdProperty = DependencyProperty.Register("LocationId", typeof(int), typeof(Cell), new PropertyMetadata(0));
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(Cell));
        public static readonly DependencyProperty RightClickCommandProperty = DependencyProperty.Register("RightClickCommand", typeof(ICommand), typeof(Cell));

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
