using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class CameraFrame
    {
        public double ScreenX { get; set; }
        public double ScreenY { get; set; }
        public double[,] BackgroundXY { get; set; }
        public List<Tuple<double, double, int>> StableWells { get; set; }
        public List<Tuple<double, double>> UnstableWells { get; set; }
        public List<Tuple<double, double, int>> Orbs { get; set; }
        public List<int> PlayerOrbs { get; set; }
        public List<Tuple<double, double>> AIShips { get; set; }
        public Tuple<double, double> PlayerShip { get; set; }
        public List<Tuple<double, double, int>> ShockWaves { get; set; } 
        public List<Tuple<double, double>>[] Backgrounds { get; set; }
        public double PlayerAngle { get; set; }
        public int SecondsLeft { get; set; }
        public int Seconds { get; set; }
        public bool IsOver { get; set; }
        public bool HasGhostingPowerup { get; set; }
        public bool HasDestabilizePowerup { get; set; }
        public bool HasNeutralizePowerup { get; set; }
        public int Points { get; internal set; }

        private List<int> screenStables;
        public List<int> ScreenStables
        {
            get { return screenStables; }
            set { screenStables = value; }
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
        }

     
    }
}
