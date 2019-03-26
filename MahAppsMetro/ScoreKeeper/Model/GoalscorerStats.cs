using System;

namespace ScoreKeeper.Model
{
    internal class GoalscorerStats
    {
        public string Name { get; set; }
        public int Goals { get; set; }
        public int Penalties { get; set; }
        public override string ToString()
        {
            return String.Format("{0} {1}{2}",
                Name, Goals, Penalties > 0
                    ? String.Format(" ({0} pens)", Penalties)
                    : "");
        }
    }
}