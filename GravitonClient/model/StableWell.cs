using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class StableWell : Well
    {
        public int TicksLeft { get; set; }
        public int Orbs { get; set; }
        public StableWell(double xcoor, double ycoor) : base(xcoor, ycoor)
        {
            Orbs = 0;
        }

        public StableWell() { }

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
