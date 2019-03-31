using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class StableWell : Well
    {
        public int TicksLeft { get; set; }
        public int Orbs { get; set; }
        public StableWell(double xcoor, double ycoor)
        {
            Xcoor = xcoor;
            Ycoor = ycoor;
            Orbs = 0;
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
