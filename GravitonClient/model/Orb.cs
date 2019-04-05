using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class Orb : GameObject
    {
        public int Color { get; set; }
        public Orb(double xcoor, double ycoor, int color) : base(xcoor, ycoor)
        { 
            Color = color;
        }
        public Orb() { }
        public override string Serialize()
        {
            return String.Format(@"
            {{
                 ""xcoor"":{0},
                 ""ycoor"":{1},
                 ""color"":{2}
            }}
            ", Xcoor, Ycoor, Color);
        }
        public override void Deserialize(string info)
        {
            base.Deserialize(info); //sets xcoor and ycoor
            Type = "Orb";
            Color = Convert.ToInt32(JsonUtils.ExtractValue(info, "color"));
        }

    }
}
