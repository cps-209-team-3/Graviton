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
    /// Interaction logic for NetworkPlayPage.xaml
    /// </summary>
    public partial class NetworkPlayPage : Page
    {
        bool cheat;
        Page prevPage;
        private NetworkedGame game;

        public NetworkPlayPage(Page p)
        {
            prevPage = p;
            cheat = false;
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(prevPage);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtBxUser.Text != "" && txtBxUser.Text != null)
            {
                string uName = txtBxUser.Text;
                double w = App.Current.MainWindow.ActualWidth;
                double h = App.Current.MainWindow.ActualHeight;
                Task<NetworkedGame> myTask = Task.Run( ()=> UDPGameClient.JoinGame(uName, w, h));
                myTask.ContinueWith((game) => StartGame(game.Result), TaskScheduler.FromCurrentSynchronizationContext());
            }

            else
            {
                MessageBox.Show("You must enter a username!");
            }
        }

        private void StartGame(NetworkedGame game)
        {
            NetworkedGameWindow g = new NetworkedGameWindow(game);
            g.Show();
            App.Current.MainWindow.Hide();
        }
    }
}
