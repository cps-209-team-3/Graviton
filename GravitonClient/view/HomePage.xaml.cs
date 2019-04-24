//-----------------------------------------------------------
//File:   HomePage.xaml.cs
//Desc:   Counterpart for HomePage.xaml, contains logic 
//        for main menu view.
//----------------------------------------------------------- 

using System.Windows;
using System.Windows.Controls;

namespace GravitonClient
{
    //-----------------------------------------------------------
    //        This class contains the logic for the main menu.
    //        Most of the work is navigating to other pages.
    //----------------------------------------------------------- 
    public partial class HomePage : Page
    {
        //The window the page is displayed in.
        public Window Window { get; set; }

        public HomePage()
        {
            InitializeComponent();
            Window = Application.Current.MainWindow;
        }

        //Logic for when High Scores button is clicked. Navigates to the High Scores page.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void BtnHighScores_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HighScorePage(this));
        }

        //Logic for when Exit button is clicked. Exits the application.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        //Logic for when Play Singleplayer button is clicked. Navigates to the Play page.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PlayPage(this, Window));
        }

        //Logic for when Help button is clicked. Navigates to the Help page.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HelpPage(this));
        }

        //Logic for when About button is clicked. Navigates to the About page.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AboutPage(this));
        }

        //Logic for when Play Multiplayer button is clicked. Navigates to the Multiplayer Play page.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void MultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new NetworkPlayPage(this));
        }
    }
}
