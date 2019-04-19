using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class NetworkedCameraFrame : CameraFrame
    {
        
        public List<Tuple<double, double>> OtherHumanShips; 
        public NetworkedCameraFrame() : base() { }
        public static NetworkedCameraFrame Deserialize(string data)
        {
            string[] parts = data.Split(' ');
            NetworkedCameraFrame newFrame = new NetworkedCameraFrame();
            newFrame.ScreenX = Convert.ToDouble(parts[0]);
            newFrame.ScreenX = Convert.ToDouble(parts[1]);

            newFrame.BackgroundXY[0, 0] = Convert.ToDouble(parts[2]);
            newFrame.BackgroundXY[0, 1] = Convert.ToDouble(parts[3]);
            newFrame.BackgroundXY[0, 2] = Convert.ToDouble(parts[4]);
            newFrame.BackgroundXY[0, 3] = Convert.ToDouble(parts[5]);
            newFrame.BackgroundXY[1, 0] = Convert.ToDouble(parts[6]);
            newFrame.BackgroundXY[1, 1] = Convert.ToDouble(parts[7]);
            newFrame.BackgroundXY[1, 2] = Convert.ToDouble(parts[8]);
            newFrame.BackgroundXY[1, 3] = Convert.ToDouble(parts[9]);

            string[] wells = parts[10].Split('|');

            for(int i = 0; i < wells.Length; i++)
            {
                string[] wellParts = wells[i].Split(',');
                newFrame.StableWells.Add(Tuple.Create(
                    Convert.ToDouble(wellParts[0]),
                    Convert.ToDouble(wellParts[1]),
                    Convert.ToInt32( wellParts[3])));
            }

            string[] unstableWells = parts[11].Split('|');

            for (int i = 0; i < unstableWells.Length; i++)
            {
                string[] wellParts = unstableWells[i].Split(',');
                newFrame.UnstableWells.Add(Tuple.Create(
                    Convert.ToDouble(wellParts[0]),
                    Convert.ToDouble(wellParts[1])));
            }

            string[] orbs = parts[12].Split('|');

            for (int i = 0; i < orbs.Length; i++)
            {
                string[] wellParts = orbs[i].Split(',');
                newFrame.Orbs.Add(Tuple.Create(
                    Convert.ToDouble(wellParts[0]),
                    Convert.ToDouble(wellParts[1]),
                    Convert.ToInt32(wellParts[3])));
            }


            string[] playerOrbs = parts[13].Split('|');

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


            return newFrame;
        }
    }
}
