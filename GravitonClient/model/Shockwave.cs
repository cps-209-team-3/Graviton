//This file contains the Shockwave emitter class which handles shockwaves emitted by a black hole.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    //This class describes the shockwave generator.
    public class Shockwave : GameObject
    {
        //Shockwave's parent game
        public Game ParentGame { get; set; }
        //Shockwave's parent well
        public Well ParentWell { get; set; }
        //Shockwave radius
        public int Radius { get; set; }
        //Ticks left until shockwave radius is reset
        public int TicksLeft { get; set; }

        //constructor
        public Shockwave(Game game, Well well)
        {
            ParentGame = game;
            ParentWell = well;
            Xcoor = well.Xcoor;
            Ycoor = well.Ycoor;
            Radius = 0;
            TicksLeft = 0;
        }

        //alternate constructor for loading
        public Shockwave(Well well)
        {
            ParentWell = well;
            Xcoor = well.Xcoor;
            Ycoor = well.Ycoor;
        }

        //Destroys all orbs in shockwave radius; increments radius; if ticksleft == 0, resets radius
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

        //not implemented.  Shockwaves do not need to be saved, since they are attached to wells
        public override string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
