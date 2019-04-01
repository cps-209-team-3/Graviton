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
        
        public HighScores(Dictionary<string, int> scoreDict)
        {
            hiScores = scoreDict;
        }
        
        // Checks a game to see if any scores should be included in the list of highscores.
        // Accepts a reference to a Game instance.
        // Returns nothing.
        public void CheckNewScores(Game game)
        {
            
        }
        
        // Adds a new score to the high score list (also deletes lowest one).
        // Accepts a username as a string and a score as an integer.
        // Returns nothing.
        public void AddNewScore(string username, int score)
        {
            
        }
        
        // Creates a HighScores object from a file.
        // Accepts a path to the file as a string.
        // Returns a HighScores object.
        public static HighScores Load(string path)
        {
            return null;
        }
        
        // Writes the current HighScores object to a file.
        // Accepts a path to the file as a string.
        // Returns nothing.
        public static HighScores Save(string path)
        {
            return null;
        }
        
        // Serializes the HighScores object to string.
        // Accepts nothing.
        // Returns a string.
        public string Serialize()
        {
            return null;
        }
        
        // Deserializes a HighScores object from a string.
        // Accepts a serialized object as a string.
        // Returns a HighScores object.
        public static HighScores Deserialize(string serialized)
        {
            return null;
        }
    }
}
