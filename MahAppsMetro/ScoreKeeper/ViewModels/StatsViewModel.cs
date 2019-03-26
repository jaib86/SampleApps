using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScoreKeeper.Model;

namespace ScoreKeeper.ViewModels
{
    class StatsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<MatchViewModel> matches;

        public StatsViewModel(ObservableCollection<MatchViewModel> matches)
        {
            this.matches = matches;
            CalculateStats();
        }

        private void CalculateStats()
        {
            CalculateAppearanceStats();
            CalculateGoalscorerStats();
            CalculateMatchStats();
        }

        private void CalculateAppearanceStats()
        {
            Appearances = matches.Select(m => m.Match)
                .Where(m => m.Competition.CompetitionType != CompetitionType.Friendly)
                .SelectMany(m => m.StartingEleven.Select(p => new {Name = p, Starts = 1, Subs = 0}).Concat(
                    m.Substitutions.Select(p => new {Name = p.PlayerOn, Starts = 0, Subs = 1})))
                .GroupBy(s => s.Name)
                .Select(g => new AppearanceStats {Name = g.Key, Starts = g.Sum(s => s.Starts), Subs = g.Sum(s => s.Subs)})
                .OrderByDescending(s => s.Total)
                .ThenByDescending(s => s.Starts)
                .ToList();
        }

        private void CalculateGoalscorerStats()
        {
            Goalscorers = matches.Select(m => m.Match)
                .Where(m => m.Competition.CompetitionType != CompetitionType.Friendly)
                .SelectMany(m => m.Goals.Select(g => new { Name = g.GoalType == GoalType.OwnGoal ? "o.g." : g.Scorer, Goals = 1, Penalties = g.GoalType == GoalType.Penalty ? 1 : 0 }))
                .GroupBy(s => s.Name)
                .Select(g => new GoalscorerStats { Name = g.Key, Goals = g.Sum(s => s.Goals), Penalties = g.Sum(s => s.Penalties) })
                .OrderByDescending(s => s.Goals)
                .ThenBy(s => s.Penalties)
                .ToList();
        }

        private void CalculateMatchStats()
        {
            Results = matches.Select(m => m.Match)
                .Where(m => m.Competition.CompetitionType != CompetitionType.Friendly)
                .Select(m => new { Name = m.Competition.CompetitionType, 
                    Wins = m.IsWin ? 1:0, 
                    Draws = m.IsDraw ? 1:0, 
                    Losses = m.IsLoss ? 1:0, 
                    GoalsFor = m.FinalScore.GoalsFor, 
                    GoalsAgainst = m.FinalScore.GoalsAgainst })
                .GroupBy(s => s.Name)
                .Select(g => new MatchStats() { Label = g.Key.ToString(), 
                    Won = g.Sum(s => s.Wins),
                    Drawn = g.Sum(s => s.Draws),
                    Lost = g.Sum(s => s.Losses),
                    For = g.Sum(s => s.GoalsFor),
                    Against = g.Sum(s => s.GoalsAgainst),
                })
                .ToList();

            var total = new MatchStats()
            {
                Label = "Total",
                Won = Results.Sum(r => r.Won),
                Drawn = Results.Sum(r => r.Drawn),
                Lost = Results.Sum(r => r.Lost),
                For = Results.Sum(r => r.For),
                Against = Results.Sum(r => r.Against)
            };
            Results.Add(total);
        }


        public List<GoalscorerStats> Goalscorers { get; set; }

        public List<AppearanceStats> Appearances { get; set; }

        public List<MatchStats> Results { get; set; }
    }
}
