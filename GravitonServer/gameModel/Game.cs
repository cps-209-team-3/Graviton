using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;


namespace GravitonServer
{
    public class Game
    {
        public event EventHandler<int> GameUpdatedEvent;
        public bool IsOver { get; set; }
        public Random Random { get; set; }
        public int Ticks { get; set; }
        public int HorizontalInput { get; set; }
        public int VerticalInput { get; set; }
        public int WellSpawnFreq { get; set; }
        public int WellDestabFreq { get; set; }
        public Timer Timer { get; set; }
        public List<Well> StableWells { get; set; }
        public List<Well> UnstableWells { get; set; }
        public List<Ship> Players { get; set; }
        public List<AIShip> AIShips { get; set; }
        public List<Orb> Orbs { get; set; }
        public List<GameObject> GameObjects { get; set; }
        private DateTime StartTime;

        internal HighScores HighScores = new HighScores();
        

        public Game()
        {
            IsOver = false;
            Random = new Random();
            Ticks = 0;
            HorizontalInput = 0;
            VerticalInput = 0;
            WellSpawnFreq = 400;
            WellDestabFreq = 4000;
            StableWells = new List<Well>();
            UnstableWells = new List<Well>();
            AIShips = new List<AIShip>();
            Orbs = new List<Orb>();
            GameObjects = new List<GameObject>();
            Players = new List<Ship>();
        }

        //This method initializes all of the wells, all of the orbs, and the timer.
        public void Initialize()
        {
            StartTime = DateTime.Now;
            while (Orbs.Count < 100)
            {
                SpawnOrb();
            }
            while (StableWells.Count < 20)
            {
                SpawnWell();
            }
            while (AIShips.Count < 10)
            {
                SpawnAI();
            }

            Timer = new Timer(50);
            Timer.AutoReset = true;
            Timer.Elapsed += Timer_Tick;
            
        }

        public Ship AddPlayer()
        {
            Ship player = new Ship(Random.NextDouble() * 5000, Random.NextDouble() * 5000, this);
            Players.Add(player);
            player.PlayerDiedEvent += RemovePlayer;
            return player;
        }

        private void RemovePlayer(object sender, EventArgs e)
        {
            try
            {
                Ship player = sender as Ship;
                Players.Remove(player);
            }
            catch { }
        }

        public void StartGame()
        {
            Initialize();

            Timer.Enabled = true;
        }


       

        

        //This method is called every frame and updates everything in the game, then notifies the view.
        public void Timer_Tick(object sender, EventArgs e)
        {
            Ticks++;
            foreach(Ship player in Players.ToArray())
                UpdatePlayer(player);
            UpdateAI();
            UpdateWells();
            if (Ticks % WellSpawnFreq == 0)
                SpawnWell();
            if (Ticks % 5 == 0 && Orbs.Count < 170)
                SpawnOrb();
            if (AIShips.Count < 3)
                SpawnAI();     

            
            GameUpdatedEvent(this, 0);
        }

        internal GameStats GetStats()
        {
            HighScores.CheckNewScores(this);
            GameStats stats = new GameStats();
            stats.SetHighScores(HighScores);
            return stats;
        }

        // This method updates all the wells in the game.
        public void UpdateWells()
        {
            foreach (Well well in StableWells.ToList())
            {
                well.TicksLeft--;
                if (well.TicksLeft == 0)
                {
                    well.TicksLeft = 3000;
                    well.IsStable = false;
                    well.Strength = 900;
                    UnstableWells.Add(well);
                    StableWells.Remove(well);
                    
                }
            }
            foreach (Well well in UnstableWells.ToList())
            {
                
                if (well.TicksLeft == 0)
                {
                    // any explosions or something????
                    UnstableWells.Remove(well);
                    GameObjects.Remove(well);
                }
            }
        }

