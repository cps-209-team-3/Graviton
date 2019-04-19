using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class NetworkedCameraFrame : CameraFrame
    {

        private static double[,] BackgroundXY = new double[4, 2];
        
        public static int Width
        {
            get;
            set;
        }
        public static int Height
        {
            get; set;
        }

        public List<Tuple<double, double>> OtherHumanShips; 
        public NetworkedCameraFrame() : base() { }
        public static NetworkedCameraFrame Deserialize(string data)
        {
            string[] parts = data.Split(' ');
            NetworkedCameraFrame newFrame = new NetworkedCameraFrame();
            newFrame.ScreenX = Convert.ToDouble(parts[0]);
            newFrame.ScreenX = Convert.ToDouble(parts[1]);

            double changeX = Convert.ToDouble(parts[2]);
            double changeY = Convert.ToDouble(parts[3]);
            var Backgrounds = new List<Tuple<double, double>>[4];
            for (int i = 0; i < 4; i++)
            {
                BackgroundXY[i, 0] = (BackgroundXY[i, 0] - changeX * (0.04 + 0.09 * i) + Width * (1 + 0.2 * i)) % (Width * (1 + 0.2 * i));
                BackgroundXY[i, 1] = (BackgroundXY[i, 1] - changeY * (0.04 + 0.09 * i) + Height * (1 + 0.2 * i)) % (Height * (1 + 0.2 * i));
                Backgrounds[i] = new List<Tuple<double, double>>();
                for (int j = 0; j < 4; j++)
                {
                    Backgrounds[i].Add(Tuple.Create(BackgroundXY[i, 0] - Width * (1 + 0.2 * i) * (j / 2), BackgroundXY[i, 1] - Height * (1 + 0.2 * i) * (j % 2)));
                }
            }
            newFrame.Backgrounds = Backgrounds;
           

            string[] wells = parts[5].Split('|');

            for(int i = 0; i < wells.Length; i++)
            {
                string[] wellParts = wells[i].Split(',');
                newFrame.StableWells.Add(Tuple.Create(
                    Convert.ToDouble(wellParts[0]),
                    Convert.ToDouble(wellParts[1]),
                    Convert.ToInt32( wellParts[3])));
            }

            string[] unstableWells = parts[6].Split('|');

            for (int i = 0; i < unstableWells.Length; i++)
            {
                string[] wellParts = unstableWells[i].Split(',');
                newFrame.UnstableWells.Add(Tuple.Create(
                    Convert.ToDouble(wellParts[0]),
                    Convert.ToDouble(wellParts[1])));
            }

            string[] orbs = parts[7].Split('|');

            for (int i = 0; i < orbs.Length; i++)
            {
                string[] wellParts = orbs[i].Split(',');
                newFrame.Orbs.Add(Tuple.Create(
                    Convert.ToDouble(wellParts[0]),
                    Convert.ToDouble(wellParts[1]),
                    Convert.ToInt32(wellParts[3])));
            }


            string[] playerOrbs = parts[8].Split('|');

            for (int i = 0; i < playerOrbs.Length; i++)
                newFrame.PlayerOrbs.Add(Convert.ToInt32(playerOrbs[i]));

            string[] aiShips = parts[14].Split('|');
            for(int i = 0; i < aiShips.Length; i++)
            {
                string[] wellParts = aiShips[i].Split(',');
                newFrame.AIShips.Add(Tuple.Create(
                    Convert.ToDouble(wellParts[0]),
                    Convert.ToDouble(wellParts[1])));
            }

            string[] PlayerShips = parts[9].Split('|');
            for (int i = 0; i < PlayerShips.Length; i++)
            {
                string[] wellParts = PlayerShips[i].Split(',');
                newFrame.AIShips.Add(Tuple.Create(
                    Convert.ToDouble(wellParts[0]),
                    Convert.ToDouble(wellParts[1])));
            }

            newFrame.Seconds = Convert.ToInt32(parts[10]);
            newFrame.SecondsLeft = Convert.ToInt32(parts[11]);
            newFrame.Points = Convert.ToInt32(parts[12]);

            newFrame.HasGhostingPowerup = (parts[13] == "t");
            newFrame.HasNeutralizePowerup = (parts[14] == "t");
            newFrame.HasDestabilizePowerup = (parts[15] == "t");

            return newFrame;
        }
    }
}
