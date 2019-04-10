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
            GameWindow g = new GameWindow(cheat);
            g.Show();
            g.Game.Username = txtBxUser.Text;
            App.Current.MainWindow.Hide();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Game g = GameLoader.Load(SaveFileName, true);
                GameWindow newWindow = new GameWindow(g.IsCheat, g);
                newWindow.Show();
                App.Current.MainWindow.Hide();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Cannot find file.");
            }
        }
    }
}
