using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace GravitonClient
{
    class Game
    {
        public event EventHandler<int> GameUpdatedEvent;
        public bool IsCheat { get; set; }
        public bool IsOver { get; set; }
        public Camera ViewCamera { get; set; }
        public Powerup GamePowerup { get; set; }
        public int Points { get; set; }
        public int Ticks { get; set; }
        public int HorizontalInput { get; set; }
        public int VerticalInput { get; set; }
        public DispatcherTimer Timer { get; set; }
        public List<Well> StableWells { get; set; }
        public List<Well> UnstableWells { get; set; }
        public Ship Player { get; set; }
        public List<Orb> Orbs { get; set; }
        public string Username { get; internal set; }

        public Game(bool isCheat)
        {
            IsCheat = isCheat;
            IsOver = false;
            ViewCamera = new Camera(this);
            Points = 0;
            Ticks = 0;
            HorizontalInput = 0;
            VerticalInput = 0;
            StableWells = new List<Well>();
            UnstableWells = new List<Well>();
            Orbs = new List<Orb>();
            Player = new Ship(100.0, 100.0, this);
            Initialize();
        }

        public void Initialize()
        {
            // TODO
            //pseudorandom orbs
            //pseudorandom wells
            //Timer initialization
        }


        public void KeyPressed(char c)
        {
            switch (c)
            {
                case 'w':
                    VerticalInput++;
                    break;
                case 'a':
                    HorizontalInput--;
                    break;
                case 's':
                    VerticalInput--;
                    break;
                case 'd':
                    HorizontalInput++;
                    break;
                case ' ':
                    Player.SpeedBoost();
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

        public void KeyReleased(char c)
        {
            switch (c)
            {
                case 'w':
                    VerticalInput--;
                    break;
                case 'a':
                    HorizontalInput++;
                    break;
                case 's':
                    VerticalInput++;
                    break;
                case 'd':
                    HorizontalInput--;
                    break;
            }

        }

        public void Timer_Tick()
        {
            Ticks++;
            UpdatePlayer();
            UpdateWells();
            if (Ticks % 480 == 0)
                SpawnWell();
            if (Ticks % 120 == 0)
                SpawnOrb();
            GameUpdatedEvent(this, Ticks / 60);
        }
        public void UpdateWells()
        {
            foreach (Well well in StableWells)
            {
                well.TicksLeft--;
                if (well.TicksLeft == 0)
                {
                    well.IsStable = false;
                    UnstableWells.Add(well);
                    StableWells.Remove(well);
                }
            }
        }
        public void UpdatePlayer()
        {
            UpdatePlayerPosition();
            Well well = Player.WellOver();
            if (well != null)
            {
                if (!well.IsStable)
                    IsOver = true;
                else if (Player.DepositOrbs(well))
                    StableWells.Remove(well);
            }
            Orb orb = Player.OrbOver();
            if (orb != null)
            {
                Orbs.Remove(orb);
                Player.Orbs.Add(orb);
                Player.SortOrbs();
            }
        }
        public void UpdatePlayerPosition()
        {
            // Gravity - change Player speeds
            Player.Move(HorizontalInput, VerticalInput);
        }
        public void SpawnWell()
        {
            double xc = 100.0;
            double yc = 100.0;
            // TODO check if it is too near anything else 
            StableWells.Add(new Well(xc, yc));
        }
        public void SpawnOrb()
        {
            double xc = 100.0;
            double yc = 100.0;
            // TODO check if it is too near anything else
            Orbs.Add(new Orb(xc, yc, 0));
        }

        public string Serialize()
        {
            return null; //TODO
        }
    }
}
