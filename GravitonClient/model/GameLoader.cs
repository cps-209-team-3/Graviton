using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GravitonClient
{
    public class GameLoader

    {
        private static string Version = "0.0.1";

        public static Game Load(string filename, bool isCheatMode)
        {
            string json;
            try
            {
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    json = sr.ReadToEnd();
                }
            }
            catch
            {
                throw new ArgumentException("The file does not exist");
            }
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
            game.Player.ParentGame = game;
            game.Player.GamePowerup = new Powerup(game);
            foreach (string s in JsonUtils.GetObjectsInArray(JsonUtils.ExtractValue(json, "stablegravitywells")))
                game.StableWells.Add(GameObject.FromJsonFactory<Well>(s));
            foreach (string s in JsonUtils.GetObjectsInArray(JsonUtils.ExtractValue(json, "unstablegravitywells")))
                game.UnstableWells.Add(GameObject.FromJsonFactory<Well>(s));
            foreach (string s in JsonUtils.GetObjectsInArray(JsonUtils.ExtractValue(json, "orbs")))
                game.Orbs.Add(GameObject.FromJsonFactory<Orb>(s));
            foreach (string s in JsonUtils.GetObjectsInArray(JsonUtils.ExtractValue(json, "aiplayers")))
            {
                AIShip aIShip = GameObject.FromJsonFactory<AIShip>(s);
                game.AIShips.Add(aIShip);
                aIShip.ParentGame = game;
            }
            game.GameObjects.AddRange(game.Orbs);
            game.GameObjects.AddRange(game.UnstableWells);
            game.GameObjects.AddRange(game.StableWells);
            game.GameObjects.Add(game.Player);
            return game;
        }

        public static void Save(Game game, string filename)
        {
            if (!File.Exists(filename))
            {
                File.CreateText(filename).Close();

                using (var sr = new StreamWriter(File.OpenWrite(filename)))
                {
                    sr.Write($@"
{{
    ""version"":""{Version}"",
    ""username"":{game.Username},
    ""ticks"":{game.Ticks},
    ""humanplayer"":{game.Player.Serialize()},
    ""stablegravitywells"":"+JsonUtils.GameObjectsToJsonList(game.StableWells)+$@",
    ""unstablegravitywells"":{JsonUtils.GameObjectsToJsonList(game.UnstableWells)},      
    ""orbs"":{JsonUtils.GameObjectsToJsonList(game.Orbs)}
    ""aiplayers"":{JsonUtils.ToJsonList(game.AIShips)}
}}");
                }
            }
            else
            {
                File.Delete(filename);
                Save(game, filename);
            }
        }
    }
}
