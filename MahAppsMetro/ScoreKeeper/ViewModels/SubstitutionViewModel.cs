namespace ScoreKeeper.ViewModels
{
    class SubstitutionViewModel : ViewModelBase
    {
        private string playerOn;
        private string playerOff;
        private int minute;

        public string PlayerOn
        {
            get { return playerOn; }
            set
            {
                if (value == playerOn) return;
                playerOn = value;
                OnPropertyChanged();
            }
        }

        public string PlayerOff
        {
            get { return playerOff; }
            set
            {
                if (value == playerOff) return;
                playerOff = value;
                OnPropertyChanged();
            }
        }

        public int Minute
        {
            get { return minute; }
            set
            {
                if (value == minute) return;
                minute = value;
                OnPropertyChanged();
            }
        }
    }
}
