using System;

namespace ScoreKeeper.Model
{
    class MatchStats
    {
        public string Label { get; set; }
        public int Played { get { return Won + Drawn + Lost; } }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int For { get; set; }
        public int Against { get; set; }
        public override string ToString()
        {
            return String.Format("{6} P: {0}, W: {1}, D:{2}, L:{3}, F:{4}, A:{5}",
                Played, Won, Drawn, Lost, For, Against, Label);
        }
    }
}