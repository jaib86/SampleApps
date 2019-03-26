using System;

namespace ScoreKeeper.Model
{
    internal class AppearanceStats
    {
        public string Name { get; set; }
        public int Starts { get; set; }
        public int Subs { get; set; }
        public int Total { get { return Starts + Subs; } }
        public override string ToString()
        {
            return String.Format("{0} {1}{2}", Name, Total, Subs > 0
                ? String.Format(" ({0} as sub)", Subs)
                : "");
        }
    }
}