﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GravitonClient
{
    [TestFixture]
    public class GravitonTest
    {
        [Test]
        public void Test_SpawnWell()
        {
            Game myGame = new Game(false);
            int num = myGame.StableWells.Count;
            myGame.SpawnWell();
            Assert.IsTrue(myGame.StableWells.Count == num + 1);
        }
        [Test]
        public void Test_SpawnOrb()
        {
            Game myGame = new Game(false);
            int num = myGame.Orbs.Count;
            myGame.SpawnOrb();
            Assert.IsTrue(myGame.Orbs.Count == num + 1);
        }
        [Test]
        public void Test_Load_Save()
        {
            Game game1 = new Game(false);
            List<int> colors = new List<int>();
            foreach (Orb orb in game1.Orbs)
            {
                colors.Add(orb.Color);
            }
            game1.Save("temp.txt");
            Game game2 = new Game(false);
            game2.Load("temp.txt");
            for (int i = 0; i < game2.Orbs.Count; i++)
            {
                Assert.IsTrue(colors[i] == game2.Orbs[i].Color);
            }
        }
        [Test]
        public void Test_KeyPressed()
        {
            Game myGame = new Game(false);
            myGame.KeyPressed('w');
            Assert.IsTrue(myGame.VerticalInput == 1);
            myGame.KeyPressed('s');
            Assert.IsTrue(myGame.VerticalInput == 0);
            myGame.KeyReleased('w');
            Assert.IsTrue(myGame.VerticalInput == -1);
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