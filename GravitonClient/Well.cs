using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class Well
    {
        int secondsLeft;
        public int orbs;
        public double strength;
        public double xcoor;
        public double ycoor;

        public Well(double xcoor, double ycoor)
        {

        }

        public string Serialize()
        {
            return null;
        }

        public static Well Deserialize(string info)
        {
            return null;
        }
    }
}
