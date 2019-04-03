using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class Well : GameObject
    {
        public bool IsStable { get; set; }
        public int TicksLeft { get; set; }
        public double Strength { get; set; }
        public int Orbs { get; set; }
        public Well(double xcoor, double ycoor) : base(xcoor, ycoor)
        {
            IsStable = true;
            Strength = 5000.0;
            TicksLeft = 4000;
            Orbs = 0;
        }

        public Well() { }

        public override string Serialize()
        {
            return String.Format(@"
            {{
                ""xcoor"":{0},
                ""ycoor"":{1},
                ""currentcolor"":{2},
                ""ticksleft"":{3}
            }}
            ", Xcoor, Ycoor, Orbs, TicksLeft); /* Colors are zero-based, 
            so the number of orbs it has is the color number it is seeking.*/
        }
        public override void Deserialize(string info)
        {
            // change the properties
            
        }
    }
}
