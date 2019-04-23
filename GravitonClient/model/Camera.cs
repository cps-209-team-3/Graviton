using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class Camera
    {
        public Game ParentGame { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double ScreenX { get; set; }
        public double ScreenY { get; set; }
        public double[,] BackgroundXY { get; set; }
        public List<Tuple<double, double>>[] Backgrounds { get; set; }

        public Camera(Game game)
        {
            ParentGame = game;
            Width = 1440;
            Height = 900;
            ScreenX = 1780.0;
            ScreenY = 2050.0;
            Backgrounds = new List<Tuple<double, double>>[4];
            BackgroundXY = new double[4, 2];
        }

        public CameraFrame GetCameraFrame() {
            CameraFrame cameraFrame = new CameraFrame();
            cameraFrame.Seconds = ParentGame.Ticks / 50;
            cameraFrame.Points = ParentGame.Points;
            cameraFrame.IsOver = ParentGame.IsOver;
            AdjustScreenForPlayer(cameraFrame);
            cameraFrame.PlayerAngle = Math.Atan2(ParentGame.Player.SpeedY, ParentGame.Player.SpeedX) * 180 / Math.PI;

            double xc, yc;

            cameraFrame.StableWells = new List<Tuple<double, double, int>>();
            cameraFrame.SecondsLeft = 600;
            int currentWell = 0;
            foreach (Well well in ParentGame.StableWells)
            {
                cameraFrame.SecondsLeft = (int)Math.Min(well.TicksLeft / 31.25, cameraFrame.SecondsLeft);
                xc = well.Xcoor - ScreenX;
                yc = well.Ycoor - ScreenY;
                if (xc > -60 && xc < Width + 60 && yc > -60 && yc < Height + 60)
                {
                    cameraFrame.StableWells.Add(Tuple.Create(xc - 60, yc - 60, well.Orbs));
                    int val = currentWell;
                    cameraFrame.ScreenStables.Add(val);
                }
                ++currentWell;
            }

            cameraFrame.UnstableWells = new List<Tuple<double, double>>();
            foreach (Well well in ParentGame.UnstableWells)
            {
                xc = well.Xcoor - ScreenX;
                yc = well.Ycoor - ScreenY;
                if (xc > -125 && xc < Width + 125 && yc > -125 && yc < Height + 125)
                    cameraFrame.UnstableWells.Add(Tuple.Create(xc - 125, yc - 125));
            }

            cameraFrame.Orbs = new List<Tuple<double, double, int>>();
            foreach (Orb orb in ParentGame.Orbs)
            {
                xc = orb.Xcoor - ScreenX;
                yc = orb.Ycoor - ScreenY;
                if (xc > -7 && xc < Width + 7 && yc > -7 && yc < Height + 7)
                    cameraFrame.Orbs.Add(Tuple.Create(xc - 7, yc - 7, orb.Color));
            }

            cameraFrame.AIShips = new List<Tuple<double, double>>();
            foreach (AIShip ship in ParentGame.AIShips)
            {
                xc = ship.Xcoor - ScreenX;
                yc = ship.Ycoor - ScreenY;
                if (xc > -25 && xc < Width + 25 && yc > -25 && yc < Height + 25)
                    cameraFrame.AIShips.Add(Tuple.Create(xc - 25, yc - 25));
            }

            cameraFrame.PlayerOrbs = new List<int>();
            foreach (int orb in ParentGame.Player.Orbs)
            {
                cameraFrame.PlayerOrbs.Add(orb);
            }

            cameraFrame.HasDestabilizePowerup = ParentGame.Player.GamePowerup.CarryingDestabilize;
            cameraFrame.HasNeutralizePowerup = ParentGame.Player.GamePowerup.CarryingNeutralize;
            cameraFrame.HasGhostingPowerup = ParentGame.Player.GamePowerup.CarryingGhost;
            return cameraFrame;
        }
        
        //This method adjusts the screen so the player is never within 250 pixels of the edge.
        public void AdjustScreenForPlayer(CameraFrame cameraFrame)
        {
            double tempX = ScreenX;
            double tempY = ScreenY;
            double xc = ParentGame.Player.Xcoor - ScreenX;
            double yc = ParentGame.Player.Ycoor - ScreenY;
            if (xc < 250)
            {
                ScreenX += xc - 250;
                xc = 250;
            }
            else if (xc > Width - 250)
            {
                ScreenX += xc - (Width - 250);
                xc = Width - 250;
            }
            if (yc < 250)
            {
                ScreenY += yc - 250;
                yc = 250;
            }
            else if (yc > Height - 250)
            {
                ScreenY += yc - (Height - 250);
                yc = Height - 250;
            }
            cameraFrame.PlayerShip = Tuple.Create(xc - 25, yc - 25);
            CalculateBackgounds(ScreenX - tempX, ScreenY - tempY, cameraFrame);
        }

        public void CalculateBackgounds(double changeX, double changeY, CameraFrame cameraFrame)
        {
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
            cameraFrame.Backgrounds = Backgrounds;
            cameraFrame.BackgroundXY = BackgroundXY;
        }
    }
}
