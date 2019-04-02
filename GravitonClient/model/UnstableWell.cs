using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class UnstableWell : Well
    {
        public UnstableWell(double xcoor, double ycoor)
        {
            
        }

        public UnstableWell() : base() { }

        public override string Serialize()
        {
            return null;
        }
        public override void Deserialize(string info)
        {
            // change the properties
        }
    }
}
