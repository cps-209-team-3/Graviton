﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;


namespace GravitonClient
{
    public class Game
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
        public int WellSpawnFreq { get; set; }
        public int WellDestabFreq { get; set; }
        public DispatcherTimer Timer { get; set; }
        public List<Well> StableWells { get; set; }
        public List<Well> UnstableWells { get; set; }
        public Ship Player { get; set; }
        public List<AIShip> AIShips { get; set; }
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
            WellSpawnFreq = 400;
            WellDestabFreq = 4000;
            StableWells = new List<Well>();
            UnstableWells = new List<Well>();
            AIShips = new List<AIShip>();
            Orbs = new List<Orb>();
            GameObjects = new List<GameObject>();
        }

        //This method initializes the ship, all of the wells, all of the orbs, and the timer.
        public void Initialize()
        {
            Player = new Ship(2500.0, 2500.0, this);
            GameObjects.Add(Player);
            while (Orbs.Count < 100)
            {
                SpawnOrb();
            }
            while (StableWells.Count < 20)
            {
                SpawnWell();
            }
            while (AIShips.Count < 7)
            {
                SpawnAI();
            }

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            Timer.Start();
            Timer.Tick += Timer_Tick;
        }

        public void InitializeWithShipCreated()
        {
            GameObjects.Add(Player);
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            Timer.Start();
            Timer.Tick += Timer_Tick;
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

        //This method deals with a key release. It updates the user directional input.
        public void KeyReleased(char c)
        {
            switch (c)
            {
                case 'w':
                    VerticalInput = 0;
                    break;
                case 'a':
                    HorizontalInput = 0;
                    break;
                case 's':
                    VerticalInput = 0;
                    break;
                case 'd':
                    HorizontalInput = 0;
                    break;
            }

        }

        //This method is called every frame and updates everything in the game, then notifies the view.
        public void Timer_Tick(object sender, EventArgs e)
        {
            Ticks++;
            UpdatePlayer();
            UpdateAI();
            UpdateWells();
            if (Ticks % WellSpawnFreq == 0)
                SpawnWell();
            if (Ticks % 10 == 0)
                SpawnOrb();
            if (Ticks % 200 == 0)
            {
                foreach (AIShip aI in AIShips)
                {
                    if (!aI.IsCloser())
                    {
                        aI.SetTargetPos();
                    }
                }
            }
            if (Ticks == 15000)
            {
                GameOver();
            }
            ViewCamera.Render();
            GameUpdatedEvent(this, 0);
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
                    well.Strength = 50;
                    UnstableWells.Add(well);
                    StableWells.Remove(well);
                }
            }
            foreach (Well well in UnstableWells.ToList())
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

        //This method updates the player's position and what orbs it has.
        public void UpdatePlayer()
        {
            UpdatePlayerPosition();
            Well well = Player.WellOver();
            if (well != null)
            {
                if (!well.IsStable)
                {
                    if (!IsCheat)
                        GameOver();
                }
                else if (Player.DepositOrbs(well))
                {
                    StableWells.Remove(well);
                    GameObjects.Remove(well);
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
        }

        //updates gravity effects on parameter ship
        public void UpdateGravity(Ship ship)
        {
            foreach (Well well in StableWells.Concat(UnstableWells))
            {
                double deltaX = well.Xcoor - ship.Xcoor;
                double deltaY = well.Ycoor - ship.Ycoor;
                double dist = Math.Max(0.01, Math.Pow(deltaX * deltaX + deltaY * deltaY, 0.5));
                double force = well.Strength / Math.Max(30, dist);
                ship.SpeedX += deltaX / dist * force;
                ship.SpeedY += deltaY / dist * force;
            }
        }

        //This method updates the player's position.
        public void UpdatePlayerPosition()
        {
            UpdateGravity(Player);
            Player.Move(HorizontalInput, VerticalInput);
        }

        //Updates AI position and collected orbs
        public void UpdateAI()
        {
            foreach (AIShip aI in AIShips.ToList())
            {
                UpdateAIPosition(aI);
                Well well = aI.WellOver();
                if (well != null)
                {
                    if (!well.IsStable)
                    {
                        AIShips.Remove(aI);
                        GameObjects.Remove(aI);
                    }
                    else if (aI.DepositOrbs(well))
                    {
                        StableWells.Remove(well);
                        GameObjects.Remove(well);
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
                        if (aI.Orbs.Count >= 3)
                        {
                            aI.SetTargetPos();
                        }
                    }
                }
            }
        }

        //updates AI position
        public void UpdateAIPosition(AIShip aI)
        {
            UpdateGravity(aI);
            aI.AIMove();
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
            GameLoader.Save(this, "C:\\temp\\temp.json"); //Change this filename
        }
    }
}
