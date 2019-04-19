using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class Well : GameObject
    {
        public bool IsStable { get; set; }
        public bool IsTrap { get; set; }
        public bool IsGhost { get; set; }
        public int TicksLeft { get; set; }
        public double Strength { get; set; }
        public int Orbs { get; set; }
        public Ship PlayerWhoSetTrap { get; set; }
        private Random rand;

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
