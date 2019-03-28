using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class Ship
    {
        public Game game;
        public double speed;
        public double xcoor;
        public double ycoor;
        public List<Orb> orbs;
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
