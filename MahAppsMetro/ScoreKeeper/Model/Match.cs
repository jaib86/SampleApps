using System;
using System.Collections.Generic;
using System.Linq;

namespace ScoreKeeper.Model
{
    enum VenueType
    {
        Home,
        Away,
        Neutral
    }

    enum ScoreType
    {
        HalfTime,
        FullTime,
        AfterExtraTime,
        Penalties,
        Aggregate
    }

    class Score
    {
        public ScoreType ScoreType { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public bool NotApplicable { get; set; }
    }

    class Venue
    {
        public VenueType VenueType { get; set; }
        public string VenueName { get; set; }
        public int Attendance { get; set; }
    }

    enum CompetitionType
    {
        League,
        FaCup,
        LeagueCup,
        ChampionsLeague,
        Friendly
    }

    class Competition
    {
        public CompetitionType CompetitionType { get; set; }
        public string CompetitionName { get; set; }
        public string CompetitionRound { get; set; }
    }

    class Player
    {
        public string Name { get; set; }
    }

    class Match
    {
        public Match()
        {
            Date = DateTime.Today;
            Venue = new Venue();
            Goals = new List<Goal>();
            Competition = new Competition();            
            Substitutions = new List<Substitution>();
            Scores = new List<Score>();
        }

        public DateTime Date { get; set; }
        public string Opponents { get; set; }
        public Venue Venue { get; set; }
        public List<Score> Scores { get; set; }
        public Competition Competition { get; set; }
        public List<string> StartingEleven { get; set; }
        public List<Goal> Goals { get; set; }
        public List<Substitution> Substitutions { get; set; }
        public List<string> Links { get; set; }

        /// <summary>
        /// On the night final score, not including penalties
        /// </summary>
        public Score FinalScore 
        {
            get
            {
                return Scores
                    .Where(s => !s.NotApplicable)
                    .FirstOrDefault(s => s.ScoreType == ScoreType.AfterExtraTime) ??
                Scores
                    .Where(s => !s.NotApplicable)
                    .FirstOrDefault(s => s.ScoreType == ScoreType.FullTime);
            }
        }

        /// <summary>
        /// On the night deciding score, including penalties, but not taking aggregate 
        /// score into account
        /// </summary>
        public Score DecidingScore {
            get
            {
                return Scores
                    .Where(s => !s.NotApplicable)
                    .FirstOrDefault(s => s.ScoreType == ScoreType.Penalties) ??
                Scores
                    .Where(s => !s.NotApplicable)
                    .FirstOrDefault(s => s.ScoreType == ScoreType.AfterExtraTime) ??
                Scores
                    .Where(s => !s.NotApplicable)
                    .FirstOrDefault(s => s.ScoreType == ScoreType.FullTime);
            }
        }

        public bool IsWin
        {
            get { return DecidingScore.GoalsFor > DecidingScore.GoalsAgainst; }
        }

        public bool IsDraw
        {
            get { return DecidingScore.GoalsFor == DecidingScore.GoalsAgainst; }       
        }

        public bool IsLoss
        {
            get { return DecidingScore.GoalsFor < DecidingScore.GoalsAgainst; }
        }

        public static Match CreateNew()
        {
            var m = new Match();
            m.StartingEleven = Enumerable.Range(1, 11).Select(n => "").ToList();
            return m;
        }
    }

    class Substitution
    {
        public string PlayerOn { get; set; }
        public string PlayerOff { get; set; }
        public int? Minute { get; set; }
    }

    class Goal
    {
        public string Scorer { get; set; }
        public GoalType GoalType { get; set; }
        public bool IsConceded { get; set; }
        public int? Minute { get; set; }

        public string Description
        {
            get
            {
                switch (GoalType)
                {
                    case GoalType.OwnGoal:
                        return "(o.g.)";
                    case GoalType.Penalty:
                        return "(pen)";
                }
                return "";
            }
        }
    }

    enum GoalType
    {
        Goal,
        Penalty,
        OwnGoal,
    }
}
