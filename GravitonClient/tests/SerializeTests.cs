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
            game1.Save("temp.txt");
            Game game2 = new Game(false);
            game2.Load("temp.txt");
            for (int i = 0; i < game2.Orbs.Count; i++)
            {
                Assert.IsTrue(colors[i] == game2.Orbs[i].Color);
            }
        }

    }
}
