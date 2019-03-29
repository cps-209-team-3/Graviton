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
        public List<Tuple<double, double, int>> Wells { get; set; }
        public List<Tuple<double, double, int>> Orbs { get; set; }
        public List<Tuple<double, double, int>> UserOrbs { get; set; }
        public Tuple<double, double> UserShip { get; set; }
        public int Seconds { get; set; }
        public int Score { get; set; }

        public Camera(Game game)
        {
            
        }
        public void Render()
        {

        }
    }
}
