using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class GameStats
    {
        internal HighScores HighScores;
        public static GameStats Deserialize(string data)
        {
            GameStats stats = new GameStats();
            stats.HighScores = HighScores.Deserialize(data);
            return stats;
        }
    }
}
