﻿//This file contains AIShip, which inherits from Ship and determines AI activity.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonServer
{
    public class AIShip : Ship
    {
        //AI destination orb and well
        public Well TargetWell { get; set; }
        public Orb TargetOrb { get; set; }
        //AI destination x and y coordinates
        public double TargetX { get; set; }
        public double TargetY { get; set; }

        //Constructor
        public AIShip(double xc, double yc, Game game) : base(xc, yc, game)
        {
            HorizontalInput = 0;
            VerticalInput = 0;
            TargetOrb = ParentGame.Orbs[0];
            TargetWell = ParentGame.StableWells[0];
            TargetNearestOrb();
            TargetNearestWell();
            SetTargetPos();
        }

        //Alternate constructor
        public AIShip() { }

        //Sets TargetWell to the nearest well that is requesting a color orb currently being carried
        public void TargetNearestWell()
        {
            Well closestWell = null;
            double xDist;
            double yDist;
            double dist;
            double compareDist = 5000 * Math.Sqrt(2);

            foreach (Well well in ParentGame.StableWells)
            {
                xDist = well.Xcoor - this.Xcoor;
                yDist = well.Ycoor - this.Ycoor;
                dist = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
                if (dist < compareDist)
                {
                    if (Orbs.Contains(well.Orbs) && !well.IsGhost)
                    {
                        closestWell = well;
                        compareDist = dist;
                    }     
                }
            }
            if (closestWell == null)
                Orbs.Clear();
            else
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
                    closestOrb = orb;
                    compareDist = dist;
                }
            }
            TargetOrb = closestOrb;
        }

        //Sets TargetX and TargetY to Xcoor and Ycoor of target well or orb
        public void SetTargetPos()
        {
            if (Orbs.Count < 3)
            {
                TargetNearestOrb();
                TargetX = TargetOrb.Xcoor;
                TargetY = TargetOrb.Ycoor;
            }
            else if (Orbs.Count < 5)
            {
                TargetNearestWell();
                TargetX = TargetWell.Xcoor;
                TargetY = TargetWell.Ycoor;
            }
            else if (Orbs.Count == 5)
            {
                SpeedBoost();
            }
        }

        //Sets XMove and YMove to determine movement direction
        public void SetMoveDir()
        {
            HorizontalInput = Math.Sign(TargetX - Xcoor);
            VerticalInput = Math.Sign(TargetY - Ycoor);
        }

        //Moves the AI, setting Xcoor and Ycoor
        public void AIMove()
        {
            SetTargetPos();
            SetMoveDir();
            Move();
        }

        //Uses destabilize powerup if conditions are right
        public void UseDestabilize()
        {
            foreach (Well well in ParentGame.UnstableWells.ToList())
            {
                if (Math.Pow(Xcoor - well.Xcoor, 2) + Math.Pow(Ycoor - well.Ycoor, 2) < 30000)
                {
                    GamePowerup.Destabilize();
                }
            }
        }

        //uses neutralize powerup if conditions are right
        public void UseNeutralize()
        {
            foreach (Well well in ParentGame.StableWells.ToList())
            {
                if (Math.Pow(Xcoor - well.Xcoor, 2) + Math.Pow(Ycoor - well.Ycoor, 2) < 30000)
                {
                    GamePowerup.Neutralize();
                }
            }
        }

        //uses ghost powerup if conditions are right
        public void UseGhost()
        {
            foreach (Well well in ParentGame.UnstableWells.ToList())
            {
                if (Math.Pow(Xcoor - well.Xcoor, 2) + Math.Pow(Ycoor - well.Ycoor, 2) < 30000)
                {
                    GamePowerup.Ghost();
                }
            }
        }

        //override method to prevent ai from gaining score
        public override void IncrementScore()
        {

        }
    }
}
