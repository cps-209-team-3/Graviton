using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace GravitonClient
{
    [TestFixture]
    public class JsonTests
    {
        [Test]
        public void Test_FromJsonArray()
        {
            string s = @"[
1,
{2:[]},
""3""]";
            var l = JsonUtils.GetObjectsInArray(s);

            Assert.AreEqual(l[0], "1");
            Assert.AreEqual(l[1], "{2:[]}");
            Assert.AreEqual(l[2], "\"3\"");
        }
        [Test]
        public void Test_ToJsonArray()
        {
            var orbs = new Game(false).Orbs;
            string st = JsonUtils.ToJsonList(orbs);
            var s = JsonUtils.GetObjectsInArray(st);
            Assert.AreEqual(s.Count, orbs.Count);
            for(int i = 0; i < orbs.Count; i++)
            {
                Orb other = GameObject.FromJsonFactory<Orb>(s[i]);
                Assert.AreEqual(other.Xcoor, orbs[i].Xcoor, 0.5);
                Assert.AreEqual((int)other.Ycoor, (int)orbs[i].Ycoor, 0.5);
                Assert.AreEqual(other.Color, orbs[i].Color);
            }
        }

        
    }
}
