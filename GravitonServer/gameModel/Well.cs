//This file contains the Well class which represents a gravity well in the game.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonServer
{
    //This class describes a gravity well in the game.
    public class Well : GameObject
    {
        //This is a bool whether the well is stable or not
        public bool IsStable { get; set; }
        //This is a bool whether the well is a trap
        public bool IsTrap { get; set; }
        //This is a bool whether the well is ghosted
        public bool IsGhost { get; set; }
        //How many ticks left until destabilization or an unstable well disappearing
        public int TicksLeft { get; set; }
        //How strong a well's gravity is
        public double Strength { get; set; }
        //How many orbs the well currently has
        public int Orbs { get; set; }
        //A reference to the owner ship
        public Ship PlayerWhoSetTrap { get; set; }
        //A Random object
        private Random rand;

        //Constructor
        public Well(double xcoor, double ycoor) : base(xcoor, ycoor)
        {
            rand = new Random();
            IsStable = true;
            IsTrap = false;
            IsGhost = false;
            Strength = 300;
            TicksLeft = rand.Next(1500,4001);
            Orbs = 0;
        }

        
        
    }
}
