using BingoMAUI.ViewModels;

namespace BingoMAUI.Views
{
    public partial class BoardConfigPage : ContentPage
    {
        public BoardConfigPage(BoardConfigPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
