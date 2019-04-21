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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace GravitonClient.view
{
    /// <summary>
    /// Interaction logic for PausePage.xaml
    /// </summary>
    public partial class PausePage : Page
    {
        public const string SaveFileName = "..\\..\\Saved Games\\game1.json";

        private Game Game { get; set; }
        private GamePage GamePage { get; set; }
        public Window Window { get; set; }
        private DateTime pauseStartTime;

        public PausePage(Game game, GamePage gamePage, Window w)
        {
            Game = game;
            GamePage = gamePage;
            Window = w;
            pauseStartTime = DateTime.Now;
            InitializeComponent();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                LeavePage();
            }
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            LeavePage();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Game.IsOver = true;
            Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Directory.GetCurrentDirectory(), SaveFileName)));
            GameLoader.Save(Game, SaveFileName);
            this.NavigationService.Navigate(GamePage.ParentPage);
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Game = GameLoader.Load(SaveFileName, false);
                GamePage newWindow = new GamePage(Game.IsCheat, Game, GamePage.ParentPage, Window);
                this.NavigationService.Navigate(newWindow);
                Game.Timer.Start();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Cannot find file.");
            }

        }

        private void LeavePage()
        {
            Game.Timer.Start();
            GamePage.PauseDuration += DateTime.Now - pauseStartTime;
            this.NavigationService.Navigate(GamePage);
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HelpPage(this));//, Window));
        }
    }
}
