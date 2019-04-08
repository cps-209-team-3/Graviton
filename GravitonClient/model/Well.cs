using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class Well : GameObject
    {
        public bool IsStable { get; set; }
        public int TicksLeft { get; set; }
        public double Strength { get; set; }
        public int Orbs { get; set; }
        public Well(double xcoor, double ycoor) : base(xcoor, ycoor)
        {
            IsStable = true;
            Strength = 100.0;
            TicksLeft = 4000;
            Orbs = 0;
        }

        public Well() { }

        public override string Serialize()
        {
            return $@"{{
    ""strength"":{Strength}
    ""xcoor"":{Xcoor},
    ""ycoor"":{Ycoor},
    {(IsStable? $"\"currentcolor\":{ Orbs }," : "" )}
    ""ticksleft"":{TicksLeft}
}}"; /* Colors are zero-based, 
            so the number of orbs it has is the color number it is seeking.*/
        }
        public override void Deserialize(string info)
        {
            // change the properties
            base.Deserialize(info);
            TicksLeft = Convert.ToInt32(JsonUtils.ExtractValue(info, "ticksleft"));
            Strength = Convert.ToInt32(JsonUtils.ExtractValue(info, "strength"));
            try
            {
                Orbs = Convert.ToInt32(JsonUtils.ExtractValue(info, "currentcolor"));
            }
            catch
            {
                IsStable = false;
            }
        }
    }
}
