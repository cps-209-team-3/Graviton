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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GravitonClient.view
{
    /// <summary>
    /// Interaction logic for PlayPage.xaml
    /// </summary>
    public partial class PlayPage : Page
    {
        bool cheat;
        Page prevPage;

        public const string SaveFileName = "..\\..\\Saved Games\\game1.json";

        public PlayPage(Page p)
        {
            prevPage = p;
            cheat = false;
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(prevPage);
        }

        private void CheatButton_Click(object sender, RoutedEventArgs e)
        {
            cheat = !cheat;
            CheatBtn.Content = cheat ? "Cheatmode: On" : "Cheatmode: Off";
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtBxUser.Text != "" && txtBxUser.Text != null)
            {
                GameWindow g = new GameWindow(cheat);
                g.Show();
                g.Game.Username = txtBxUser.Text;
                if (DifficultyBx.Text == "Easy")
                {
                    g.Game.WellSpawnFreq = 400;
                    g.Game.WellDestabFreq = 3000;
                }
                else if (DifficultyBx.Text == "Normal")
                {
                    g.Game.WellSpawnFreq = 300;
                    g.Game.WellDestabFreq = 2000;
                }
                else if (DifficultyBx.Text == "Hard")
                {
                    g.Game.WellSpawnFreq = 200;
                    g.Game.WellDestabFreq = 1000;
                }
                App.Current.MainWindow.Hide();
            }

            else
            {
                MessageBox.Show("You must enter a username!");
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtBxUser.Text != "" && txtBxUser.Text != null)
            {
                try
                {
                    Game g = GameLoader.Load(SaveFileName, false);
                    GameWindow newWindow = new GameWindow(g.IsCheat, g);
                    newWindow.Show();
                    App.Current.MainWindow.Hide();
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Cannot find file.");
                }
            }

            else
            {
                MessageBox.Show("You must enter a username!");
            }
        }
    }
}
