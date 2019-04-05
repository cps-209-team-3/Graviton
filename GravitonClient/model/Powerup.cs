using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class Powerup
    {
        public enum powerups { neutralize, destabilize, ghost };
        public List<powerups> CurrentPowerups;
        public Game ParentGame;

        public Powerup(Game game)
        {
            ParentGame = game;
            CurrentPowerups = new List<powerups>();
        }

        public void AddNew()
        {
            // add a random powerup to CurrentPowerups
        }

        public void Neutralize()
        {
            if (!CurrentPowerups.Contains(powerups.neutralize))
                return;
            CurrentPowerups.Remove(powerups.neutralize);
            // Do neutralization
        }

        public void Destabilize()
        {
            if (!CurrentPowerups.Contains(powerups.destabilize))
                return;
            CurrentPowerups.Remove(powerups.destabilize);
            // Do destabilization logic
        }

        public void Ghost()
        {
            if (!CurrentPowerups.Contains(powerups.ghost))
                return;
            CurrentPowerups.Remove(powerups.ghost);
            // Do ghost logic
        }
    }
}