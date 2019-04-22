using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonServer
{
    public abstract class GameObject
    {

        public GameObject() { }
        public GameObject(double xcoor, double ycoor) {
            Xcoor = xcoor;
            Ycoor = ycoor;
        }
        public double Xcoor { get; set; }
        public double Ycoor { get; set; }
        public string Type { get; set; }
        
    }
}
