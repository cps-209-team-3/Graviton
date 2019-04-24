//-----------------------------------------------------------
//File:   AboutPage.xaml.cs
//Desc:   Counterpart for AboutPage.xaml, contains logic 
//        for credits menu view.
//----------------------------------------------------------- 

using System.Windows;
using System.Windows.Controls;

namespace GravitonClient
{
    //-----------------------------------------------------------
    //        This class contains the logic for the credits menu.
    //        Its only function is to initialize the page and 
    //        navigate back to the page that created it.
    //----------------------------------------------------------- 
    public partial class AboutPage : Page
    {
        //The page that created this page.
        public Page ParentPage { get; set; }

        public AboutPage(Page parent)
        {
            ParentPage = parent;
            InitializeComponent();
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
