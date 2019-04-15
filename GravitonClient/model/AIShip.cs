﻿using System;
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
            TargetOrb = ParentGame.Orbs[0];
            TargetWell = ParentGame.StableWells[0];
            TargetNearestOrb();
            TargetNearestWell();
            SetTargetPos();
        }

        public AIShip() { }

        //Sets TargetWell to the nearest well
        public void TargetNearestWell()
        {
            Well closestWell = ParentGame.StableWells[0];
            double xDist;
            double yDist;
            double dist;
            double compareDist = 5000 * Math.Sqrt(2);

            foreach (Well well in ParentGame.StableWells)
            {
                xDist = well.Xcoor - this.Xcoor;
                yDist = well.Ycoor - this.Ycoor;
                dist = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
                if(dist < compareDist)
                {
                    if (well != TargetWell)
                    {
                        closestWell = well;
                        compareDist = dist;
                    }
                }
            }
            TargetWell = closestWell;
        }

        //Sets TargetOrb to the nearest orb
        public void TargetNearestOrb()
        {
            Orb closestOrb = ParentGame.Orbs[0];
            double xDist;
            double yDist;
            double dist;
            double compareDist = 5000 * Math.Sqrt(2);

            foreach (Orb orb in ParentGame.Orbs)
            {
                xDist = orb.Xcoor - this.Xcoor;
                yDist = orb.Ycoor - this.Ycoor;
                dist = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
                if (dist < compareDist)
                {
                    if (orb != TargetOrb)
                    {
                        closestOrb = orb;
                        compareDist = dist;
                    }
                }
            }
            TargetOrb = closestOrb;
        }

        //Sets TargetDist to dist from current target
        public void FindTargetDist()
        {
            double xDist = TargetX - this.Xcoor;
            double yDist = TargetY - this.Ycoor;
            TargetDist = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
        }

        //Sets TargetX and TargetY to Xcoor and Ycoor of target well or orb
        public void SetTargetPos()
        {
            TargetNearestOrb();
            TargetNearestWell();
            if (Orbs.Count < 3)
            {
                TargetX = TargetOrb.Xcoor;
                TargetY = TargetOrb.Ycoor;
            }
            else if (Orbs.Count < 5)
            {
                TargetX = TargetWell.Xcoor;
                TargetY = TargetWell.Ycoor;
            }
            else if (Orbs.Count == 5)
            {
                SpeedBoost();
            }
            FindTargetDist();
        }

        public bool IsCloser()
        {
            double xDist = TargetX - this.Xcoor;
            double yDist = TargetY - this.Ycoor;
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
            SetTargetPos();
            SetMoveDir();
            Move(XMove, YMove);
        }

        public override void IncrementScore()
        {
        }
    }
}
