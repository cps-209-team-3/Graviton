using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class Powerup
    {
        public enum powerups { neutralize, destabilize, ghost}
        public List<powerups> CurrentPowerups;
        public Game ParentGame;
        public bool CarryingNeutralize { get; set; }
        public bool CarryingDestabilize { get; set; }
        public bool CarryingGhost { get; set; }

        private Random rand = new Random();

        public event EventHandler<SoundEffect> GameInvokeSoundEvent;

        public event EventHandler<AnimationEventArgs> UpdateAnimationEvent;

        public Powerup(Game game)
        {
            ParentGame = game;
            CarryingNeutralize = false;
            CarryingDestabilize = false;
            CarryingGhost = false;
            CurrentPowerups = new List<powerups>();
        }

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
                    GameInvokeSoundEvent(this, SoundEffect.Neutralize);
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
                                if (ParentGame.Player.GamePowerup == this)
                                    GameInvokeSoundEvent(this, SoundEffect.PowerupGrab);
                                CarryingNeutralize = true;
                                powerupAdded = true;
                            }
                            break;
                        case 1:
                            if (!CarryingDestabilize)
                            {
                                CurrentPowerups.Add(powerups.destabilize);
                                if (ParentGame.Player.GamePowerup == this)
                                    GameInvokeSoundEvent(this, SoundEffect.PowerupGrab);
                                CarryingDestabilize = true;
                                powerupAdded = true;
                            }
                            break;
                        case 2:
                            if (!CarryingGhost)
                            {
                                CurrentPowerups.Add(powerups.ghost);
                                if (ParentGame.Player.GamePowerup == this)
                                    GameInvokeSoundEvent(this, SoundEffect.PowerupGrab);
                                CarryingGhost = true;
                                powerupAdded = true;
                            }
                            break;
                    }
                }
            }
        }


        //This method neutralizes a nearby destabilized well
        public void Neutralize(Ship ship)
        {
            if (!CarryingNeutralize)
                return;
            foreach (Well well in ParentGame.UnstableWells.ToList())
            {
                if (Math.Pow(ship.Xcoor - well.Xcoor, 2) + Math.Pow(ship.Ycoor - well.Ycoor, 2) < 40000)
                {
                    if (ParentGame.Player.GamePowerup == this)
                        GameInvokeSoundEvent(this, SoundEffect.Neutralize);
                    int objIndex = ParentGame.UnstableWells.FindIndex(item => item.Equals(well));
                    UpdateAnimationEvent(this, new AnimationEventArgs(false, AnimationType.Unstable, objIndex, 24, 0));
                    ParentGame.UnstableWells.Remove(well);
                    ParentGame.GameObjects.Remove(well);
                    CarryingNeutralize = false;
                    CurrentPowerups.Remove(powerups.neutralize);
                }
            }
        }

        //This method destabilizes a nearby well
        public void Destabilize(Ship ship)
        {
            if (!CarryingDestabilize)
                return;
            foreach (Well well in ParentGame.StableWells.ToList())
            {
                if (Math.Pow(ship.Xcoor - well.Xcoor, 2) + Math.Pow(ship.Ycoor - well.Ycoor, 2) < 40000)
                {
                    if (ParentGame.Player.GamePowerup == this)
                        GameInvokeSoundEvent(this, SoundEffect.Destabilize);
                    int objIndex = ParentGame.StableWells.FindIndex(item => item.Equals(well));
                    UpdateAnimationEvent(this, new AnimationEventArgs(false, AnimationType.Stable, objIndex, 12, 0));
                    CarryingDestabilize = false;
                    CurrentPowerups.Remove(powerups.destabilize);
                    well.TicksLeft = 3000;
                    well.IsStable = false;
                    well.Strength = 900;
                    well.IsTrap = true;
                    well.Owner = ship;
                    ParentGame.UnstableWells.Add(well);
                    UpdateAnimationEvent(this, new AnimationEventArgs(false, AnimationType.Unstable, ParentGame.UnstableWells.Count, 0, 0));
                    ParentGame.StableWells.Remove(well);
                    ship.IsImmune = true;
                    ship.ImmuneTicksLeft = 50;
                }
            }
        }

        //This method locks AI from depositing in a nearby well.
        public void Ghost(Ship ship)
        {
            if (!CarryingGhost)
                return;
            foreach (Well well in ParentGame.StableWells.ToList())
            {
                if (Math.Pow(ship.Xcoor - well.Xcoor, 2) + Math.Pow(ship.Ycoor - well.Ycoor, 2) < 40000)
                {
                    if (ParentGame.Player.GamePowerup == this)
                        GameInvokeSoundEvent(this, SoundEffect.Ghost);
                    well.IsGhost = true;
                    well.Owner = ship;
                }
            }
            CarryingGhost = false;
            CurrentPowerups.Remove(powerups.ghost);
            ship.IsImmune = true;
            ship.ImmuneTicksLeft = 100;
        }
    }
}