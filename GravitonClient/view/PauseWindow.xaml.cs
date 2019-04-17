using System;
using System.Collections.Generic;
using System.IO;
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


namespace GravitonClient
{
    /// <summary>
    /// Interaction logic for PauseWindow.xaml
    /// </summary>
    /// 
    
    public partial class PauseWindow : Window
    {
        public const string SaveFileName = "..\\..\\Saved Games\\game1.json";

        private Game Game { get; set; }
        private GameWindow GameWindow { get; set; }
        private DateTime pauseStartTime;

        public PauseWindow(Game game, GameWindow gameWindow)
        {
            Game = game;
            GameWindow = gameWindow;
            pauseStartTime = DateTime.Now;
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Game.IsOver = true;
            Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Directory.GetCurrentDirectory(), SaveFileName )));
            GameLoader.Save(Game, SaveFileName);
            GameWindow.Close();
            Close();
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Game = GameLoader.Load(SaveFileName, false);
                GameWindow newWindow = new GameWindow(Game.IsCheat, Game);
                GameWindow.Close();
                newWindow.Show();
                Close();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Cannot find file.");
            }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Game.Timer.Start();
            GameWindow.PauseDuration += DateTime.Now - pauseStartTime;
        }
    }
}
