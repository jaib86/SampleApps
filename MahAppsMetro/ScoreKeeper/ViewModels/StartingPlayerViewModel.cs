using System.Collections.Generic;

namespace ScoreKeeper.ViewModels
{
    class StartingPlayerViewModel : ViewModelBase
    {
        private readonly List<string> startingList;
        private readonly int index;

        public StartingPlayerViewModel(List<string> startingList, int index )
        {
            this.startingList = startingList;
            this.index = index;
        }

        public int Number { get { return index + 1; } }

        public string Name
        {
            get { return startingList[index]; }
            set
            {
                if (startingList[index] == value) return;
                startingList[index] = value;
                OnPropertyChanged();
            }
        }
    }
}