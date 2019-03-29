using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    abstract class GameObject
    {
        double Xcoor { get; set; }
        double Ycoor { get; set; }
        string Type { get; set; }
        public abstract string Serialize();

        public static GameObject Deserialize(string info)
        {
            return null;
        }
    }
}
