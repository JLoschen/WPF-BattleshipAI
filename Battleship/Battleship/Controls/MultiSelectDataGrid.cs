using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Battleship.Controls
{
    public class MultiSelectDataGrid : DataGrid
    {
        private bool _mouseLeftClickDown;
        private bool _shiftIsPressed;

        public MultiSelectDataGrid()
        {
            SelectionChanged += CustomDataGrid_SelectionChanged;
            Drop += MultiSelectDataGrid_Drop;
            MouseDoubleClick += MultiSelectDataGrid_MouseDoubleClick;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (SelectionChangedCommand != null)//only need to listen for events to know when to fire command
            {
                MouseUp += OnMouseUp;
                PreviewMouseDown += OnMouseDown;
                KeyDown += OnKeyDown;
                KeyUp += OnKeyUp;
            }
        }

        #region RowSelectionEvents
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            _mouseLeftClickDown = true;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            _mouseLeftClickDown = false;
            OnSelectionChanged();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.LeftShift && e.Key != Key.RightShift) return;
            _shiftIsPressed = true;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.LeftShift && e.Key != Key.RightShift) return;
            _shiftIsPressed = false;
            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            if (SelectionChangedCommand != null && SelectionChangedCommand.CanExecute(SelectedItems))
            {
                SelectionChangedCommand.Execute(SelectedItems);
            }
        }
        #endregion

        private void MultiSelectDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DoubleClickCommand != null && DoubleClickCommand.CanExecute(SelectedItem))
            {
                DoubleClickCommand.Execute(SelectedItem);
            }
        }

        private void MultiSelectDataGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) &&
                DropCommand != null &&
                DropCommand.CanExecute(e.Data.GetData(DataFormats.StringFormat)))
            {
                DropCommand.Execute(e.Data.GetData(DataFormats.StringFormat));
            }

            if (DropDataCommand != null && DropDataCommand.CanExecute(e.Data))
            {
                DropDataCommand.Execute(e.Data);
            }
        }

        void CustomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsList = SelectedItems;

            if (!_shiftIsPressed && !_mouseLeftClickDown)
                OnSelectionChanged();
        }

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register(nameof(SelectedItemsList), typeof(IList), typeof(MultiSelectDataGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.RegisterAttached(nameof(SelectionChangedCommand), typeof(ICommand), typeof(MultiSelectDataGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.RegisterAttached(nameof(DropCommand), typeof(ICommand), typeof(MultiSelectDataGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty DropDataCommandProperty =
            DependencyProperty.RegisterAttached(nameof(DropDataCommand), typeof(ICommand), typeof(MultiSelectDataGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty DoubleClickProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommand", typeof(ICommand), typeof(MultiSelectDataGrid), new PropertyMetadata(null));

        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        public ICommand DropCommand
        {
            get { return (ICommand)GetValue(DropCommandProperty); }
            set { SetValue(DropCommandProperty, value); }
        }

        public ICommand DropDataCommand
        {
            get { return (ICommand)GetValue(DropDataCommandProperty); }
            set { SetValue(DropDataCommandProperty, value); }
        }

        public ICommand DoubleClickCommand
        {
            get { return (ICommand)GetValue(DoubleClickProperty); }
            set { SetValue(DoubleClickProperty, value); }
        }
    }
}

