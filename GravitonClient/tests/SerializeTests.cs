using System.Collections.Generic;
using System.IO;
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
            GameLoader.Save(game1,"\\temp\\temp.json");
            Game game2 = GameLoader.Load("\\temp\\temp.json", false);

            for (int i = 0; i < game2.Orbs.Count; i++)
            {
                Assert.AreEqual(game1.Orbs[i].Color, game2.Orbs[i].Color);
            }
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
            string s = orb1.Serialize();
            var orb2 = GameObject.FromJsonFactory<Well>(s);
            Assert.AreEqual(orb1.Xcoor, orb2.Xcoor);
            Assert.AreEqual(orb1.Ycoor, orb2.Ycoor);
            Assert.AreEqual(orb1.Strength, orb2.Strength);
        }

        [Test]
        public void Test_UnstableWellSerialize()
        {
            var orb1 = new Well(1, 2);

            string s = orb1.Serialize();
            var orb2 = GameObject.FromJsonFactory<Well>(s);
            Assert.AreEqual(orb1.Xcoor, orb2.Xcoor);
            Assert.AreEqual(orb1.Ycoor, orb2.Ycoor);
        }

        [Test]
        public void Test_ShipSerialize()
        {
            var orb1 = new Well(1, 2);
            string s = orb1.Serialize();
            var orb2 = GameObject.FromJsonFactory<Well>(s);
            Assert.AreEqual(orb1.Xcoor, orb2.Xcoor);
            Assert.AreEqual(orb1.Ycoor, orb2.Ycoor);
        }
    }
}
