using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ScoreKeeper.Model;
using ScoreKeeper.Views;

namespace ScoreKeeper.ViewModels
{
    class EditMatchViewModel : ViewModelBase
    {
        private readonly Match match;
        private readonly ScoreViewModel halfTimeScore;
        private readonly ScoreViewModel fullTimeScore;
        private readonly ScoreViewModel extraTimeScore;
        private readonly ScoreViewModel penaltiesScore;
        private readonly ScoreViewModel aggregateScore;
        private Goal selectedGoal;
        private Substitution selectedSubstitution;

        public EditMatchViewModel(Match match)
        {
            this.match = match;
            VenueTypes = new[]
            {
                VenueType.Home,
                VenueType.Away,
                VenueType.Neutral
            };
            CompetitionTypes = new[]
            {
                CompetitionType.League,
                CompetitionType.FaCup,
                CompetitionType.LeagueCup,
                CompetitionType.ChampionsLeague,
                CompetitionType.Friendly,
            };
            StartingEleven = match.StartingEleven
                .Select((p, n) => new StartingPlayerViewModel(match.StartingEleven, n))
                .ToArray();

            halfTimeScore = new ScoreViewModel(GetMatchScore(match, ScoreType.HalfTime));
            fullTimeScore = new ScoreViewModel(GetMatchScore(match, ScoreType.FullTime));
            extraTimeScore = new ScoreViewModel(GetMatchScore(match, ScoreType.AfterExtraTime));
            penaltiesScore = new ScoreViewModel(GetMatchScore(match, ScoreType.Penalties));
            aggregateScore = new ScoreViewModel(GetMatchScore(match, ScoreType.Aggregate));

            Goals = new ObservableCollection<Goal>(match.Goals);
            AddGoal = new RelayCommand(OnAddGoal);
            RemoveGoal =  new RelayCommand(OnRemoveGoal, o => o != null);

            Substitutions = new ObservableCollection<Substitution>(match.Substitutions);
            AddSub = new RelayCommand(OnAddSub);
            RemoveSub = new RelayCommand(OnRemoveSub, o => o != null);
        }

        private void OnRemoveGoal(object obj)
        {
            Goals.Remove(SelectedGoal);
        }

        private void OnRemoveSub(object obj)
        {
            Substitutions.Remove(SelectedSubstitution);
        }

        public Goal SelectedGoal
        {
            get { return selectedGoal; }
            set
            {
                if (Equals(value, selectedGoal)) return;
                selectedGoal = value;
                OnPropertyChanged();
            }
        }

        public Substitution SelectedSubstitution
        {
            get { return selectedSubstitution; }
            set
            {
                if (Equals(value, selectedSubstitution)) return;
                selectedSubstitution = value;
                OnPropertyChanged();
            }
        }

        private void OnAddGoal(object obj)
        {
            var w = new Window();
            w.Width = 400;
            w.Height = 250;
            w.Title = "Add new goal";
            w.Content = new GoalView();
            var vm = new GoalViewModel();
            w.DataContext = vm;
            if (w.ShowDialog().GetValueOrDefault())
            {
                Goals.Add(new Goal() { 
                    GoalType = vm.GoalType, 
                    Minute = vm.Minute, 
                    Scorer = vm.Scorer});
            }
        }

        private void OnAddSub(object obj)
        {
            var w = new Window();
            w.Width = 400;
            w.Height = 250;
            w.Title = "Add new substitution";
            w.Content = new SubstitutionView();
            var vm = new SubstitutionViewModel();
            w.DataContext = vm;
            if (w.ShowDialog().GetValueOrDefault())
            {
                Substitutions.Add(new Substitution()
                {
                    PlayerOff = vm.PlayerOff,
                    PlayerOn = vm.PlayerOn,
                    Minute = vm.Minute
                });
            }
        }


        private static Score GetMatchScore(Match match, ScoreType scoreType)
        {
            return match.Scores.FirstOrDefault(s => s.ScoreType == scoreType) ?? new Score() { ScoreType = scoreType, NotApplicable = true};
        }

        public StartingPlayerViewModel[] StartingEleven { get; private set; }

        public VenueType[] VenueTypes { get; set; }

        public CompetitionType[] CompetitionTypes { get; set; }

        public VenueType VenueType
        {
            get { return match.Venue.VenueType; }
            set
            {
                if (value == match.Venue.VenueType) return;
                match.Venue.VenueType = value;
                OnPropertyChanged();
            }
        }

        public CompetitionType CompetitionType
        {
            get { return match.Competition.CompetitionType; }
            set
            {
                if (value == match.Competition.CompetitionType) return;
                match.Competition.CompetitionType = value;
                OnPropertyChanged();
            }
        }

        public string Competition
        {
            get { return match.Competition.CompetitionName; }
            set
            {
                if (value == match.Competition.CompetitionName) return;
                match.Competition.CompetitionName = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get
            {
                return match.Date;
            }
            set
            {
                if (match.Date == value) return;
                match.Date = value;
                OnPropertyChanged();
            }
        }

        public string Opponents
        {
            get { return match.Opponents; }
            set
            {
                if (match.Opponents == value) return;
                match.Opponents = value;
                OnPropertyChanged();
            }
        }

        public string Stadium
        {
            get { return match.Venue.VenueName; }
            set
            {
                if (value == match.Venue.VenueName) return;
                match.Venue.VenueName = value;
                OnPropertyChanged();
            }
        }

        public int Attendance
        {
            get { return match.Venue.Attendance; }
            set
            {
                if (value == match.Venue.Attendance) return;
                match.Venue.Attendance = value;
                OnPropertyChanged();
            }
        }

        public ScoreViewModel HalfTimeScore
        {
            get { return halfTimeScore; }
        }

        public ScoreViewModel FullTimeScore
        {
            get { return fullTimeScore; }
        }

        public ScoreViewModel ExtraTimeScore
        {
            get { return extraTimeScore; }
        }

        public ScoreViewModel PenaltiesScore
        {
            get { return penaltiesScore; }
        }

        public ScoreViewModel AggregateScore
        {
            get { return aggregateScore; }
        }

        public ObservableCollection<Goal> Goals { get; private set; }
        public ObservableCollection<Substitution> Substitutions { get; private set; }

        public RelayCommand AddGoal { get; private set; }
        public RelayCommand RemoveGoal { get; private set;  }

        public RelayCommand AddSub { get; private set; }
        public RelayCommand RemoveSub { get; private set; }
    }
}
