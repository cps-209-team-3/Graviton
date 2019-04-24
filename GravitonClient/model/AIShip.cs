using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class AIShip : Ship
    {
        //Reference to AI destination well
        public Well TargetWell { get; set; }
        //Reference to AI destination orb
        public Orb TargetOrb { get; set; }
        //Destination x coordinate
        public double TargetX { get; set; }
        //Destination y coordinate
        public double TargetY { get; set; }
        //Vertical and Horizontal thrust indicators
        public int XMove { get; set; }
        public int YMove { get; set; }

        //Constructor
        public AIShip(double xc, double yc, Game game) : base(xc, yc, game)
        {
            XMove = 0;
            YMove = 0;
            InitializeTargets();
        }

        //Alternate constructor for loading
        public AIShip() { }

        //sets initial target values
        public void InitializeTargets()
        {
            TargetOrb = ParentGame.Orbs[0];
            TargetWell = ParentGame.StableWells[0];
            TargetNearestOrb();
            TargetNearestWell();
            SetTargetPos();
        }

        //Sets TargetWell to the nearest well requesting an orb that the AI is carrying
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
            {
                Orbs.Clear();
                SetTargetPos();
            }
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

        //AI uses the destabilize powerup if a well is in range
        public void UseDestabilize()
        {
            if (GamePowerup.CarryingDestabilize)
            {
                foreach (Well well in ParentGame.UnstableWells.ToList())
                {
                    if (Math.Pow(Xcoor - well.Xcoor, 2) + Math.Pow(Ycoor - well.Ycoor, 2) < 30000)
                    {
                        GamePowerup.Destabilize(this);
                    }
                }
            }
        }

        //AI uses neutralize powerup if a well is in range
        public void UseNeutralize()
        {
            if (GamePowerup.CarryingNeutralize)
            {
                foreach (Well well in ParentGame.StableWells.ToList())
                {
                    if (Math.Pow(Xcoor - well.Xcoor, 2) + Math.Pow(Ycoor - well.Ycoor, 2) < 30000)
                    {
                        GamePowerup.Neutralize(this);
                    }
                }
            }
        }

        //Uses ghost powerup to become immune if a destabilized well is in range,
        // or to take control of a stable well if in range
        public void UseGhost()
        {
            if (GamePowerup.CarryingGhost)
            {
                foreach (Well well in ParentGame.UnstableWells.ToList())
                {
                    if (Math.Pow(Xcoor - well.Xcoor, 2) + Math.Pow(Ycoor - well.Ycoor, 2) < 30000)
                    {
                        GamePowerup.Ghost(this);
                    }
                }
                foreach (Well well in ParentGame.StableWells.ToList())
                {
                    if (Math.Pow(Xcoor - well.Xcoor, 2) + Math.Pow(Ycoor - well.Ycoor, 2) < 30000)
                    {
                        GamePowerup.Ghost(this);
                    }
                }
            }
        }

        //overrides ship IncrementScore() method to prevent AI from scoring when depositing orbs
        public override void IncrementScore()
        {
        }
    }
}
