using System;
using System.Linq;
using ScoreKeeper.Model;

namespace ScoreKeeper.ViewModels
{
    class MatchViewModel
    {
        private readonly Match match;

        public MatchViewModel(Match match)
        {
            this.match = match;
        }

        public Match Match { get { return match; } }

        public string Date
        {
            get { return match.Date.ToShortDateString(); }
        }

        public string Opponents
        {
            get { return match.Opponents; }
        }

        public string FinalScore
        {
            get
            {
                var finalScore = match.Scores.FirstOrDefault(s => s.ScoreType == ScoreType.FullTime);
                if (finalScore != null)
                    return String.Format("{0}-{1}", finalScore.GoalsFor, finalScore.GoalsAgainst);
                return "";
            }
        }
    }
}
