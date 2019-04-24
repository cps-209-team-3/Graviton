//This file contains the CameraFrame class which holds all of the information for one frame in the game.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    //This class stores all of the information for one frame in the game.
    //It only has instance variables; no methods.
    public class CameraFrame
    {
        //The screen coordinates
        public double ScreenX { get; set; }
        public double ScreenY { get; set; }
        //The background coordinates
        public double[,] BackgroundXY { get; set; }
        //The coordinates and color (as appropriate) for stable wells, unstable wells, orbs, player orbs, ai ships, player ship, shockwaves.
        public List<Tuple<double, double, int>> StableWells { get; set; }
        public List<Tuple<double, double>> UnstableWells { get; set; }
        public List<Tuple<double, double, int>> Orbs { get; set; }
        public List<int> PlayerOrbs { get; set; }
        public List<Tuple<double, double>> AIShips { get; set; }
        public Tuple<double, double> PlayerShip { get; set; }
        public List<Tuple<double, double, int>> ShockWaves { get; set; }
        //Backgrounds coordinates
        public List<Tuple<double, double>>[] Backgrounds { get; set; }
        //The player's angle direction
        public double PlayerAngle { get; set; }
        //The seconds left for a well in the game
        public int SecondsLeft { get; set; }
        //How many seconds left for the game
        public int Seconds { get; set; }
        //If the game is over
        public bool IsOver { get; set; }
        //Whther each powerup is enabled
        public bool HasGhostingPowerup { get; set; }
        public bool HasDestabilizePowerup { get; set; }
        public bool HasNeutralizePowerup { get; set; }
        //How many points the player has
        public int Points { get; internal set; }

        //stable and unstable wells in the game (for animation)
        private List<int> screenStables;
        public List<int> ScreenStables
        {
            get { return screenStables; }
            set { screenStables = value; }
        }

        private List<int> screenUnstables;
        public List<int> ScreenUnstables
        {
            get { return screenUnstables; }
            set { screenUnstables = value; }
        }

        private List<int> screenAI;
        public List<int> ScreenAI
        {
            get { return screenAI; }
            set { screenAI = value; }
        }

        public CameraFrame()
        {
            Backgrounds = new List<Tuple<double, double>>[4];
            BackgroundXY = new double[4, 2];
            StableWells = new List<Tuple<double, double, int>>();
            UnstableWells = new List<Tuple<double, double>>();
            PlayerOrbs = new List<int>();
            AIShips = new List<Tuple<double, double>>();
            Orbs = new List<Tuple<double, double, int>>();
            screenStables = new List<int>();
            screenUnstables = new List<int>();
            screenAI = new List<int>();
        }

     
    }
}
