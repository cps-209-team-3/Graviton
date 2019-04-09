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

namespace GravitonClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool cheat;

        public MainWindow()
        {
            InitializeComponent();
            cheat = false;
        }

        private void BtnHighScores_Click(object sender, RoutedEventArgs e)
        {
            HighScoreWindow hs = new HighScoreWindow(this);
            hs.Show();
            Hide();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CheatButton_Click(object sender, RoutedEventArgs e)
        {
            cheat = !cheat;
            CheatBtn.Content = cheat ? "Cheatmode: On" : "Cheatmode: Off";
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            GameWindow g = new GameWindow(cheat, this);
            g.Show();
            g.Game.Username = txtBxUser.Text;
            Hide();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
