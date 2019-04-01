using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class Orb : GameObject
    {
        public int Color { get; set; }
        public Orb(double xcoor, double ycoor, int color)
        {
            Xcoor = xcoor;
            Ycoor = ycoor;
            Color = color;
        }

        public override string Serialize()
        {
            return null;
        }
        public override void Deserialize(string info)
        {
            // change the properties
        }
    }
}
