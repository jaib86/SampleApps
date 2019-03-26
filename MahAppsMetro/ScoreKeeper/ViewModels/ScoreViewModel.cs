using ScoreKeeper.Model;

namespace ScoreKeeper.ViewModels
{
    class ScoreViewModel : ViewModelBase
    {
        private readonly Score score;

        public ScoreViewModel(Score score)
        {
            this.score = score;
        }

        public int GoalsFor
        {
            get { return score.GoalsFor; }
            set
            {
                if (value == score.GoalsFor) return;
                score.GoalsFor = value;
                OnPropertyChanged();
            }
        }

        public int GoalsAgainst
        {
            get { return score.GoalsAgainst; }
            set
            {
                if (value == score.GoalsAgainst) return;
                score.GoalsAgainst = value;
                OnPropertyChanged();
            }
        }

        public bool IsApplicable
        {
            get { return !score.NotApplicable; }
            set
            {
                if (value.Equals(!score.NotApplicable)) return;
                score.NotApplicable = !value;
                OnPropertyChanged();
            }
        }
    }
}
