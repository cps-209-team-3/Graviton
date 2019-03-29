using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class Orb : IPosition
    {
        public int Color { get; set; }
        public double Xcoor { get; set; }
        public double Ycoor { get; set; }
        public Orb(double xcoor, double ycoor, int color)
        {

        }

        public string Serialize()
        {
            return null;
        }

        public Orb Deserialize(string info)
        {
            return null;
        }
    }
}
