using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonServer
{
    public class Orb : GameObject
    {
        public int Color { get; set; }
        public Orb(double xcoor, double ycoor, int color) : base(xcoor, ycoor)
        { 
            Color = color;
            Type = "Orb";
        }
        

    }
}