        //This method updates the player's position and what orbs it has.
        public void UpdatePlayer(Ship Player)
        {

            UpdateGravity(Player);
            Player.Move();
            Well well = Player.WellOver();
            if (well != null)
            {
                int originalColor = well.Orbs;
                if (!well.IsStable)
                {
                    if (!Player.IsImmune)
                        Player.Die();
                }
                else if (Player.DepositOrbs(well))
                {
                    StableWells.Remove(well);
                    GameObjects.Remove(well);
                    Player.GamePowerup.AddNew();
                    Player.Points += 100;
                }
            }
            Orb orb = Player.OrbOver();
            if (orb != null)
            {
                if (Player.Orbs.Count < 5)
                {
                    Orbs.Remove(orb);
                    GameObjects.Remove(orb);
                    Player.Orbs.Add(orb.Color);
                    Player.Orbs.Sort();
                    
                }
            }
            if (Player.ImmuneTicksLeft > 0)
                --Player.ImmuneTicksLeft;
            if (Player.ImmuneTicksLeft == 0)
                Player.IsImmune = false;
        }

       

        //updates gravity effects on parameter ship
        public void UpdateGravity(Ship ship)
        {
            foreach (Well well in StableWells.Concat(UnstableWells))
            {
                double deltaX = well.Xcoor - ship.Xcoor;
                double deltaY = well.Ycoor - ship.Ycoor;
                double dist = Math.Max(0.01, Math.Pow(deltaX * deltaX + deltaY * deltaY, 0.5));
                double force = well.Strength / Math.Max(200, Math.Pow(dist, 1.5));
                ship.SpeedX += deltaX / dist * force;
                ship.SpeedY += deltaY / dist * force;
            }
        }


        //Updates AI position and collected orbs
        public void UpdateAI()
        {
            foreach (AIShip aI in AIShips.ToList())
            {
                UpdateGravity(aI);
                aI.AIMove();
                Well well = aI.WellOver();
                if (well != null)
                {
                    if (!well.IsStable)
                    {
                        if (well.IsTrap)
                            well.PlayerWhoSetTrap.Points += 200;
                        AIShips.Remove(aI);
                        GameObjects.Remove(aI);
                    }
                    else if (aI.DepositOrbs(well) && !well.IsGhost)
                    {
                        StableWells.Remove(well);
                        GameObjects.Remove(well);
                        aI.GamePowerup.AddNew();
                        aI.SetTargetPos();
                    }
                }
                Orb orb = aI.OrbOver();
                if (orb != null)
                {
                    if (aI.Orbs.Count < 5)
                    {
                        Orbs.Remove(orb);
                        GameObjects.Remove(orb);
                        aI.Orbs.Add(orb.Color);
                        aI.Orbs.Sort();
                        aI.SetTargetPos();
                    }
                }
                aI.UseNeutralize();
                aI.UseDestabilize();
                aI.UseGhost();
            }
        }


        //This method usually spawns a well. It sometimes not spawning a well has 2 reasons:
        //#1: To add a little bit of randomness to the game.
        //#2: So the game screen doesn't get too cluttered. (If it doesn't find an empty space, it doesn't spawn)
        public void SpawnWell()
        {
            double xc = Random.NextDouble() * 5000.0;
            double yc = Random.NextDouble() * 5000.0;
            if (!NearOtherObject(xc, yc))
            {
                Well well = new Well(xc, yc);
                well.TicksLeft = WellDestabFreq + Random.Next(1001);
                StableWells.Add(well);
                GameObjects.Add(well);
            }
        }

        //This method usually spawns an orb.
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

        //Spawns an AIShip object in a random position
        public void SpawnAI()
        {
            double xc = Random.NextDouble() * 5000.0;
            double yc = Random.NextDouble() * 5000.0;
            if (!NearOtherObject(xc, yc))
            {
                AIShip aI = new AIShip(xc, yc, this);
                AIShips.Add(aI);
                GameObjects.Add(aI);
            }
        }

        //This method is used by the spawn methods to tell whether a given location is too neat another object.
        public bool NearOtherObject(double xc, double yc)
        {
            foreach (GameObject obj in GameObjects)
            {
                if (Math.Pow(xc - obj.Xcoor, 2) + Math.Pow(yc - obj.Ycoor, 2) < 40000)
                    return true;
            }
            return false;
        }

        //This method is called when the game ends.
        public void GameOver()
        {
            IsOver = true;
            Timer.Stop();
           
        }
    }
}
