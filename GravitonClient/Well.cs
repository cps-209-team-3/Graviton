using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    abstract class Well
    {
        public double strength;
        public double xcoor;
        public double ycoor;

        public abstract string Serialize();

        public static Well Deserialize(string info)
        {
            return null;
        }
    }
}
