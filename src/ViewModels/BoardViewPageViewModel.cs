using System.Collections.ObjectModel;
using System.Windows.Input;
using BingoMAUI.Services;
using BingoMAUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BingoMAUI.ViewModels
{
    public partial class BoardViewPageViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBingoBoardService _boardService;
        private BingoBoard? _currentBoard;

        [ObservableProperty]
        private string? _boardName = null;

        [ObservableProperty]
        private int _gridSize;

        [ObservableProperty]
        private ObservableCollection<BingoCellViewModel> _cells;

        public ICommand CellToggledCommand { get; }

        public BoardViewPageViewModel(IBingoBoardService boardService)
        {
            _boardService = boardService;
            _cells = new ObservableCollection<BingoCellViewModel>();
            CellToggledCommand = new Command<BingoCellViewModel>(async (cell) => await OnCellToggled(cell));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("boardId"))
            {
                var boardId = query["boardId"].ToString();
                Task.Run(async () => await LoadBoard(boardId));
            }
        }

        private async Task LoadBoard(string? boardId)
        {
            if (string.IsNullOrEmpty(boardId))
            {
                return;
            }

            _currentBoard = await _boardService.GetBoardByIdAsync(boardId);
            if (_currentBoard != null)
            {
                BoardName = _currentBoard.Name;
                GridSize = _currentBoard.Size;

                var newCells = new ObservableCollection<BingoCellViewModel>();
                for (int i = 0; i < _currentBoard.Content.Length; i++)
                {
                    newCells.Add(new BingoCellViewModel(_currentBoard.Content[i], i, _currentBoard.Marked[i]));
                }
                Cells = newCells;
            }
        }

        private async Task OnCellToggled(BingoCellViewModel cell)
        {
            if (_currentBoard == null || cell == null)
            {
                return;
            }

            cell.IsMarked = !cell.IsMarked;
            _currentBoard.Marked[cell.Index] = cell.IsMarked;

            await _boardService.SaveBoardAsync(_currentBoard);
        }
    }

    public partial class BingoCellViewModel : ObservableObject
    {
        public string Text { get; }
        public int Index { get; }

        [ObservableProperty]
        private bool _isMarked;

        public BingoCellViewModel(string text, int index, bool isMarked)
        {
            Text = text;
            Index = index;
            _isMarked = isMarked;
        }
    }
}
