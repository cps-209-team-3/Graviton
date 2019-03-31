using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    abstract class GameObject
    {
        public double Xcoor { get; set; }
        public double Ycoor { get; set; }
        public string Type { get; set; }
        public abstract string Serialize();
        public abstract void Deserialize(string info);
    }
}
