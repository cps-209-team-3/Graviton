//This file contains the Well class which represents a gravity well in the game.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    //This class describes a gravity well in the game.
    public class Well : GameObject
    {
        //This is a bool whether the well is stable or not
        public bool IsStable { get; set; }
        //This is a bool whether the well is a trap
        public bool IsTrap { get; set; }
        //This is a bool whether the well is ghosted
        public bool IsGhost { get; set; }
        //A reference to the owner ship
        public Ship Owner { get; set; }
        //How many ticks left until destabilization or an unstable well disappearing
        public int TicksLeft { get; set; }
        //How strong a well's gravity is
        public double Strength { get; set; }
        //How many orbs the well currently has
        public int Orbs { get; set; }
        //A reference to a shockwave emitted by the well
        public Shockwave ShockWave { get; set; }
        //A Random object
        private Random rand;

        public Well(double xcoor, double ycoor) : base(xcoor, ycoor)
        {
            rand = new Random();
            IsStable = true;
            IsTrap = false;
            IsGhost = false;
            Owner = null;
            Strength = 300;
            TicksLeft = rand.Next(1500,4001);
            Orbs = 0;
        }

        public Well() { }

        //This method serializes the instance and returns the string.
        public override string Serialize()
        {
            return $@"{{
    ""strength"":{Strength},
    ""xcoor"":{Xcoor},
    ""ycoor"":{Ycoor},
    {(IsStable? $"\"currentcolor\":{ Orbs }," : "" )}
    ""ticksleft"":{TicksLeft}
}}"; /* Colors are zero-based, 
            so the number of orbs it has is the color number it is seeking.*/
        }

        //This method deserializes a string and puts the information into the instance itself.
        public override void Deserialize(string info)
        {
            // change the properties
            base.Deserialize(info);
            TicksLeft = Convert.ToInt32(JsonUtils.ExtractValue(info, "ticksleft"));
            Strength = Convert.ToInt32(JsonUtils.ExtractValue(info, "strength"));
            try
            {
                Orbs = Convert.ToInt32(JsonUtils.ExtractValue(info, "currentcolor"));
                IsStable = true;
            }
            catch
            {
                IsStable = false;
                ShockWave = new Shockwave(this);
            }
        }
    }
}
