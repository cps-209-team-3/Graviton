using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class Shockwave : GameObject
    {
        private Well well;

        public Game ParentGame { get; set; }
        public Well ParentWell { get; set; }
        public int Radius { get; set; }
        public int TicksLeft { get; set; }

        public Shockwave(Game game, Well well)
        {
            ParentGame = game;
            ParentWell = well;
            Xcoor = well.Xcoor;
            Ycoor = well.Ycoor;
            Radius = 0;
            TicksLeft = 0;
        }

        public Shockwave(Well well)
        {
            this.well = well;
        }

        public void Pulse()
        {
            if (TicksLeft > 0)
            {
                Radius += 5;
                foreach (Orb orb in ParentGame.Orbs.ToList())
                {
                    if (Math.Pow(Xcoor - orb.Xcoor, 2) + Math.Pow(Ycoor - orb.Ycoor, 2) < Math.Pow(Radius, 2))
                    {
                        ParentGame.Orbs.Remove(orb);
                        ParentGame.GameObjects.Remove(orb);
                    }
                }
                --TicksLeft;
            }
            if (TicksLeft == 0)
                Radius = 0;
        }

        public override string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
