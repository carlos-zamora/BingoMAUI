using BingoMAUI.Models;
using BingoMAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ApplyConfigButtonLabel), nameof(PageTitle))]
        private bool _isNewBoard = true;

        public string ApplyConfigButtonLabel => _isNewBoard ? "Create Board" : "Save Changes";
        public string PageTitle => _isNewBoard ? "Configure New Board" : "Configure Board";

        partial void OnBoardSizeInputChanged(string value)
        {
            UpdateCellFields();
        }

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

        private void UpdateCellFields()
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
            if (_board is not null && !_isNewBoard)
            {
                await _boardService.SaveBoardAsync(_board);
                await Shell.Current.GoToAsync($"//MainPage/BoardViewPage?boardId={_board.Id}");
            }
            else
            {
                var content = CellInputs.Select(c => c.Value)
                                        .Shuffle()
                                        .ToArray();
                var newBoard = new BingoBoard(BoardNameInput, CellInputs.Count, content);
                await _boardService.SaveBoardAsync(newBoard);
                await Shell.Current.GoToAsync($"//MainPage/BoardViewPage?boardId={newBoard.Id}");
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
