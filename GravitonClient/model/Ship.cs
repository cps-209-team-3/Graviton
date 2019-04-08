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
        public Powerup GamePowerup { get; set; }
        public double BoostFactor { get; set; }
        public double SpeedX { get; set; }
        public double SpeedY { get; set; }
        public List<int> Orbs { get; set; }
        public int Points{ get; set; }

        private Random rand;

        public Ship(double xcoor, double ycoor, Game game)
        {
            ParentGame = game;
            GamePowerup = new Powerup(game);
            Xcoor = xcoor;
            Ycoor = ycoor;
            SpeedX = 0.0;
            SpeedY = 0.0;
            BoostFactor = 1.0;
            Orbs = new List<int>();
        }

        public Ship() : base() {
            SpeedX = 0.0;
            SpeedY = 0.0;
            BoostFactor = 1.0;
            Orbs = new List<int>();
        }

        //This method moves the ship according to its inputs and speeds.
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

        //This method gives the ship a speed boost.
        public void SpeedBoost()
        {
            if (Orbs.Count >= 3)
            {
                for(int i = 0; i < 3; ++i)
                {
                    int orbIndex = rand.Next(Orbs.Count);
                    Orbs.RemoveAt(orbIndex);
                }
                BoostFactor += 1.6;
            }
        }

        //This method returns the Orb that the ship is over (or null if it is not over any).
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

        //This method returns the Well that the ship is over (or null if it is not over any).
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


        //This method deposits all of the orbs it can into a given well.
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
            return $@"{{
    ""xcoor"":{Xcoor},
    ""ycoor"":{Ycoor},
    ""points"":{Points},
    ""orblist"":{JsonUtils.ToJsonList(Orbs)},
    ""powerups"":{JsonUtils.ToJsonList( GamePowerup.CurrentPowerups)}
}}";

        }



        public override void Deserialize(string info)
        {
            
            base.Deserialize(info);
            Points = Convert.ToInt32(JsonUtils.ExtractValue(info, "points"));
            
            foreach (string s in JsonUtils.GetObjectsInArray(JsonUtils.ExtractValue(info, "orblist")))
            {
                Orbs.Add(Convert.ToInt32(s));
            }

            var strs = JsonUtils.GetObjectsInArray(JsonUtils.ExtractValue(info, "powerups"));
            for (int i = 0; i < Math.Min(strs.Count, 3); ++i)
            {
                switch (strs[i])
                {
                    case "ghost": GamePowerup.CurrentPowerups.Add(Powerup.powerups.ghost); break;
                    case "destabilize": GamePowerup.CurrentPowerups.Add(Powerup.powerups.destabilize); break;
                    case "neutralize": GamePowerup.CurrentPowerups.Add(Powerup.powerups.neutralize); break;
                }
            }
        }
    }
}
