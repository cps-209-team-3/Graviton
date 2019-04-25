//This file contains the Orb class, which represents energy orbs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonServer
{
    public class Orb : GameObject
    {
        //Orb type indicator
        public int Color { get; set; }
        //Constructor
        public Orb(double xcoor, double ycoor, int color) : base(xcoor, ycoor)
        { 
            Color = color;
            Type = "Orb";
        }
        

    }
}
