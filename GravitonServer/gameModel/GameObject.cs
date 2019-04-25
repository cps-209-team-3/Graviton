//This file contains the GameObject class, which is the ancestor for all game objects, such as ships, wells, and orbs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonServer
{
    public abstract class GameObject
    {
        //Constructors
        public GameObject() { }
        public GameObject(double xcoor, double ycoor) {
            Xcoor = xcoor;
            Ycoor = ycoor;
        }
        //X and Y coordinate properties
        public double Xcoor { get; set; }
        public double Ycoor { get; set; }
        //Object type string
        public string Type { get; set; }
        
    }
}
