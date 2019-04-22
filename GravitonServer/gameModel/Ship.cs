using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonServer
{
    public enum SoundEffect { OrbGrab, PowerupGrab, Neutralize, Destabilize, OrbDrop, Ghost, Collapse, Boost };
    public class Ship : GameObject
    {
        public string Username { get; internal set; }
        public Game ParentGame { get; set; }
        public Powerup GamePowerup { get; set; }
        public double SpeedX { get; set; }
        public double SpeedY { get; set; }
        public double BoostFactor { get; set; }
        public List<int> Orbs { get; set; }
        public int Points{ get; set; }
        public bool IsImmune { get; set; }
        public int ImmuneTicksLeft { get; set; }
        public int VerticalInput { get; set; }
        public int HorizontalInput { get; set; }
        public Camera ViewCamera { get; set; }

        private Random rand = new Random();

        public event EventHandler PlayerDiedEvent; 

        public Ship(double xcoor, double ycoor, Game game)
        {
            ParentGame = game;
            GamePowerup = new Powerup(this);
            ViewCamera = new Camera(game, this);
            IsImmune = false;
            Xcoor = xcoor;
            Ycoor = ycoor;
            SpeedX = 0.0;
            SpeedY = 0.0;
            BoostFactor = 1.0;
            Orbs = new List<int>();
        }

        internal void Die()
        {
            PlayerDiedEvent(this, null);
        }

        public Ship() : base() {
            SpeedX = 0.0;
            SpeedY = 0.0;
            BoostFactor = 1.0;
            Orbs = new List<int>();
        }

        //This method moves the ship according to its inputs and speeds.
        public void Move()
        {
            SpeedX += 0.3 * HorizontalInput * BoostFactor;
            SpeedY += 0.3 * VerticalInput * BoostFactor;
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
        //This method deals with a keypress. It either updates the user directional input, does a speed boost, or uses a powerup.
        public void KeyPressed(char c)
        {
            switch (c)
            {
                case 'w':
                    VerticalInput = -1;
                    break;
                case 'a':
                    HorizontalInput = -1;
                    break;
                case 's':
                    VerticalInput = 1;
                    break;
                case 'd':
                    HorizontalInput = 1;
                    break;
                case ' ':
                    SpeedBoost();
                    break;
                case 'q':
                    GamePowerup.Neutralize();
                    break;
                case 'f':
                    GamePowerup.Destabilize();
                    break;
                case 'e':
                    GamePowerup.Ghost();
                    break;
            }
        }

        //This method deals with a key release. It updates the user directional input.
        public void KeyReleased(char c)
        {
            switch (c)
            {
                case 'w':
                case 's':
                    VerticalInput = 0;
                    break;
                case 'a':
                case 'd':
                    HorizontalInput = 0;
                    break;

            }

        }

        //This method gives the ship a speed boost.
        public void SpeedBoost()
        {
            if (Orbs.Count >= 3)
            {
                //    InvokeSoundEventForShip(this, SoundEffect.Boost);
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
                if (orb == well.Orbs)
                {
                    if (well.Orbs == 5)
                        completed = true;
                    else
                        well.Orbs++;
                    IncrementScore();
                    Orbs.Remove(orb);
                    well.TicksLeft = ParentGame.WellDestabFreq;
                }
            }
            return completed;
        }

        public virtual void IncrementScore()
        {
            Points += 10;
        }
       
    }
}
