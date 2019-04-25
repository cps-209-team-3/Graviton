//This file contains Powerup, which is used to handle powerups for ships
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonServer
{
    public class Powerup
    {
        //Powerup enum for saving purposes
        public enum powerups { neutralize, destabilize, ghost}
        public List<powerups> CurrentPowerups;
        //Parent Game
        public Game ParentGame;
        //Bools used to limit powerup inventory to 1 each
        public bool CarryingNeutralize { get; set; }
        public bool CarryingDestabilize { get; set; }
        public bool CarryingGhost { get; set; }
        //Player object
        private Ship player
        {
            get; set;
        }
        //Random used for selection
        private Random rand = new Random();

        //Constructor
        public Powerup(Ship player)
        {
            this.player = player;
            ParentGame = player.ParentGame;
            CarryingNeutralize = false;
            CarryingDestabilize = false;
            CarryingGhost = false;
            CurrentPowerups = new List<powerups>();
        }


        //Constructor
        public Powerup()
        {
            CarryingNeutralize = false;
            CarryingDestabilize = false;
            CarryingGhost = false;
            CurrentPowerups = new List<powerups>();
        }

        //This method adds a random powerup to the powerup list.
        public void AddNew()
        {
            bool powerupAdded = false;
            while (powerupAdded == false)
            {
                if (CarryingDestabilize && CarryingNeutralize && CarryingGhost)
                {
                    powerupAdded = true;
                }
                else
                {
                    int powerup = rand.Next(3);
                    switch (powerup)
                    {
                        case 0:
                            if (!CarryingNeutralize)
                            {
                                CurrentPowerups.Add(powerups.neutralize);
                                CarryingNeutralize = true;
                                powerupAdded = true;
                            }
                            break;
                        case 1:
                            if (!CarryingDestabilize)
                            {
                                CurrentPowerups.Add(powerups.destabilize);
 
                                CarryingDestabilize = true;
                                powerupAdded = true;
                            }
                            break;
                        case 2:
                            if (!CarryingGhost)
                            {
                                CurrentPowerups.Add(powerups.ghost);
       
                                CarryingGhost = true;
                                powerupAdded = true;
                            }
                            break;
                    }
                }
            }
        }


        //This method neutralizes a nearby destabilized well
        public void Neutralize()
        {
            if (!CarryingNeutralize)
                return;
            foreach (Well well in ParentGame.UnstableWells.ToList())
            {
                if (Math.Pow(player.Xcoor - well.Xcoor, 2) + Math.Pow(player.Ycoor - well.Ycoor, 2) < 40000)
                {
                    ParentGame.UnstableWells.Remove(well);
                    ParentGame.GameObjects.Remove(well);
                    CarryingNeutralize = false;
                    CurrentPowerups.Remove(powerups.neutralize);
                }
            }
        }

        //This method destabilizes a nearby well
        public void Destabilize()
        {
            if (!CarryingDestabilize)
                return;
            foreach (Well well in ParentGame.StableWells.ToList())
            {
                if (Math.Pow(player.Xcoor - well.Xcoor, 2) + Math.Pow(player.Ycoor - well.Ycoor, 2) < 40000)
                {
                    CarryingDestabilize = false;
                    CurrentPowerups.Remove(powerups.destabilize);
                    well.TicksLeft = 3000;
                    well.IsStable = false;
                    well.Strength = 50;
                    well.IsTrap = true;
                    well.PlayerWhoSetTrap = player;
                    ParentGame.UnstableWells.Add(well);
                    ParentGame.StableWells.Remove(well);
                    player.IsImmune = true;
                    player.ImmuneTicksLeft = 50;
                }
            }
        }

        //This method locks AI from depositing in a nearby well.
        public void Ghost()
        {
            if (!CarryingGhost)
                return;
            foreach (Well well in ParentGame.StableWells.ToList())
            {
                if (Math.Pow(player.Xcoor - well.Xcoor, 2) + Math.Pow(player.Ycoor - well.Ycoor, 2) < 40000)
                {
                    CarryingGhost = false;
                    CurrentPowerups.Remove(powerups.ghost);

                    well.IsGhost = true;
                    player.IsImmune = true;
                    player.ImmuneTicksLeft = 150;
                }
            }
        }
    }
}