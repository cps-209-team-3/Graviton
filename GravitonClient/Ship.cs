using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class Ship : IPosition
    {
        public Game ParentGame { get; set; }
        public double Speed { get; set; }
        public double Xcoor { get; set; }
        public double Ycoor { get; set; }
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
