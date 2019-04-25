//This file contains the NetworkedCameraFrame class which contains the info for one frame in the game
using System;
using System.Collections.Generic;

namespace GravitonClient
{
    //This class contains the info for one frame in the game and does backgrounds and such.
    public class NetworkedCameraFrame : CameraFrame
    {

        //This array holds the info for background coordinates
        private static double[,] BackgroundXY = new double[4, 2];

        //This is the screen width
        public static int Width
        {
            get;
            set;
        }
        //This is the screen height
        public static int Height
        {
            get; set;
        }

        //This is a list of other human ships to display
        public List<Tuple<double, double, string>> OtherHumanShips = new List<Tuple<double, double, string>>(); 
        public NetworkedCameraFrame() : base() { }
        //This method implements the logic to deserialize a camera frame string and return a NetworkedCameraFrame object.
        public static NetworkedCameraFrame Deserialize(string data)
        {
            string[] parts = data.Split(' ');
            NetworkedCameraFrame newFrame = new NetworkedCameraFrame();
            newFrame.ScreenX = Convert.ToDouble(parts[0]);
            newFrame.ScreenY = Convert.ToDouble(parts[1]);

            newFrame.PlayerShip = Tuple.Create<double, double>(newFrame.ScreenX, newFrame.ScreenY);

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
           

            string[] wells = parts[4].Split('|');

            if (wells.Length > 0)
            {
                for (int i = 0; i < wells.Length; i++)
                {
                    string[] wellParts = wells[i].Split(',');
                    if (wellParts.Length == 3) 
                        newFrame.StableWells.Add(Tuple.Create(
                            Convert.ToDouble(wellParts[0]),
                            Convert.ToDouble(wellParts[1]),
                            Convert.ToInt32(wellParts[2])));
                }
            }

            string[] unstableWells = parts[5].Split('|');

            if(unstableWells.Length > 0)
            for (int i = 0; i < unstableWells.Length; i++)
            {
                string[] wellParts = unstableWells[i].Split(',');
                if(wellParts.Length ==2)
                newFrame.UnstableWells.Add(Tuple.Create(
                    Convert.ToDouble(wellParts[0]),
                    Convert.ToDouble(wellParts[1])));
            }

            string[] orbs = parts[6].Split('|');
            if(orbs.Length > 0)
                for (int i = 0; i < orbs.Length; i++)
                {
                    string[] wellParts = orbs[i].Split(',');
                        if (wellParts.Length == 3)
                            newFrame.Orbs.Add(Tuple.Create(
                                Convert.ToDouble(wellParts[0]),
                                Convert.ToDouble(wellParts[1]),
                                Convert.ToInt32(wellParts[2])));
                }


            string[] playerOrbs = parts[7].Split('|');

            for (int i = 0; i < playerOrbs.Length; i++)
                if(!string.IsNullOrEmpty(playerOrbs[i]))
                    newFrame.PlayerOrbs.Add(Convert.ToInt32(playerOrbs[i]));

            string[] aiShips = parts[8].Split('|');
            for(int i = 0; i < aiShips.Length; i++)
            {
                string[] wellParts = aiShips[i].Split(',');
                if (wellParts.Length == 2)
                    newFrame.AIShips.Add(Tuple.Create(
                    Convert.ToDouble(wellParts[0]),
                    Convert.ToDouble(wellParts[1])));
            }

            string[] PlayerShips = parts[9].Split('|');
            for (int i = 0; i < PlayerShips.Length; i++)
            {
                string[] wellParts = PlayerShips[i].Split(',');
                if (wellParts.Length == 3)
                {
                    newFrame.OtherHumanShips.Add(Tuple.Create(
                    Convert.ToDouble(wellParts[0]),
                    Convert.ToDouble(wellParts[1]),
                    wellParts[2]));
                }
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
