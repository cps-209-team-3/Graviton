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
            hiScore.HiScores.Sort(HighScores.CompareHighScores);

            myGame.Username = "Evisserate";
            myGame.Points = 10;
            hiScore.AddNewScore(myGame.Username, myGame.Points);
            Assert.IsTrue(hiScore.HiScores[9].User == "Evisserate");
        }


        [Test]
        public void Load_InputFile_ReturnsHighScoresObject()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "tests/HSLoad.txt");
            
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
            hiScore.HiScores.Sort(HighScores.CompareHighScores);
            
            HighScores score = HighScores.Load(path);
            Assert.IsTrue(score.Equals(hiScore));
        }

        [Test]
        public void Save_InputFile_NoExceptionThrown()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "tests/HSSave.txt");
            
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
            hiScore.HiScores.Sort(HighScores.CompareHighScores);

            try
            {
                hiScore.Save(path);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
            
            HighScores score = HighScores.Load(path);
            Assert.IsTrue(score.Equals(hiScore));
        }

        [Test]
        public void Serialize_InitializedObject_StringNotEmpty()
        {
            string testval = "knisely%903%mcgee%890%schaub%789%bluff%678%dark%567%hess%456%deb%345%act%234%ben%123%watson%1%";
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
            hiScore.HiScores.Sort(HighScores.CompareHighScores);
            
            string serialized = hiScore.Serialize();
            Assert.IsTrue(serialized.Equals(testval));
        }

        [Test]
        public void Deserialize_InputSerializedString_ReturnsHighScoresObject()
        {
            string serialized = "knisely%903%mcgee%890%schaub%789%bluff%678%dark%567%hess%456%deb%345%act%234%ben%123%watson%1%";
            
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
            hiScore.HiScores.Sort(HighScores.CompareHighScores);

            HighScores score = HighScores.Deserialize(serialized);
            Assert.IsTrue(score.Equals(hiScore));
        }
    }
}
