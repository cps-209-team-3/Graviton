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
        public double BoostFactor { get; set; }
        public double SpeedX { get; set; }
        public double SpeedY { get; set; }
        public List<Orb> Orbs { get; set; }
        public Ship(double xcoor, double ycoor, Game game)
        {
            ParentGame = game;
            Xcoor = xcoor;
            Ycoor = ycoor;
            SpeedX = 0.0;
            SpeedY = 0.0;
            BoostFactor = 1.0;
            Orbs = new List<Orb>();
        }

        public void Move(int xInput, int yInput)
        {
            SpeedX += xInput * BoostFactor;
            SpeedY += yInput * BoostFactor;
            Xcoor += SpeedX;
            Ycoor += SpeedY;
            if (BoostFactor > 1.0)
                BoostFactor -= 0.02;
        }

        public void SpeedBoost()
        {
            if (Orbs.Count > 0)
            {
                Orbs.RemoveAt(0);
                BoostFactor += 1.6;
            }
        }

        public Orb OrbOver()
        {
            foreach (Orb orb in ParentGame.Orbs)
            {
                double deltaX = orb.Xcoor - this.Xcoor;
                double deltaY = orb.Ycoor - this.Ycoor;
                if (deltaX * deltaX + deltaY * deltaY < 20)
                    return orb;
            }
            return null;
        }
        public StableWell StableWellOver()
        {
            foreach (StableWell well in ParentGame.StableWells)
            {
                double deltaX = well.Xcoor - this.Xcoor;
                double deltaY = well.Ycoor - this.Ycoor;
                if (deltaX * deltaX + deltaY * deltaY < 20)
                    return well;
            }
            return null;
        }
        public bool IsOverUnstable()
        {
            foreach (UnstableWell well in ParentGame.UnstableWells)
            {
                double deltaX = well.Xcoor - this.Xcoor;
                double deltaY = well.Ycoor - this.Ycoor;
                if (deltaX * deltaX + deltaY * deltaY < 20)
                    return true;
            }
            return false;
        }
        public void SortOrbs()
        {
            Orbs.Sort((orb1, orb2) => orb1.Color < orb2.Color ? -1 : orb1.Color == orb2.Color ? 0 : 1);
        }

        public bool DepositOrbs(StableWell well)
        {
            foreach (Orb orb in Orbs)
            {
                if (orb.Color == well.Orbs)
                {
                    well.Orbs++;
                    Orbs.Remove(orb);
                } 
            }
            return well.Orbs == 6;
        }

        public override string Serialize()
        {
            return null;
        }
        public override void Deserialize(string info)
        {
            // change the properties
        }

    }
}
