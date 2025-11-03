using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using BingoMAUI.Models;
using BingoMAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BingoMAUI.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IBingoBoardService _boardService;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private ObservableCollection<BingoBoard> _boards = new();

        public ICommand AddNewBoardCommand { get; }
        public ICommand BoardTappedCommand { get; }
        public ICommand EditBoardCommand { get; }
        public ICommand DeleteBoardCommand { get; }
        public ICommand RefreshCommand { get; }

        public MainPageViewModel(IBingoBoardService boardService)
        {
            _boardService = boardService;
            AddNewBoardCommand = new Command(async () => await OnAddNewBoard());
            BoardTappedCommand = new Command<BingoBoard>(async (board) => await OnBoardTapped(board));
            EditBoardCommand = new Command<BingoBoard>(async (board) => await OnEditBoard(board));
            DeleteBoardCommand = new Command<BingoBoard>(async (board) => await OnDeleteBoard(board));
            RefreshCommand = new Command(async () => await _LoadBoards());
        }

        public async Task InitializeAsync()
        {
            await _LoadBoards();
        }

        private async Task _LoadBoards()
        {
            IsLoading = true;
            try
            {
                var boards = await _boardService.GetAllBoardsAsync();
                Boards.Clear();
                foreach (var board in boards)
                {
                    Boards.Add(board);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task OnAddNewBoard()
        {
            await Shell.Current.GoToAsync("BoardConfigPage?boardId=new");
        }

        private async Task OnBoardTapped(BingoBoard board)
        {
            if (board == null)
            {
                return;
            }
            await Shell.Current.GoToAsync($"BoardViewPage?boardId={board.Id}");
        }

        private async Task OnEditBoard(BingoBoard board)
        {
            if (board == null)
            {
                return;
            }
            await Shell.Current.GoToAsync($"BoardConfigPage?boardId={board.Id}");
        }

        private async Task OnDeleteBoard(BingoBoard board)
        {
            if (board == null)
            {
                return;
            }
            await _boardService.DeleteBoardAsync(board.Id);
            Boards.Remove(board);
        }
    }
}
