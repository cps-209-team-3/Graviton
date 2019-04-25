//This file contains the GameStats class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    //This class deals with the game statistics
    public class GameStats
    {
        internal HighScores HighScores;
        
        //This method deserializes a string into game statistics information
        public static GameStats Deserialize(string data)
        {
            GameStats stats = new GameStats();
            stats.HighScores = HighScores.Deserialize(data);
            return stats;
        }
    }
}
