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
        public void Render() //TODO adjust orbs
        {
            Seconds = ParentGame.Ticks / 50; //ai, ship 90x90   well 120x120   dswell 250x250    orbs 14x14
            Score = ParentGame.Points;
            AdjustScreenForPlayer();
            double xc, yc;
            StableWells = new List<Tuple<double, double, int>>();
            SecondsLeft = 600;
            foreach (Well well in ParentGame.StableWells)
            {
                SecondsLeft = Math.Min(well.TicksLeft / 50, SecondsLeft);
                xc = well.Xcoor - ScreenX;
                yc = well.Ycoor - ScreenY;
                if (xc > -60 && xc < 1500 && yc > -60 && yc < 960)
                    StableWells.Add(Tuple.Create(xc - 60, yc - 60, well.Orbs));
            }
            UnstableWells = new List<Tuple<double, double>>();
            foreach (Well well in ParentGame.UnstableWells)
            {
                xc = well.Xcoor - ScreenX;
                yc = well.Ycoor - ScreenY;
                if (xc > -125 && xc < 1565 && yc > -125 && yc < 1025)
                    UnstableWells.Add(Tuple.Create(xc - 125, yc - 125));
            }
            Orbs = new List<Tuple<double, double, int>>();
            foreach (Orb orb in ParentGame.Orbs)
            {
                xc = orb.Xcoor - ScreenX;
                yc = orb.Ycoor - ScreenY;
                if (xc > -7 && xc < 1447 && yc > -7 && yc < 907)
                    StableWells.Add(Tuple.Create(xc - 7, yc - 7, orb.Color));
            }
            PlayerOrbs = new List<int>();
            foreach (int orb in ParentGame.Player.Orbs)
            {
                PlayerOrbs.Add(orb);
            }
        }

        public void AdjustScreenForPlayer()
        {
            double xc = ParentGame.Player.Xcoor - ScreenX;
            double yc = ParentGame.Player.Ycoor - ScreenY;
            if (xc < 100.0)
            {
                ScreenX += xc - 100.0;
                xc = 100.0;
            }
            else if (xc > 1340.0)
            {
                ScreenX += xc - 1340.0;
                xc = 1340.0;
            }
            if (yc < 100.0)
            {
                ScreenY += yc - 100.0;
                yc = 100.0;
            }
            else if (yc > 800.0)
            {
                ScreenY += yc - 800.0;
                yc = 800.0;
            }
            PlayerShip = Tuple.Create(xc - 45, yc - 45);
        }

    }
}
