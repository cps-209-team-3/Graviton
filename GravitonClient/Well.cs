using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    abstract class Well : GameObject
    {
        public double Strength { get; set; }
        public double Xcoor { get; set; }
        public double Ycoor { get; set; }

        public override abstract string Serialize();
    }
}
