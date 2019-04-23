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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public Window Window { get; set; }
        public HomePage()
        {
            InitializeComponent();
            Window = Application.Current.MainWindow;
        }

        private void BtnHighScores_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HighScorePage(this));
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }
        
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PlayPage(this, Window));
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HelpPage(this));
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AboutPage(this));
        }

        private void MultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new NetworkPlayPage(this));
        }
    }
}
