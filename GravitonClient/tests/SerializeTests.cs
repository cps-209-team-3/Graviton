using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace GravitonClient
{
    [TestFixture]
    public class SerializeTests
    {
        
        [Test]
        public void Test_Load_Save()
        {
            Game game1 = new Game(false);
            game1.Initialize();
            game1.Player.Xcoor = 23.17;
            game1.Player.Ycoor = 23.17;

            GameLoader.Save(game1,"\\temp\\temp.json");
            Game game2 = GameLoader.Load("\\temp\\temp.json", false);
            Assert.AreEqual(game1.Orbs.Count, game2.Orbs.Count);
            for (int i = 0; i < game2.Orbs.Count; i++)
            {
                Assert.AreEqual(game1.Orbs[i].Color, game2.Orbs[i].Color);
                Assert.AreEqual(game1.Orbs[i].Xcoor, game2.Orbs[i].Xcoor, 0.5);
                Assert.AreEqual(game1.Orbs[i].Ycoor, game2.Orbs[i].Ycoor, 0.5);
            }

            for (int i = 0; i < game2.StableWells.Count; i++)
            {
                Assert.AreEqual(game1.StableWells[i].Strength, game2.StableWells[i].Strength, 0.5);
                Assert.AreEqual(game1.StableWells[i].Xcoor, game2.StableWells[i].Xcoor, 0.5);
                Assert.AreEqual(game1.StableWells[i].Ycoor, game2.StableWells[i].Ycoor, 0.5);
                Assert.AreEqual(game1.StableWells[i].TicksLeft, game2.StableWells[i].TicksLeft);
            }
            for (int i = 0; i < game2.UnstableWells.Count; i++)
            {
                Assert.AreEqual(game1.UnstableWells[i].Strength, game2.UnstableWells[i].Strength, 0.5);
                Assert.AreEqual(game1.UnstableWells[i].Xcoor, game2.UnstableWells[i].Xcoor, 0.5);
                Assert.AreEqual(game1.UnstableWells[i].Ycoor, game2.UnstableWells[i].Ycoor, 0.5);
                Assert.IsTrue( game2.UnstableWells[i].IsStable);
                Assert.AreEqual(game1.UnstableWells[i].TicksLeft, game2.UnstableWells[i].TicksLeft);
            }
            Assert.IsTrue(Enumerable.SequenceEqual(game1.Player.Orbs, game2.Player.Orbs));
            Assert.AreEqual(game1.Player.Points, game2.Player.Points);
            Assert.AreEqual(game1.Player.Xcoor, game2.Player.Xcoor, 0.5);
            Assert.AreEqual(game1.Player.Ycoor, game2.Player.Ycoor, 0.5);

            File.Delete("\\temp\\temp.json");
        }
        [Test]
        public void Test_OrbSerialize()
        {
            var orb1 = new Orb(1, 2, 3);
            string s = orb1.Serialize();
            var orb2 = GameObject.FromJsonFactory<Orb>(s);
            Assert.AreEqual(orb1.Xcoor, orb2.Xcoor);
            Assert.AreEqual(orb1.Ycoor, orb2.Ycoor);
            Assert.AreEqual(orb1.Color, orb2.Color);
        }

        [Test]
        public void Test_StableWellSerialize()
        {
            var orb1 = new Well(1, 2);
            orb1.IsStable = true;
            orb1.Orbs = 2;
            string s = orb1.Serialize();
            var orb2 = GameObject.FromJsonFactory<Well>(s);
            Assert.AreEqual(orb1.Xcoor, orb2.Xcoor);
            Assert.AreEqual(orb1.Ycoor, orb2.Ycoor);
            Assert.AreEqual(orb1.Strength, orb2.Strength);
            Assert.AreEqual(orb1.Orbs, orb2.Orbs);
        }

        [Test]
        public void Test_UnstableWellSerialize()
        {
            var orb1 = new Well(1, 2);
            orb1.IsStable = false;
            string s = orb1.Serialize();
            var orb2 = GameObject.FromJsonFactory<Well>(s);
            Assert.AreEqual(orb1.Xcoor, orb2.Xcoor);
            Assert.AreEqual(orb1.Ycoor, orb2.Ycoor);
            Assert.AreEqual(orb1.Strength, orb2.Strength);
        }

        [Test]
        public void Test_ShipSerialize()
        {
            var ship1 = new Ship(1, 2, new Game(false));
            ship1.Orbs = new List<int>() { 5, 3, 4 };
            string s = ship1.Serialize();
            var orb2 = GameObject.FromJsonFactory<Ship>(s);
            Assert.AreEqual(ship1.Xcoor, orb2.Xcoor);
            Assert.AreEqual(ship1.Ycoor, orb2.Ycoor);
        }
    }
}
