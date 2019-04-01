using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    abstract class Well : GameObject
    {
        public Well(double xcoor, double ycoor) : base(xcoor, ycoor) { }
        public Well() : base() { }
        public double Strength { get; set; }
        // public override abstract string Serialize();
        // public override abstract void Deserialize(string info);
    }
}
