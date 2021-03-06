﻿//This file contains the Ship class which represents an ship in the game.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    //This class describes a ship in the game. The ship can move, 
    //do a speed boost, return the well/orb it is over, ...
    public class Ship : GameObject
    {
        //This is a reference to the parent game
        public Game ParentGame { get; set; }
        //This is a reference to a powerup instance (which will apply powerups to the game)
        public Powerup GamePowerup { get; set; }
        //These are the speeds of the ship
        public double SpeedX { get; set; }
        public double SpeedY { get; set; }
        //This scales the ship's input (i.e. it can go faster) according to whether boost is enabled
        public double BoostFactor { get; set; }
        //This is a list of orbs that the ship has
        public List<int> Orbs { get; set; }
        //This is how many points the ship has
        public int Points{ get; set; }
        //This is a bool whether the ship is immune to destabilized wells
        public bool IsImmune { get; set; }
        //This describes how much longer the ship will be immune
        public int ImmuneTicksLeft { get; set; }
        //This is a Random object
        private Random rand = new Random();

        public event EventHandler<SoundEffect> GameInvokeSoundEvent;

        public Ship(double xcoor, double ycoor, Game game)
        {
            ParentGame = game;
            GamePowerup = new Powerup(game);
            IsImmune = false;
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
            SpeedX += 0.3 * xInput * BoostFactor;
            SpeedY += 0.3 * yInput * BoostFactor;
            SpeedX *= 0.998;
            SpeedY *= 0.998;
            if (BoostFactor > 1.0)
                BoostFactor -= 0.01;

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
                if (ParentGame.Player == this)
                    GameInvokeSoundEvent(this, SoundEffect.Boost);
                for (int i = 0; i < 3; ++i)
                {
                    int orbIndex = rand.Next(Orbs.Count);
                    Orbs.RemoveAt(orbIndex);
                }
                BoostFactor += 2.0;

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
                if (deltaX * deltaX + deltaY * deltaY < 2500)
                    return well;
            }
            return null;
        }


        //This method deposits all of the orbs it can into a given well.
        public bool DepositOrbs(Well well)
        {
            bool completed = false;
            foreach (int orb in Orbs.ToList())
            {
                if (orb == well.Orbs && (!well.IsGhost || this == well.Owner))
                {
                    if (well.Orbs == 5)
                        completed = true;
                    well.Orbs++;
                    IncrementScore();
                    Orbs.Remove(orb);
                    well.TicksLeft = ParentGame.WellDestabFreq;
                }
            }
            if (well.Orbs == 6)
                well.Orbs = 5;
            return completed;
        }

        //This method increments the ParentGame's points.
        public virtual void IncrementScore()
        {
            ParentGame.Points += 10;
        }


        //This method serializes the instance and returns the string.
        public override string Serialize()
        {
            return $@"{{
    ""xcoor"":{Xcoor},
    ""ycoor"":{Ycoor},
    ""points"":{ParentGame.Points},
    ""orblist"":{JsonUtils.ToJsonList(Orbs)},
    ""powerups"":{JsonUtils.ToJsonList( GamePowerup.CurrentPowerups)},
}}";

        }


        //This method deserializes a string and puts the information into the instance itself.
        public override void Deserialize(string info)
        {
            
            base.Deserialize(info);
            Points = Convert.ToInt32(JsonUtils.ExtractValue(info, "points"));
            
            foreach (string s in JsonUtils.GetObjectsInArray(JsonUtils.ExtractValue(info, "orblist")))
            {
                Orbs.Add(Convert.ToInt32(s));
            }
            GamePowerup = new Powerup();
            var strs = JsonUtils.GetObjectsInArray(JsonUtils.ExtractValue(info, "powerups"));
            for (int i = 0; i < Math.Min(strs.Count, 3); ++i)
            {
                switch (strs[i])
                {
                    case "ghost":
                        GamePowerup.CurrentPowerups.Add(Powerup.powerups.ghost);
                        GamePowerup.CarryingGhost = true;
                        break;
                    case "destabilize":
                        GamePowerup.CurrentPowerups.Add(Powerup.powerups.destabilize);
                        GamePowerup.CarryingDestabilize = true;
                        break;
                    case "neutralize":
                        GamePowerup.CurrentPowerups.Add(Powerup.powerups.neutralize);
                        GamePowerup.CarryingNeutralize = true;
                        break;
                }
            }
        }
    }
}
