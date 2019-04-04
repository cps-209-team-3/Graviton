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
            while (Orbs.Count < 30)
            {
                SpawnOrb();
            }
            while (StableWells.Count < 15)
            {
                SpawnWell();
            }

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            Timer.Tick += Timer_Tick;
            Timer.Start();
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
                    Player.GamePowerup.Neutralize();
                    break;
                case 'f':
                    Player.GamePowerup.Destabilize();
                    break;
                case 'e':
                    Player.GamePowerup.Ghost();
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

        public void Timer_Tick(object sender, EventArgs e)
        {
            Ticks++;
            UpdatePlayer();
            UpdateWells();
            if (Ticks % 400 == 0)
                SpawnWell();
            if (Ticks % 100 == 0)
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
                    well.TicksLeft = 3000;
                    well.IsStable = false;
                    UnstableWells.Add(well);
                    StableWells.Remove(well);
                }
            }
            foreach (Well well in UnstableWells)
            {
                well.TicksLeft--;
                // do a shock wave every so often????
                if (well.TicksLeft == 0)
                {
                    // any explosions or something????
                    UnstableWells.Remove(well);
                    GameObjects.Remove(well);
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
                {
                    StableWells.Remove(well);
                    GameObjects.Remove(well);
                }
                    
                    
            }
            Orb orb = Player.OrbOver();
            if (orb != null)
            {
                Orbs.Remove(orb);
                GameObjects.Remove(orb);
                Player.Orbs.Add(orb.Color);
                Player.Orbs.Sort();
            }
        }
        public void UpdatePlayerPosition()
        {
            foreach (Well well in StableWells.Concat(UnstableWells))
            {
                double deltaX = well.Xcoor - Player.Xcoor;
                double deltaY = well.Ycoor - Player.Ycoor;
                double dist = Math.Max(0.01, Math.Pow(deltaX * deltaX + deltaY * deltaY, 0.5));
                double force = well.Strength / Math.Max(31.0, dist);
                Player.SpeedX += deltaX / dist * force;
                Player.SpeedY += deltaY / dist * force;
            }
            Player.Move(HorizontalInput, VerticalInput);
        }
        public void SpawnWell()
        {
            double xc = Random.NextDouble() * 5000.0;
            double yc = Random.NextDouble() * 5000.0;
            if (!NearOtherObject(xc, yc))
            {
                Well well = new Well(xc, yc);
                StableWells.Add(well);
                GameObjects.Add(well);
            }
        }
        public void SpawnOrb()
        {
            double xc = Random.NextDouble() * 5000.0;
            double yc = Random.NextDouble() * 5000.0;
            if (!NearOtherObject(xc, yc))
            {
                Orb orb = new Orb(xc, yc, Random.Next(6));
                Orbs.Add(orb);
                GameObjects.Add(orb);
            }    
        }
        public bool NearOtherObject(double xc, double yc)
        {
            foreach (GameObject obj in GameObjects)
            {
                if (Math.Pow(xc - obj.Xcoor, 2) + Math.Pow(yc - obj.Ycoor, 2) < 40000)
                    return true;
            }
            return false;
        }

        public string Serialize()
        {
            return null; //TODO
        }
    }
}
