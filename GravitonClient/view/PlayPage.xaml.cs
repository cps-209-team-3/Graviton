//-----------------------------------------------------------
//File:   PlayPage.xaml.cs
//Desc:   Counterpart for PlayPage.xaml, contains logic 
//        for game configuration menu view.
//----------------------------------------------------------- 

using System;
using System.Windows;
using System.Windows.Controls;

namespace GravitonClient
{
    //-----------------------------------------------------------
    //        This class contains the logic for the game config 
    //        menu. This configures and launches the game.
    //----------------------------------------------------------- 
    public partial class PlayPage : Page
    {
        //Determines if cheatmode is activated.
        bool cheat;
        //The page that created this page.
        Page prevPage;
        //The window this page is displayed in.
        Window window;

        //The path of the game save file.
        public const string SaveFileName = "..\\..\\Saved Games\\game1.json";

        public PlayPage(Page p, Window w)
        {
            prevPage = p;
            window = w;
            cheat = false;
            InitializeComponent();
        }

        //Logic for when Exit button is clicked. Returns to the page that created this page.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(prevPage);
        }

        //Toggles cheatmode and the button text.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void CheatButton_Click(object sender, RoutedEventArgs e)
        {
            cheat = !cheat;
            CheatBtn.Content = cheat ? "Cheatmode: On" : "Cheatmode: Off";
        }

        //Creates a new game page and starts the game.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtBxUser.Text != "" && txtBxUser.Text != null)
            {
                GamePage g = new GamePage(cheat, this, window);
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
                this.NavigationService.Navigate(g);
            }

            else
            {
                MessageBox.Show("You must enter a username!");
            }
        }

        //Loads a game from the save file and starts it.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtBxUser.Text != "" && txtBxUser.Text != null)
            {
                try
                {
                    Game g = GameLoader.Load(SaveFileName, false);
                    GamePage newWindow = new GamePage(g.IsCheat, g, this, window);
                    for (int i = 0; i < g.StableWells.Count; ++i)
                    {
                        newWindow.UpdateAnimation(this, new AnimationEventArgs(false, AnimationType.Stable, g.StableWells.Count, 0, 0));
                    }

                    for (int i = 0; i < g.UnstableWells.Count; ++i)
                    {
                        newWindow.UpdateAnimation(this, new AnimationEventArgs(false, AnimationType.Unstable, g.StableWells.Count, 0, 0));
                    }
                    this.NavigationService.Navigate(newWindow);
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
