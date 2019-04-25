
namespace GravitonServer
{
    internal class GameStats
    {
        //HighScores object
        private HighScores HighScores;
        internal void SetHighScores(HighScores hs)
        {
            HighScores = hs;
        }
        
        //Serialize method
        public string Serialize()
        {
            return HighScores.Serialize();
        }
    }
}
