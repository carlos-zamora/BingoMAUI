using BingoMAUI.Helpers;
using BingoMAUI.Models;
using BingoMAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BingoMAUI.ViewModels
{
    public partial class BoardConfigPageViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBingoBoardService _boardService;

        [ObservableProperty]
        private BingoBoard? _board = null;

        [ObservableProperty]
        private string _boardNameInput = string.Empty;

        [ObservableProperty]
        private string _boardSizeInput = string.Empty;
        partial void OnBoardSizeInputChanged(string value)
        {
            _UpdateCellFields();
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ApplyConfigButtonLabel), nameof(PageTitle))]
        private bool _isNewBoard = true;

        public string ApplyConfigButtonLabel => IsNewBoard ? "Create Board" : "Save Changes";
        public string PageTitle => IsNewBoard ? "Configure New Board" : "Configure Board";

        [ObservableProperty]
        public ObservableCollection<CellInputItem> _cellInputs = new();

        public ICommand ApplyConfigCommand { get; }

        public BoardConfigPageViewModel(IBingoBoardService boardService)
        {
            _boardService = boardService;
            ApplyConfigCommand = new Command(async () => await _OnApplyConfig(), _CanApplyConfig);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _ = _LoadBoardAsync(query["boardId"] as string);
        }

        private async Task _LoadBoardAsync(string boardId)
        {
            if (boardId is null || boardId == "new")
            {
                Board = BingoBoard.CreateNewBingoBoard();
                IsNewBoard = true;
            }
            else
            {
                Board = await _boardService.GetBoardByIdAsync(boardId) ?? new BingoBoard(boardId);
                BoardNameInput = Board.Name;
                BoardSizeInput = Math.Sqrt(Board.Size).ToString();
                IsNewBoard = false;

                CellInputs.Clear();
                for (int i = 0; i < Board.Size; i++)
                {
                    var cellInput = new CellInputItem(Board.Content[i], i+1);
                    cellInput.PropertyChanged += (s, e) =>
                    {
                        ((Command)ApplyConfigCommand).ChangeCanExecute();
                    };
                    CellInputs.Add(cellInput);
                }
                ((Command)ApplyConfigCommand).ChangeCanExecute();
            }
        }

        private void _UpdateCellFields()
        {
            if (int.TryParse(BoardSizeInput, out int size) && size > 0 && size <= 10)
            {
                int totalCells = size * size;

                // Preserve existing values
                var existingValues = CellInputs.Select(c => c.Value).ToList();

                CellInputs.Clear();
                for (int i = 0; i < totalCells; i++)
                {
                    var cellInput = new CellInputItem(i < existingValues.Count ? existingValues[i] : string.Empty, i + 1);
                    cellInput.PropertyChanged += (s, e) =>
                    {
                        ((Command)ApplyConfigCommand).ChangeCanExecute();
                    };
                    CellInputs.Add(cellInput);
                }
            }
            else
            {
                CellInputs.Clear();
            }

            ((Command)ApplyConfigCommand).ChangeCanExecute();
        }

        private bool _CanApplyConfig()
        {
            return !string.IsNullOrWhiteSpace(BoardNameInput) &&
                   int.TryParse(BoardSizeInput, out int size) && size > 0 &&
                   CellInputs.Count == size * size &&
                   CellInputs.All(c => !string.IsNullOrWhiteSpace(c.Value));
        }

        private async Task _OnApplyConfig()
        {
            if (Board is not null && !IsNewBoard)
            {
                await _boardService.SaveBoardAsync(Board);
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.GoToAsync($"//MainPage/BoardViewPage?boardId={Board.Id}");
                });
            }
            else
            {
                var content = EnumerableExtensions.Shuffle(CellInputs.Select(c => c.Value));
                var newBoard = new BingoBoard(BoardNameInput, CellInputs.Count, content.ToArray());
                await _boardService.SaveBoardAsync(newBoard);
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.GoToAsync($"//MainPage/BoardViewPage?boardId={newBoard.Id}");
                });
            }
        }
    }

    public partial class CellInputItem : ObservableObject
    {
        [ObservableProperty]
        private string _value;

        public int Index { get; }

        public CellInputItem(string value, int index)
        {
            Value = value;
            Index = index;
        }
    }
}
