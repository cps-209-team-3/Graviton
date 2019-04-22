using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    internal interface GameReporter
    {
        void GameOver();
        void DisplayStats(GameStats gameStats);
        void DisplaySecondsTillStart(int seconds);
        void DisplayError(string s);
    }
}
