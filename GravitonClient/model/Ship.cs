using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class Ship : GameObject
    {
        public Game ParentGame { get; set; }
        public double BoostFactor { get; set; }
        public double SpeedX { get; set; }
        public double SpeedY { get; set; }
        public List<int> Orbs { get; set; }
        public Ship(double xcoor, double ycoor, Game game)
        {
            ParentGame = game;
            Xcoor = xcoor;
            Ycoor = ycoor;
            SpeedX = 0.0;
            SpeedY = 0.0;
            BoostFactor = 1.0;
            Orbs = new List<int>();
        }

        public Ship() : base() { }

        public void Move(int xInput, int yInput)
        {
            SpeedX += 2.0 * xInput * BoostFactor;
            SpeedY += 2.0 * yInput * BoostFactor;
            if (BoostFactor > 1.0)
                BoostFactor -= 0.02;

            Xcoor += SpeedX;
            Ycoor += SpeedY;
            if (Xcoor < 0.0)
            {
                Xcoor = 0.0;
                SpeedX = 0.0;
            }
            else if (Xcoor > 5000.0)
            {
                Xcoor = 5000.0;
                SpeedX = 0.0;
            }
            if (Ycoor < 0.0)
            {
                Ycoor = 0.0;
                SpeedY = 0.0;
            }
            else if (Ycoor > 5000.0)
            {
                Ycoor = 5000.0;
                SpeedY = 0.0;
            }
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
                if (deltaX * deltaX + deltaY * deltaY < 1600)
                    return orb;
            }
            return null;
        }
        public Well WellOver()
        {
            foreach (Well well in ParentGame.StableWells.Concat(ParentGame.UnstableWells))
            {
                double deltaX = well.Xcoor - this.Xcoor;
                double deltaY = well.Ycoor - this.Ycoor;
                if (deltaX * deltaX + deltaY * deltaY < 1600)
                    return well;
            }
            return null;
        }

        //public void SortOrbs()
        //{
        //    Orbs.Sort((orb1, orb2) => orb1.Color < orb2.Color ? -1 : orb1.Color == orb2.Color ? 0 : 1);
        //}

        public bool DepositOrbs(Well well)
        {
            foreach (int orb in Orbs)
            {
                if (orb == well.Orbs)
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
