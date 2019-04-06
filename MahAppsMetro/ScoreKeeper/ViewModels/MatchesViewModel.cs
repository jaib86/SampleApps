using System.Collections.ObjectModel;
using MahApps.Metro.Controls.Dialogs;

namespace ScoreKeeper.ViewModels
{
    class MatchesViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        public RelayCommand NewMatch { get; set; }
        public RelayCommand EditMatch { get; set; }
        public RelayCommand DeleteMatch { get; set; }
        private MatchViewModel selectedMatch;

        public MatchesViewModel(ObservableCollection<MatchViewModel> matches,
            RelayCommand newMatch,
            RelayCommand editMatch,
            IDialogService dialogService)
        {
            this.dialogService = dialogService;
            NewMatch = newMatch;
            EditMatch = editMatch;
            DeleteMatch = new RelayCommand(DeleteSelectedMatch, o => o != null);
            Matches = matches;
        }

        private async void DeleteSelectedMatch(object obj)
        {
            var result = await dialogService.AskQuestionAsync("Delete Match",
                "Are you sure you want to delete this Match record?");
            if (result == MessageDialogResult.Affirmative)
            {
                Matches.Remove(selectedMatch);
                SelectedMatch = null;
            }
        }

        public ObservableCollection<MatchViewModel> Matches { get; private set; }

        public MatchViewModel SelectedMatch
        {
            get { return selectedMatch; }
            set
            {
                if (Equals(value, selectedMatch)) return;
                selectedMatch = value;
                OnPropertyChanged();
            }
        }
    }
}
