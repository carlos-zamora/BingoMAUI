using BingoMAUI.ViewModels;
using MauiIcons.Core;

namespace BingoMAUI
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;

            // workaroud for MauiIcons XAML namespace issue
            // see https://github.com/AathifMahir/MauiIcons?tab=readme-ov-file#workaround
            _ = new MauiIcon();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InitializeAsync();
        }
    }
}
