using System.Collections.Generic;
using NUnit.Framework;

namespace GravitonClient
{
    [TestFixture]
    public class GameEngineTests
    {
        [Test]
        public void Test_SpawnWell()
        {
            Game myGame = new Game(false);
            int num = myGame.StableWells.Count;
            myGame.GameObjects = new List<GameObject>();
            myGame.SpawnWell();
            Assert.IsTrue(myGame.StableWells.Count == num + 1);
        }
        [Test]
        public void Test_SpawnOrb()
        {
            Game myGame = new Game(false);
            int num = myGame.Orbs.Count;
            myGame.GameObjects = new List<GameObject>();
            myGame.SpawnOrb();
            Assert.IsTrue(myGame.Orbs.Count == num + 1);
        }
        [Test]
        public void Test_KeyPressed()
        {
            Game myGame = new Game(false);
            myGame.KeyPressed('w');
            Assert.IsTrue(myGame.VerticalInput == -1);
            myGame.KeyPressed('s');
            Assert.IsTrue(myGame.VerticalInput == 0);
            myGame.KeyReleased('w');
            Assert.IsTrue(myGame.VerticalInput == 1);
        }
        [Test]
        public void Test_UpdateUser()
        {
            Game myGame = new Game(false);
            double num = myGame.Player.Xcoor;
            myGame.KeyPressed('d');
            myGame.UpdatePlayer();
            Assert.IsTrue(myGame.Player.Xcoor != num);
        }
    }
}

