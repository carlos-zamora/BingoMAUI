using BingoMAUI.Views;

namespace BingoMAUI
{
    public partial class AppShell : Shell
    {
        private bool _hasRestoredNavigation = false;

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("BoardConfigPage", typeof(BoardConfigPage));
            Routing.RegisterRoute("BoardViewPage", typeof(BoardViewPage));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_hasRestoredNavigation) return;
            _hasRestoredNavigation = true;

            var lastBoardId = Preferences.Get("last_board_id", null as string);
            if (!string.IsNullOrEmpty(lastBoardId))
            {
                await GoToAsync($"BoardViewPage?boardId={lastBoardId}");
            }
        }
    }
}
