using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class HighScores
    {
        private Dictionary<string, int> hiScores;
        public Dictionary<string, int> HiScores
        {
            get{return hiScores;}
            set{hiScores = value;}
        }
        
        public void CheckNewScores(Game game)
        {
            
        }
        
        public void Serialize()
        {
            
        }
        
        public void Deserialize()
        {
            
        }
    }
}
