using BingoMAUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BingoMAUI.ViewModels
{
    public partial class BoardViewModel : ObservableObject
    {
        private readonly BingoBoard _board;
        private readonly MainPageViewModel _parent;

        public string Id => _board.Id;
        public string Name => _board.Name;
        public int Size => _board.Size;

        public BoardViewModel(BingoBoard board, MainPageViewModel parent) 
        {
            _board = board;
            _parent = parent;
        }

        [RelayCommand]
        public async Task Navigate()
        {
            Preferences.Set("last_board_id", Id);
            await Shell.Current.GoToAsync($"BoardViewPage?boardId={Id}");
        }

        [RelayCommand]
        public async Task Edit()
        {
            await Shell.Current.GoToAsync($"BoardConfigPage?boardId={Id}");
        }

        [RelayCommand]
        public async Task Delete()
        {
            await _parent.DeleteBoardAsync(Id);
        }
    }
}
