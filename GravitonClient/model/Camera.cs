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

        public List<Tuple<double, double, int>> StableWells { get; set; }
        public List<Tuple<double, double>> UnstableWells { get; set; }
        public List<Tuple<double, double, int>> Orbs { get; set; }
        public List<int> PlayerOrbs { get; set; }
        public Tuple<double, double> PlayerShip { get; set; }
        public int SecondsLeft { get; set; }
        public int Seconds { get; set; }
        public int Score { get; set; }
        public bool IsOver { get; set; }

        public Camera(Game game)
        {
            ParentGame = game;
            Width = 1440;
            Height = 900;
            ScreenX = 1780.0;
            ScreenY = 2050.0;
        }

        //This method updates all of its properties to represent where everything should be on the screen.
        public void Render() //ai, ship 90x90   well 120x120   dswell 250x250    orbs 14x14
        {
            Seconds = ParentGame.Ticks / 50; 
            Score = ParentGame.Points;
            IsOver = ParentGame.IsOver;
            AdjustScreenForPlayer();
            double xc, yc;
            StableWells = new List<Tuple<double, double, int>>();
            SecondsLeft = 600;
            foreach (Well well in ParentGame.StableWells)
            {
                SecondsLeft = Math.Min(well.TicksLeft / 50, SecondsLeft);
                xc = well.Xcoor - ScreenX;
                yc = well.Ycoor - ScreenY;
                if (xc > -60 && xc < Width + 60 && yc > -60 && yc < Height + 60)
                    StableWells.Add(Tuple.Create(xc - 60, yc - 60, well.Orbs));
            }
            UnstableWells = new List<Tuple<double, double>>();
            foreach (Well well in ParentGame.UnstableWells)
            {
                xc = well.Xcoor - ScreenX;
                yc = well.Ycoor - ScreenY;
                if (xc > -125 && xc < Width + 125 && yc > -125 && yc < Height + 125)
                    UnstableWells.Add(Tuple.Create(xc - 125, yc - 125));
            }
            Orbs = new List<Tuple<double, double, int>>();
            foreach (Orb orb in ParentGame.Orbs)
            {
                xc = orb.Xcoor - ScreenX;
                yc = orb.Ycoor - ScreenY;
                if (xc > -7 && xc < Width + 7 && yc > -7 && yc < Height + 7)
                    Orbs.Add(Tuple.Create(xc - 7, yc - 7, orb.Color));
                
            }
            PlayerOrbs = new List<int>();
            foreach (int orb in ParentGame.Player.Orbs)
            {
                PlayerOrbs.Add(orb);
            }
        }

        //This method adjusts the screen so the player is never within 100 pixels of the edge.
        public void AdjustScreenForPlayer()
        {
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
            PlayerShip = Tuple.Create(xc - 25, yc - 25);
        }

    }
}
