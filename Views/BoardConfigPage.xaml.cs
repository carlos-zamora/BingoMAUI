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

        private void _OnBoardNameCompleted(object sender, EventArgs e)
        {
            BoardSizeEntry.Focus();
        }

        private void _OnBoardSizeCompleted(object sender, EventArgs e)
        {
            _FocusCellEntryAtIndex(0);
        }

        // When Enter is pressed, move to the next entry field.
        // If there are no more entry fields, unfocus the current one to hide the keyboard.
        private void _OnEntryCompleted(object sender, EventArgs e)
        {
            if (sender is Entry currentEntry)
            {
                var entries = _GetAllEntries();
                var currentIndex = entries.IndexOf(currentEntry);
                if (currentIndex >= 0 && currentIndex < entries.Count - 1)
                {
                    entries[currentIndex + 1].Focus();
                }
                else
                {
                    currentEntry.Unfocus();
                }
            }
        }

        private List<Entry> _GetAllEntries()
        {
            return EntryListView.GetVisualTreeDescendants()
                    .OfType<Entry>()
                    .Where(entry => entry.IsEnabled && entry.IsVisible)
                    .ToList();
        }

        private void _FocusCellEntryAtIndex(int index)
        {
            var entries = _GetAllEntries();
            if (index < entries.Count)
            {
                entries[index].Focus();
            }
        }
    }
}
