using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;

namespace GravitonClient
{
    [TestFixture]
    public class HighScoresTests
    {
        [Test]
        public void CheckNewScores_HigherScore_AddsHighscore()
        {
            Game myGame = new Game(false);
            HighScores hiScore = new HighScores(new List<HiScore>());
            hiScore.HiScores.Add(new HiScore("ben", 123));
            hiScore.HiScores.Add(new HiScore("act", 234));
            hiScore.HiScores.Add(new HiScore("deb", 345));
            hiScore.HiScores.Add(new HiScore("hess", 456));
            hiScore.HiScores.Add(new HiScore("dark", 567));
            hiScore.HiScores.Add(new HiScore("bluff", 678));
            hiScore.HiScores.Add(new HiScore("schaub", 789));
            hiScore.HiScores.Add(new HiScore("mcgee", 890));
            hiScore.HiScores.Add(new HiScore("knisely", 903));
            hiScore.HiScores.Add(new HiScore("watson", 1));
            myGame.Username = "Evisserate";
            myGame.Points = 1000;
            hiScore.CheckNewScores(myGame);
            Assert.IsTrue(hiScore.HiScores.Contains(new HiScore("Evisserate", 1000)));
        }

        [Test]
        public void CheckNewScores_LowerScore_AddsNothing()
        {
            Game myGame = new Game(false);
            HighScores hiScore = new HighScores(new List<HiScore>());
            hiScore.HiScores.Add(new HiScore("ben", 123));
            hiScore.HiScores.Add(new HiScore("act", 234));
            hiScore.HiScores.Add(new HiScore("deb", 345));
            hiScore.HiScores.Add(new HiScore("hess", 456));
            hiScore.HiScores.Add(new HiScore("dark", 567));
            hiScore.HiScores.Add(new HiScore("bluff", 678));
            hiScore.HiScores.Add(new HiScore("schaub", 789));
            hiScore.HiScores.Add(new HiScore("mcgee", 890));
            hiScore.HiScores.Add(new HiScore("knisely", 903));
            hiScore.HiScores.Add(new HiScore("watson", 1));
            myGame.Username = "Evisserate";
            myGame.Points = 0;
            hiScore.CheckNewScores(myGame);
            Assert.IsFalse(hiScore.HiScores.Contains(new HiScore("Evisserate", 0)));
        }

        [Test]
        public void AddNewScore_ValidInput_Adds()
        {
            Game myGame = new Game(false);
            HighScores hiScore = new HighScores(new List<HiScore>());
            hiScore.HiScores.Add(new HiScore("ben", 123));
            hiScore.HiScores.Add(new HiScore("act", 234));
            hiScore.HiScores.Add(new HiScore("deb", 345));
            hiScore.HiScores.Add(new HiScore("hess", 456));
            hiScore.HiScores.Add(new HiScore("dark", 567));
            hiScore.HiScores.Add(new HiScore("bluff", 678));
            hiScore.HiScores.Add(new HiScore("schaub", 789));
            hiScore.HiScores.Add(new HiScore("mcgee", 890));
            hiScore.HiScores.Add(new HiScore("knisely", 903));
            hiScore.HiScores.Add(new HiScore("watson", 1));
            myGame.Username = "Evisserate";
            myGame.Points = 10;
            hiScore.AddNewScore(myGame.Username, myGame.Points);
            Assert.IsTrue(hiScore.HiScores.Contains(new HiScore("Evisserate", 10)));
        }

       
       

        

       
    }
}
