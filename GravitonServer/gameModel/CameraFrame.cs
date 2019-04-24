using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonServer
{
    public class CameraFrame
    {
        public double ChangeX { get; set; }
        public double ChangeY { get; set; }
        public double ScreenX { get; set; }
        public double ScreenY { get; set; }
        public double[,] BackgroundXY { get; set; }
        public List<Tuple<double, double, int>> StableWells { get; set; }
        public List<Tuple<double, double>> UnstableWells { get; set; }
        public List<Tuple<double, double, int>> Orbs { get; set; }
        public List<int> PlayerOrbs { get; set; }
        public List<Tuple<double, double>> AIShips { get; set; }
        public Tuple<double, double> PlayerShip { get; set; }
        public List<Tuple<double, double>>[] Backgrounds { get; set; }
        public double PlayerAngle { get; set; }
        public int SecondsLeft { get; set; }
        public int Seconds { get; set; }
        public bool IsOver { get; set; }
        public bool HasGhostingPowerup { get; set; }
        public bool HasDestabilizePowerup { get; set; }
        public bool HasNeutralizePowerup { get; set; }
        public int Points { get; internal set; }
        public List<Tuple<double, double, string>> OtherHumanShips { get; set; }


        public CameraFrame()
        {
            Backgrounds = new List<Tuple<double, double>>[4];
            BackgroundXY = new double[4, 2];
        }

        public string Serialize()
        {
            
            return $"{Math.Round(ScreenX,2)} {Math.Round(ScreenY, 2)} {Math.Round(ChangeX, 2)} {Math.Round(ChangeY, 2)} {SerializeTuples<int>(StableWells)} {SerializeTuples(UnstableWells)} {SerializeTuples<int>(Orbs)} {String.Join('|', PlayerOrbs.ToArray())} {SerializeTuples(AIShips)} {SerializeTuples(OtherHumanShips)} {Seconds} {SecondsLeft} {Points} {(HasGhostingPowerup?"t":"f")} {(HasNeutralizePowerup ? "t" : "f")} {(HasDestabilizePowerup ? "t" : "f")}";
        }

        private static string SerializeTuples<T>(List<Tuple<double, double, T>> tuples)
        {
            string retVal = "";
            int i = 0;
            Tuple<double, double, T> currentTuple;
            if (tuples.Count > 1)
            {
                for (; i < tuples.Count - 1; i++)
                {
                    currentTuple = tuples[i];
                    retVal += $"{Math.Round(currentTuple.Item1, 2)},{Math.Round(currentTuple.Item2, 2)},{currentTuple.Item3}|";
                }
            }
            if (tuples.Count > 0)
            {
                currentTuple = tuples[i];
                retVal += $"{Math.Round(currentTuple.Item1, 2)},{Math.Round(currentTuple.Item2, 2)},{currentTuple.Item3}";
            }
            return retVal;
        }

        private static string SerializeTuples(List<Tuple<double, double>> tuples)
        {
            string retVal = "";
            int i = 0;
            Tuple<double, double> currentTuple;
            if (tuples.Count > 1)
            {
                for (; i < tuples.Count - 1; i++)
                {
                    currentTuple = tuples[i];
                    retVal += $"{Math.Round(currentTuple.Item1, 2)},{Math.Round(currentTuple.Item2, 2)}|";
                }
                currentTuple = tuples[i];
                retVal += $"{Math.Round(currentTuple.Item1, 2)},{Math.Round(currentTuple.Item2, 2)}";
            }
            else if(tuples.Count == 1)
                retVal += $"{Math.Round(tuples[i].Item1, 2)},{Math.Round(tuples[i].Item2, 2)}";
            return retVal;
        }
    }
}
