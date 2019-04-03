using System;
using System.Collections.Generic;
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
            return null;//TODO
        }
        
        // Writes the current HighScores object to a file.
        // Accepts a path to the file as a string.
        // Returns nothing.
        public void Save(string path)
        {
            
        }
        
        // Serializes the HighScores object to string.
        // Accepts nothing.
        // Returns a string.
        public string Serialize()
        {
            return null; //TODO
        }
        
        // Deserializes a HighScores object from a string.
        // Accepts a serialized object as a string.
        // Returns a HighScores object.
        public static HighScores Deserialize(string serialized)
        {
            return null; //TODO
        }
        
        // Compares two high scores based on the score value. Used to sort the list of HiScore objects.
        // Accepts two HiScore objects to compare.
        // Returns an int denoting the order.
        public int CompareHighScores (HiScore a, HiScore b) {
            return 0; //TODO
        }
    }
    
    public class HiScore {
        private string user;
        public string User
        {
            get{return user;}
            set{user = value;}
        }
        
        private int score;
        public int Score
        {
            get{return score;}
            set{score = value;}
        }

        public HiScore (string username, int score)
        {

        }
    }
}
