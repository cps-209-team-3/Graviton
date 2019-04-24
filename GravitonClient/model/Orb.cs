//This file contains the Orb class which represents an orb in the game.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    //This class describes an orb in the game.
    public class Orb : GameObject
    {
        public int Color { get; set; }
        public Orb(double xcoor, double ycoor, int color) : base(xcoor, ycoor)
        { 
            Color = color;
            Type = "Orb";
        }
        public Orb() { }

        //This method serializes the instance and returns the string.
        public override string Serialize()
        {
            return String.Format(@"
            {{
                 ""xcoor"":{0},
                 ""ycoor"":{1},
                 ""color"":{2}
            }}", Xcoor, Ycoor, Color);
        }

        //This method deserializes a string and puts the information into the instance itself.
        public override void Deserialize(string info)
        {
            base.Deserialize(info); //sets xcoor and ycoor
            Type = "Orb";
            Color = Convert.ToInt32(JsonUtils.ExtractValue(info, "color"));
        }

    }
}
