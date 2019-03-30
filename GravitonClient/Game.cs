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
        public Camera ViewCamera { get; set; }
        public int Points { get; set; }
        public int Ticks { get; set; }
        public int HorizontalInput { get; set; }
        public int VerticalInput { get; set; }
        public DispatcherTimer Timer { get; set; }
        public List<StableWell> StableWells { get; set; }
        public List<UnstableWell> UnstableWells { get; set; }
        public Ship User { get; set; }
        public List<Orb> Orbs { get; set; }
        public Game(bool isCheat)
        {
            IsCheat = isCheat;
            ViewCamera = new Camera(this);
            Points = 0;
            Ticks = 0;
            HorizontalInput = 0;
            VerticalInput = 0;
            StableWells = new List<StableWell>();
            UnstableWells = new List<UnstableWell>();
            Orbs = new List<Orb>();
            User = new Ship(100.0, 100.0, this);
            Initialize();
        }

        public void Initialize()
        {
            //pseudorandom orbs
            //pseudorandom wells
            //Timer initialization
        }

        public void Load(string filename)
        {
            
        }

        public void Save(string filename)
        {

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
                    User.Speed += 2;
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
            UpdateUser();
            UpdateWells();
            if (Ticks % 480 == 0)
                SpawnWell();
            if (Ticks % 120 == 0)
                SpawnOrb();
            GameUpdatedEvent(this, Ticks / 60);

        }
        public void UpdateWells()
        {
            for (int i = 0; i < StableWells.Count; i++)
            {
                StableWell well = StableWells[i];
                well.TicksLeft--;
                if (well.TicksLeft == 0)
                {
                    UnstableWells.Add(new UnstableWell(well.Xcoor, well.Ycoor));
                    StableWells.RemoveAt(i);
                }
            }
        }
        public void UpdateUser()
        {
            User.Xcoor += HorizontalInput;
            User.Ycoor += VerticalInput;
            // Gravity
        }
        public void SpawnWell()
        {
            double xc = 100.0;
            double yc = 100.0;
            // check if it is too near anything else
            StableWells.Add(new StableWell(xc, yc));
        }
        public void SpawnOrb()
        {
            double xc = 100.0;
            double yc = 100.0;
            // check if it is too near anything else
            Orbs.Add(new Orb(xc, yc, 0));
        }
    }
}
