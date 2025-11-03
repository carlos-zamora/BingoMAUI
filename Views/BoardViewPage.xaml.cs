using BingoMAUI.ViewModels;
using System.Collections.Specialized;

namespace BingoMAUI.Views
{
    public partial class BoardViewPage : ContentPage
    {
        private readonly BoardViewPageViewModel _viewModel;

        public BoardViewPage(BoardViewPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.Cells))
            {
                _BuildGrid();
            }
        }

        private void _BuildGrid()
        {
            // exit early if VM is not fully loaded
            if (_viewModel.Cells.Count == 0)
            {
                return;
            }

            BingoGrid.Children.Clear();
            BingoGrid.RowDefinitions.Clear();
            BingoGrid.ColumnDefinitions.Clear();

            // Create NxN Grid
            var dimensionLength = (int)Math.Sqrt(_viewModel.Cells.Count);
            for (int i = 0; i < dimensionLength; i++)
            {
                BingoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                BingoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            // Add buttons
            for (int i = 0; i < _viewModel.Cells.Count; i++)
            {
                var cell = _viewModel.Cells[i];
                int row = i / dimensionLength;
                int col = i % dimensionLength;

                var button = new Button
                {
                    Text = cell.Text,
                    CommandParameter = cell
                };

                button.SetBinding(Button.CommandProperty, new Binding(nameof(_viewModel.CellToggledCommand)));
                button.SetBinding(Button.BackgroundColorProperty,
                                  new Binding(nameof(cell.IsMarked),
                                  source: cell,
                                  converter: new MarkedToColorConverter()));

                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                BingoGrid.Children.Add(button);
            }
        }
    }

    public class MarkedToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isMarked && isMarked)
            {
                return Colors.Green;
            }
            return Colors.Gray;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
