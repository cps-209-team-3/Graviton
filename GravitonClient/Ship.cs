using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class Ship : GameObject
    {
        public Game ParentGame { get; set; }
        public double Speed { get; set; }
        public List<Orb> Orbs { get; set; }
        public Ship(double xcoor, double ycoor, Game game)
        {
             
        }

        public Orb OrbOver()
        {
            return null;
        }
        public Well WellOver()
        {
            return null;
        }

        public bool DepositOrbs(StableWell well)
        {
            return false;
        }

        public override string Serialize()
        {
            return null;
        }

    }
}
