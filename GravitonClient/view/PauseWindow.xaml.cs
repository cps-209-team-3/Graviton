using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GravitonClient
{
    /// <summary>
    /// Interaction logic for PauseWindow.xaml
    /// </summary>
    public partial class PauseWindow : Window
    {
        private Game Game { get; set; }

        public PauseWindow(Game game)
        {
            Game = game;
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            Game.Timer.Start();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Game.GameOver();
            Close();
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            Game = GameLoader.Load("C:\temp\\game.json", true);
            GameWindow newWindow = new GameWindow(Game);
            newWindow.Show();
            Close();
        }
    }
}
