using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GravitonClient
{
    [TestFixture]
    public class HighScoresTests
    {
      [Test]
        public void CheckNewScores_HigherScore_AddsHighscore()
        {
            Game myGame = new Game(false);
            HighScores hiScore = new HighScores(new Dictionary<string, int>());
            hiScore.HiScores.Add("ben", 123);
            hiScore.HiScores.Add("act", 234);
            hiScore.HiScores.Add("deb", 345);
            hiScore.HiScores.Add("hess", 456);
            hiScore.HiScores.Add("dark", 567);
            hiScore.HiScores.Add("bluff", 678);
            hiScore.HiScores.Add("schaub", 789);
            hiScore.HiScores.Add("mcgee", 890);
            hiScore.HiScores.Add("knisely", 903);
            hiScore.HiScores.Add("watson", 1);
            myGame.Username = "Evisserate";
            myGame.Points = 1000;
            hiScore.CheckNewScores(myGame);
            Assert.IsTrue(hiScore.HiScores.ContainsKey("Evisserate"));
        }

        [Test]
        public void CheckNewScores_LowerScore_AddsNothing()
        {
            Game myGame = new Game(false);
            HighScores hiScore = new HighScores(new Dictionary<string, int>());
            hiScore.HiScores.Add("ben", 123);
            hiScore.HiScores.Add("act", 234);
            hiScore.HiScores.Add("deb", 345);
            hiScore.HiScores.Add("hess", 456);
            hiScore.HiScores.Add("dark", 567);
            hiScore.HiScores.Add("bluff", 678);
            hiScore.HiScores.Add("schaub", 789);
            hiScore.HiScores.Add("mcgee", 890);
            hiScore.HiScores.Add("knisely", 903);
            hiScore.HiScores.Add("watson", 1);
            myGame.Username = "Evisserate";
            myGame.Points = 0;
            hiScore.CheckNewScores(myGame);
            Assert.IsFalse(hiScore.HiScores.ContainsKey("Evisserate"));
        }

        [Test]
        public void AddNewScore_ValidInput_Adds()
        {
            Game myGame = new Game(false);
            HighScores hiScore = new HighScores(new Dictionary<string, int>());
            hiScore.HiScores.Add("ben", 123);
            hiScore.HiScores.Add("act", 234);
            hiScore.HiScores.Add("deb", 345);
            hiScore.HiScores.Add("hess", 456);
            hiScore.HiScores.Add("dark", 567);
            hiScore.HiScores.Add("bluff", 678);
            hiScore.HiScores.Add("schaub", 789);
            hiScore.HiScores.Add("mcgee", 890);
            hiScore.HiScores.Add("knisely", 903);
            hiScore.HiScores.Add("watson", 1);
            myGame.Username = "Evisserate";
            myGame.Points = 0;
            hiScore.AddNewScore(myGame.Username, myGame.Points);
            Assert.IsTrue(hiScore.HiScores.ContainsKey("Evisserate"));
        }

        [Test]
        public void Load_InputFile_ReturnsHighScoresObject()
        {
            string path = "PATH";
            HighScores score = HighScores.Load(path);
            Assert.IsFalse(score == null);
        }

        [Test]
        public void Save_InputFile_NoExceptionThrown()
        {
            string path = "PATH";
            try
            {
                HighScores score = HighScores.Save(path);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void Serialize_InitializedObject_StringNotEmpty()
        {
            HighScores hiScore = new HighScores(new Dictionary<string, int>());
            hiScore.HiScores.Add("ben", 123);
            hiScore.HiScores.Add("act", 234);
            hiScore.HiScores.Add("deb", 345);
            hiScore.HiScores.Add("hess", 456);
            hiScore.HiScores.Add("dark", 567);
            hiScore.HiScores.Add("bluff", 678);
            hiScore.HiScores.Add("schaub", 789);
            hiScore.HiScores.Add("mcgee", 890);
            hiScore.HiScores.Add("knisely", 903);
            hiScore.HiScores.Add("watson", 1);
            string serialized = hiScore.Serialize();
            Assert.IsFalse(serialized == "");
        }

        [Test]
        public void Deserialize_InputSerializedString_ReturnsHighScoresObject()
        {
            string serialized = "IAMSERIALIZED";
            HighScores score = HighScores.Deserialize(serialized);
            Assert.IsFalse(score == null);
        }
    }
}