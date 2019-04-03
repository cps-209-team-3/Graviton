using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class Camera
    {
        public Game ParentGame { get; set; }
        public double ScreenX { get; set; }
        public double ScreenY { get; set; } 
        public List<Tuple<double, double, int>> StableWells { get; set; }
        public List<Tuple<double, double>> UnstableWells { get; set; }
        public List<Tuple<double, double, int>> Orbs { get; set; }
        public List<int> PlayerOrbs { get; set; }
        public Tuple<double, double> PlayerShip { get; set; }
        public int SecondsLeft { get; set; }
        public int Seconds { get; set; }
        public int Score { get; set; }

        public Camera(Game game)
        {
            ParentGame = game;
            ScreenX = 1780.0;
            ScreenY = 2050.0;
        }
        public void Render() //TODO adjust everything for image width and height
        {
            Seconds = ParentGame.Ticks / 60;
            Score = ParentGame.Points;
            AdjustScreenForPlayer();
            double xc, yc;
            StableWells = new List<Tuple<double, double, int>>();
            SecondsLeft = 600;
            foreach (Well well in ParentGame.StableWells)
            {
                SecondsLeft = Math.Min(well.TicksLeft / 60, SecondsLeft);
                xc = well.Xcoor - ScreenX;
                yc = well.Ycoor - ScreenY;
                if (xc > 0 && xc < 1440 && yc > 0 && yc < 900)
                    StableWells.Add(Tuple.Create(xc, yc, well.Orbs));
            }
            UnstableWells = new List<Tuple<double, double>>();
            foreach (Well well in ParentGame.UnstableWells)
            {
                xc = well.Xcoor - ScreenX;
                yc = well.Ycoor - ScreenY;
                if (xc > 0 && xc < 1440 && yc > 0 && yc < 900)
                    UnstableWells.Add(Tuple.Create(xc, yc));
            }
            Orbs = new List<Tuple<double, double, int>>();
            foreach (Orb orb in ParentGame.Orbs)
            {
                xc = orb.Xcoor - ScreenX;
                yc = orb.Ycoor - ScreenY;
                if (xc > 0 && xc < 1440 && yc > 0 && yc < 900)
                    StableWells.Add(Tuple.Create(xc, yc, orb.Color));
            }
            PlayerOrbs = new List<int>();
            foreach (Orb orb in ParentGame.Player.Orbs)
            {
                PlayerOrbs.Add(orb.Color);
            }
        }

        public void AdjustScreenForPlayer()
        {
            double xc = ParentGame.Player.Xcoor - ScreenX;
            double yc = ParentGame.Player.Ycoor - ScreenY;
            if (xc < 50.0)
            {
                ScreenX += xc - 50.0;
                xc = 50.0;
            }
            else if (xc > 1390.0)
            {
                ScreenX += xc - 1390.0;
                xc = 1390.0;
            }
            if (yc < 50.0)
            {
                ScreenY += yc - 50.0;
                yc = 50.0;
            }
            else if (yc > 850.0)
            {
                ScreenY += yc - 850.0;
                yc = 850.0;
            }
            PlayerShip = Tuple.Create(xc, yc);
        }

    }
}
