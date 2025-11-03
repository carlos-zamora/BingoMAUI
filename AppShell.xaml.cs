using BingoMAUI.Views;

namespace BingoMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("BoardConfigPage", typeof(BoardConfigPage));
            Routing.RegisterRoute("BoardViewPage", typeof(BoardViewPage));
        }
    }
}
