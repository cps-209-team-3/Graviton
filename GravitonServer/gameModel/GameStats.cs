using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonServer
{
    internal class GameStats
    {
        private HighScores HighScores;
        internal void SetHighScores(HighScores hs)
        {
            HighScores = hs;
        }
        

        public string Serialize()
        {
            return HighScores.Serialize();
        }
    }
}
