using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class Ship
    {
        public double xcoor;
        public double ycoor;
        public Orb[] orbs;
        public Ship(double xcoor, double ycoor)
        {

        }

        public string Serialize()
        {
            return null;
        }

        public static Ship Deserialize(string info)
        {
            return null;
        }
    }
}
