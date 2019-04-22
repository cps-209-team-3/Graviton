
namespace GravitonServer
{
    internal class GameStats
    {
        private HighScores HighScores;
        internal void SetHighScores(HighScores hs)
        {
            HighScores = hs;
        }
        

        public string Serialize()
        {
            return HighScores.Serialize();
        }
    }
}
