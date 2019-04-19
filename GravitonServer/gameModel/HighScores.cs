using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class HighScores
    {

        private List<HiScore> hiScores;
        public List<HiScore> HiScores
        {
            get{return hiScores;}
            set{hiScores = value;}
        }
        
        public HighScores(List<HiScore> scoreList)
        {
            hiScores = scoreList;
        }

        public HighScores()
        {

        }

        
        // Checks a game to see if any scores should be included in the list of highscores.
        // Accepts a reference to a Game instance.
        // Returns nothing.
        public void CheckNewScores(Game game)
        {
            if (hiScores.Count == 10)
            {
                if (game.Points > hiScores[9].Score)
                    AddNewScore(game.Username, game.Points);
            }
            else
            {
                AddNewScore(game.Username, game.Points);
            }
        }
        
        // Adds a new score to the high score list (also deletes lowest one).
        // Accepts a username as a string and a score as an integer.
        // Returns nothing.
        public void AddNewScore(string username, int score)
        {
            if (hiScores.Count >= 10)
            {
                hiScores.RemoveAt(9);
                hiScores.Add(new HiScore(username, score));
                hiScores.Sort(CompareHighScores);
            }
            else
            {
                hiScores.Add(new HiScore(username, score));
                hiScores.Sort(CompareHighScores);
            }
        }
        
        // Creates a HighScores object from a file.
        // Accepts a path to the file as a string.
        // Returns a HighScores object.
        public static HighScores Load(string path)
        {
            HighScores loadScores;
            using (StreamReader rd = new StreamReader(path))
            {
                loadScores = Deserialize(rd.ReadLine());
                loadScores.hiScores.Sort(CompareHighScores);
            }
            return loadScores;
        }
        
        // Writes the current HighScores object to a file.
        // Accepts a path to the file as a string.
        // Returns nothing.
        public void Save(string path)
        {
            hiScores.Sort(CompareHighScores);
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine(Serialize());
            }
        }
        
        // Serializes the HighScores object to string.
        // Accepts nothing.
        // Returns a string.
        public string Serialize()
        {
            string serialized = "";
            for (int i = 0; i < hiScores.Count; ++i)
            {
                serialized += string.Format("{0}%{1}%", hiScores[i].User, hiScores[i].Score);
            }
            return serialized;
        }
        
        // Deserializes a HighScores object from a string.
        // Accepts a serialized object as a string.
        // Returns a HighScores object.
        public static HighScores Deserialize(string serialized)
        {
            List<HiScore> loadScores = new List<HiScore>();
            if (serialized != "" && serialized != null)
            {
                string[] split = serialized.Split(new char[] { '%' });
                for (int i = 0; i < split.Length / 2; ++i)
                {
                    if (split[i * 2] != "")
                        loadScores.Add(new HiScore(split[i * 2], Convert.ToInt32(split[i * 2 + 1])));

                    else
                        break;
                }
            }
            return new HighScores(loadScores);
        }
        
        // Compares two high scores based on the score value. Used to sort the list of HiScore objects.
        // Accepts two HiScore objects to compare.
        // Returns an int denoting the order.
        public static int CompareHighScores (HiScore a, HiScore b) {
            if (a.Score > b.Score)
                return -1;
            else if (a.Score < b.Score)
                return 1;
            else
                return 0;
        }

        public override bool Equals(object obj)
        {
            HighScores newScore = obj as HighScores;
            if (newScore == null || newScore.hiScores.Count != hiScores.Count)
                return false;

            else
            {
                for (int i = 0; i < 10; ++i)
                {
                    if (!hiScores[i].Equals(newScore.hiScores[i]))
                        return false;
                }
                return true;
            }
        }
    }
    
    public class HiScore : IEquatable<HiScore> {
        private string user;
        public string User
        {
            get { return user; }
            set { user = value; }
        }

        private int score;
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public HiScore(string name, int score)
        {
            user = name;
            this.score = score;
        }

        public bool Equals(HiScore score)
        {
            if (score == null)
                return false;

            else
            {
                if (this.score == score.score && this.user.Equals(score.user))
                    return true;
                else
                    return false;
            }
        }
    }
}
