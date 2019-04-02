using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GravitonClient
{
    class GameLoader
    {
        public static Game Load(string filename)
        {
            StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open));
            throw new NotImplementedException();
        }

        public static void Save(Game game1, string v)
        {
            throw new NotImplementedException();
        }
    }
}
