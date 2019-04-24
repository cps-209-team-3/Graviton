//-----------------------------------------------------------
//File:   HelpPage.xaml.cs
//Desc:   Counterpart for HelpPage.xaml, contains logic 
//        for help menu view.
//----------------------------------------------------------- 

using System.Windows;
using System.Windows.Controls;

namespace GravitonClient.view
{
    //-----------------------------------------------------------
    //        This class contains the logic for the help menu.
    //        It sets two text values and navigates back to the 
    //        page that called it.
    //----------------------------------------------------------- 
    public partial class HelpPage : Page
    {
        //The page that created this page.
        public Page ParentPage { get; set; }

        public HelpPage(Page parentPage)
        {
            ParentPage = parentPage;
            InitializeComponent();
            StoryTxt.Text = "It is the year 2739. You are a spaceship pilot for remote research station Z-23 in the gravity fields beyond the solar system. You are enjoying a quiet, uneventful ride on routine inspection until a warning light flashes. A dangerous gravity storm has been detected, and it is moving closer to the station! All available ships are to head into the tempest and attempt to stop the distortions before they grow too big! The station is a bit low on personnel at the moment, and you are the only pilot in the immediate area. You pull your ship around and head to confront the storm!";
            InstructTxt.Text = "You must pilot your spaceship through a field of gravity wells and try to prevent them from destabilizing into dangerous black holes. In order to calm the gravity wells, you must pick up energy orbs of various colors by flying over them and then fly over and place them into the wells with the same color. Depositing enough orbs into a gravity well will neutralize it and give you a powerup, but be wary! Wells destabilize into black holes after a certain amount of time and the time can only be reset by depositing an energy orb. If you get too close to a black hole it will suck you in and kill you! Black holes disappear after a while and periodically send out shockwaves that destroy nearby energy orbs, so move quickly. If you make it out of the match alive and with the most points, you win!";
        }

        //Logic for when Exit button is clicked. Returns to the menu that created the page.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(ParentPage);
        }
    }
}
