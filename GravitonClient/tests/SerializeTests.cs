using System.Collections.Generic;
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
            List<int> colors = new List<int>();
            foreach (Orb orb in game1.Orbs)
            {
                colors.Add(orb.Color);
            }

            GameLoader.Save(game1,"temp.json");
            Game game2 = GameLoader.Load("temp.json");
            for (int i = 0; i < game2.Orbs.Count; i++)
            {
                Assert.IsTrue(colors[i] == game2.Orbs[i].Color);
            }
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
            var orb1 = new (1, 2, 3);
            string s = orb1.Serialize();
            var orb2 = GameObject.FromJsonFactory<Orb>(s);
            Assert.AreEqual(orb1.Xcoor, orb2.Xcoor);
            Assert.AreEqual(orb1.Ycoor, orb2.Ycoor);
            Assert.AreEqual(orb1.Color, orb2.Color);
        }
    }
}
