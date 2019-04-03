﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class HighScores
    {
        private List<HiScore> highScores;
        public List<HiScore> HighScoresList
        {
            get{return highScores;}
            set{highScores = value;}
        }
        
        public HighScores(List<HiScore> scoreList)
        {
            highScores = scoreList;
        }

        public HighScores() { }
        
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
        // Compares two high scores based on the score value. Used to sort the list of HiScore objects.
        // Accepts two HiScore objects to compare.
        // Returns an int denoting the order.
        public int CompareHighScores (HiScore a, HiScore b) {
            return 0; //TODO
        }
    }
    
    public class HiScore {
        public HiScore(string name, int score)
        {
            User = name;
            Score = score;
        }

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
    }
}
