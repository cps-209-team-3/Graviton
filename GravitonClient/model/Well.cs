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
        public bool IsTrap { get; set; }
        public bool IsGhost { get; set; }
        public int TicksLeft { get; set; }
        public int ShockWaveRadius { get; set; }
        public int ShockWaveTicksLeft { get; set; }
        public double Strength { get; set; }
        public int Orbs { get; set; }
        private Random rand;

        public Well(double xcoor, double ycoor) : base(xcoor, ycoor)
        {
            rand = new Random();
            ShockWaveRadius = 0;
            ShockWaveTicksLeft = 0;
            IsStable = true;
            IsTrap = false;
            IsGhost = false;
            Strength = 300;
            TicksLeft = rand.Next(1500,4001);
            Orbs = 0;
        }

        //destroys all orbs within square root of ShockWaveRadius of the well
        public void ShockWave(Game game)
        {
            foreach (Orb orb in game.Orbs.ToList())
            {
                if (Math.Pow(Xcoor - orb.Xcoor, 2) + Math.Pow(Ycoor - orb.Ycoor, 2) < ShockWaveRadius)
                {
                    game.Orbs.Remove(orb);
                    game.GameObjects.Remove(orb);
                }
            }
        }

        public Well() { }

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
            }
        }
    }
}
