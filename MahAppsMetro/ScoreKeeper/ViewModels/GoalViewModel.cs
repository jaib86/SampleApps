using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScoreKeeper.Model;

namespace ScoreKeeper.ViewModels
{
    class GoalViewModel : ViewModelBase
    {
        private string scorer;
        private int minute;
        private GoalType goalType;

        public GoalType[] GoalTypes { get { return goalTypes; } }

        private static readonly GoalType[] goalTypes =
                                        {
                                            GoalType.Goal,
                                            GoalType.OwnGoal,
                                            GoalType.Penalty,
                                        };

        public GoalType GoalType
        {
            get { return goalType; }
            set
            {
                if (value == goalType) return;
                goalType = value;
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

        public string Scorer
        {
            get { return scorer; }
            set
            {
                if (value == scorer) return;
                scorer = value;
                OnPropertyChanged();
            }
        }
    }
}
