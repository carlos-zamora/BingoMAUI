using System.Collections.ObjectModel;
using BingoMAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BingoMAUI.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IBingoBoardService _boardService;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private ObservableCollection<BoardViewModel> _boardVMs = new();

        public MainPageViewModel(IBingoBoardService boardService)
        {
            _boardService = boardService;
        }

        public async Task InitializeAsync()
        {
            await LoadBoards();
        }

        [RelayCommand]
        private async Task LoadBoards()
        {
            IsLoading = true;
            try
            {
                var boards = await _boardService.GetAllBoardsAsync();
                BoardVMs.Clear();
                foreach (var board in boards)
                {
                    BoardVMs.Add(new BoardViewModel(board, this));
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task AddNewBoard()
        {
            await Shell.Current.GoToAsync("BoardConfigPage?boardId=new");
        }

        public async Task DeleteBoardAsync(string boardId)
        {
            await _boardService.DeleteBoardAsync(boardId);
            var boardToRemove = BoardVMs.FirstOrDefault(b => b.Id == boardId);
            if (boardToRemove != null)
            {
                BoardVMs.Remove(boardToRemove);
            }
        }
    }
}
