using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class Powerup
    {
        public enum powerups { neutralize, destabilize, ghost };
        public List<powerups> CurrentPowerups;
        public Game ParentGame;

        private Random rand;

        public Powerup(Game game)
        {
            ParentGame = game;
            CurrentPowerups = new List<powerups>();
        }

        //This method adds a random powerup to the powerup list.
        public void AddNew()
        {
            int powerup = rand.Next(3);
            switch (powerup)
            {
                case 0:
                    CurrentPowerups.Add(powerups.neutralize);
                    break;
                case 1:
                    CurrentPowerups.Add(powerups.destabilize);
                    break;
                case 2:
                    CurrentPowerups.Add(powerups.ghost);
                    break;
            }
        }


        //This method does a neutralize powerup
        public void Neutralize()
        {
            if (!CurrentPowerups.Contains(powerups.neutralize))
                return;
            CurrentPowerups.Remove(powerups.neutralize);
            // TODO: neutralization
            Well well = ParentGame.Player.WellOver();
            if(well != null)
            {
                //execute neutralize
            }
        }

        //This method does a destabilize powerup
        public void Destabilize()
        {
            if (!CurrentPowerups.Contains(powerups.destabilize))
                return;
            CurrentPowerups.Remove(powerups.destabilize);
            // TODO: destabilization logic
            Well well = ParentGame.Player.WellOver();
            if (well != null)
            {
                //execute destabilize
            }
        }

        //This method does a ghost powerup
        public void Ghost()
        {
            if (!CurrentPowerups.Contains(powerups.ghost))
                return;
            CurrentPowerups.Remove(powerups.ghost);
            // TODO: ghost logic
            Well well = ParentGame.Player.WellOver();
            if (well != null)
            {
                //execute ghost
            }
        }
    }
}