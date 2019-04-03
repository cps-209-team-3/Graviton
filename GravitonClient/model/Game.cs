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
        public Random Random { get; set; }
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
        public List<GameObject> GameObjects { get; set; }
        public string Username { get; internal set; }

        public Game(bool isCheat)
        {
            IsCheat = isCheat;
            IsOver = false;
            Random = new Random();
            ViewCamera = new Camera(this);
            Points = 0;
            Ticks = 0;
            HorizontalInput = 0;
            VerticalInput = 0;
            StableWells = new List<Well>();
            UnstableWells = new List<Well>();
            Orbs = new List<Orb>();
            GameObjects = new List<GameObject>();
            Initialize();
        }

        public void Initialize()
        {
            Player = new Ship(2500.0, 2500.0, this);
            GameObjects.Add(Player);
            while (Orbs.Count < 40)
            {
                SpawnOrb();
            }
            while (StableWells.Count < 20)
            {
                SpawnWell();
            }
            

            //TODO Timer initialization
        }


        public void KeyPressed(char c)
        {
            switch (c)
            {
                case 'w':
                    VerticalInput--;
                    break;
                case 'a':
                    HorizontalInput--;
                    break;
                case 's':
                    VerticalInput++;
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
                    VerticalInput++;
                    break;
                case 'a':
                    HorizontalInput++;
                    break;
                case 's':
                    VerticalInput--;
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
            ViewCamera.Render();
            GameUpdatedEvent(this, 0);
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
            // TODO - Gravity - change Player speeds
            Player.Move(HorizontalInput, VerticalInput);
        }
        public void SpawnWell()
        {
            double xc = Random.NextDouble() * 5000.0;
            double yc = Random.NextDouble() * 5000.0;
            foreach (GameObject obj in GameObjects)
            {
                if (Math.Pow(xc - obj.Xcoor, 2) + Math.Pow(yc - obj.Ycoor, 2) < 100000)
                    return;
            }
            Well well = new Well(xc, yc);
            StableWells.Add(well);
            GameObjects.Add(well);
        }
        public void SpawnOrb()
        {
            double xc = Random.NextDouble() * 5000.0;
            double yc = Random.NextDouble() * 5000.0;
            foreach (GameObject obj in GameObjects)
            {
                if (Math.Pow(xc - obj.Xcoor, 2) + Math.Pow(yc - obj.Ycoor, 2) < 100000)
                    return;
            }
            Orb orb = new Orb(xc, yc, Random.Next(6));
            Orbs.Add(orb);
            GameObjects.Add(orb);
        }

        public string Serialize()
        {
            return null; //TODO
        }
    }
}
