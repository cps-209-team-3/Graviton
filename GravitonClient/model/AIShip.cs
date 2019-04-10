using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class AIShip : Ship
    {
        public Well TargetWell { get; set; }
        public Orb TargetOrb { get; set; }
        public double TargetX { get; set; }
        public double TargetY { get; set; }
        public double TargetDist { get; set; }
        public int XMove { get; set; }
        public int YMove { get; set; }

        //Constructor
        public AIShip(double xc, double yc, Game game) : base(xc, yc, game)
        {
            XMove = 0;
            YMove = 0;
            TargetNearestOrb();
            TargetNearestWell();
            SetTargetPos();
        }

        //Sets TargetWell to the nearest well
        public void TargetNearestWell()
        {
            Well closestWell;
            double xDist = ParentGame.StableWells[0].Xcoor - this.Xcoor;
            double yDist = ParentGame.StableWells[0].Ycoor - this.Ycoor;
            double dist = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
            double compareDist = dist;

            foreach (Well well in ParentGame.StableWells)
            {
                xDist = well.Xcoor - this.Xcoor;
                yDist = well.Ycoor - this.Ycoor;
                dist = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
                if(dist < compareDist)
                {
                    closestWell = well;
                }
            }
        }

        //Sets TargetOrb to the nearest orb
        public void TargetNearestOrb()
        {
            Orb closestOrb;
            double xDist = ParentGame.Orbs[0].Xcoor - this.Xcoor;
            double yDist = ParentGame.Orbs[0].Ycoor - this.Ycoor;
            double dist = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
            double compareDist = dist;

            foreach (Orb orb in ParentGame.Orbs)
            {
                xDist = orb.Xcoor - this.Xcoor;
                yDist = orb.Ycoor - this.Ycoor;
                dist = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
                if (dist < compareDist)
                {
                    closestOrb = orb;
                }
            }
        }

        //Sets TargetDist to dist from current target
        public void FindTargetDist()
        {
            double xDist = TargetX - this.Xcoor;
            double yDist = TargetX - this.Ycoor;
            TargetDist = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
        }

        //Sets TargetX and TargetY to Xcoor and Ycoor of target well or orb
        public void SetTargetPos()
        {
            if(Orbs.Count < 3)
            {
                TargetNearestOrb();
                TargetX = TargetOrb.Xcoor;
                TargetY = TargetOrb.Ycoor;
            }
            else
            {
                TargetNearestWell();
                TargetX = TargetWell.Xcoor;
                TargetY = TargetWell.Ycoor;
            }
            FindTargetDist();
        }

        public bool IsCloser()
        {
            double xDist = TargetX - this.Xcoor;
            double yDist = TargetX - this.Ycoor;
            double currentDist = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
            if (currentDist < TargetDist)
                return true;
            else
                return false;
        }

        //Sets XMove and YMove to determine movement direction
        public void SetMoveDir()
        {
            XMove = Math.Sign(TargetX - Xcoor);
            YMove = Math.Sign(TargetY - Ycoor);
        }

        //Moves the AI, setting Xcoor and Ycoor
        public void AIMove()
        {
            SetMoveDir();
            Move(XMove, YMove);
        }    
    }
}
