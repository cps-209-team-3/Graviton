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
        private static string Version = "0.0.1";

        public static Game Load(string filename, bool isCheatMode)
        {
            string json = new StreamReader(File.OpenRead(filename)).ReadToEnd();
            string version = JsonUtils.ExtractValue(json, "version");
            if (version != '"' + Version + '"')
                throw new FormatException("Wrong version of saved game file: "+ version);
            var game = new Game(isCheatMode);
            game.StableWells = new List<Well>();
            game.UnstableWells = new List<Well>();
            game.Orbs = new List<Orb>();


            game.Username = JsonUtils.ExtractValue(json, "username");
            game.Ticks = Convert.ToInt32(JsonUtils.ExtractValue(json, "ticks"));
            game.Player = GameObject.FromJsonFactory<Ship>(JsonUtils.ExtractValue(json, "humanplayer"));
            foreach (string s in JsonUtils.GetObjectsInArray(JsonUtils.ExtractValue(json, "stablegravitywells")))
                game.StableWells.Add(GameObject.FromJsonFactory<Well>(s));
            foreach (string s in JsonUtils.GetObjectsInArray(JsonUtils.ExtractValue(json, "unstablegravitywells")))
                game.UnstableWells.Add(GameObject.FromJsonFactory<Well>(s));
            foreach (string s in JsonUtils.GetObjectsInArray(JsonUtils.ExtractValue(json, "orbs")))
                game.Orbs.Add(GameObject.FromJsonFactory<Orb>(s));
            return game;
        }

        public static void Save(Game game, string filename)
        {
            using (var sr = new StreamWriter(File.OpenWrite(filename)))
            {
                sr.Write(String.Format(@"
{{
    ""version"":""{0}"",
    ""username"":{1},
    ""ticks"":{2},
    ""humanplayer"":{3}
    ""stablegravitywells"":{4},
    ""unstablegravitywells"":{5},      
    ""orbs"":{6}
}}",Version, game.Username, game.Ticks, game.Player.Serialize(), 
                JsonUtils.ToJsonList(game.StableWells), JsonUtils.ToJsonList(game.UnstableWells),
                JsonUtils.ToJsonList(game.Orbs)));
            }
        }
    }
}
