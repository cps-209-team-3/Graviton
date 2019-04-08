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
using System.Windows.Shapes;

namespace GravitonClient
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        List<Image> wellDict;
        List<Image> destableDict;
        List<Image> orbDict;
        Image ship;


        List<BitmapImage> wellImages;
        BitmapImage destabilizedImage;
        List<BitmapImage> orbImages;
        BitmapImage shipImage;

        private Game Game { get; set; }

        Window parentWindow;

        public GameWindow(bool cheat)
        {
            wellDict = new List<Image>();
            destableDict = new List<Image>();
            orbDict = new List<Image>();
            ship = new Image();





            string parentDir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\");

            wellImages = new List<BitmapImage>();
            string[] imagePaths = new string[6] { "Assets\\Images/WellBasic1.png", "Assets\\Images/WellOrange.png", "Assets\\Images/WellYellow.png", "Assets\\Images/WellGreen.png", "Assets\\Images/WellBlue.png", "Assets\\Images/WellPurple.png" };
            for (int i = 0; i < 6; ++i)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(System.IO.Path.Combine(parentDir, imagePaths[i]));
                img.EndInit();
                wellImages.Add(img);
            }

            destabilizedImage = new BitmapImage();
            destabilizedImage.BeginInit();
            destabilizedImage.UriSource = new Uri(System.IO.Path.Combine(parentDir, "Assets\\Images/destabilized1.png"));
            destabilizedImage.EndInit();
            
            orbImages = new List<BitmapImage>();
            imagePaths = new string[6] { "Assets\\Images/OrbRed.png", "Assets\\Images/OrbOrange.png", "Assets\\Images/OrbYellow.png", "Assets\\Images/OrbGreen.png", "Assets\\Images/OrbBlue.png", "Assets\\Images/OrbPurple.png" };
            for (int i = 0; i < 6; ++i)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(System.IO.Path.Combine(parentDir, imagePaths[i]));
                img.EndInit();
                orbImages.Add(img);
            }

            shipImage = new BitmapImage();
            shipImage.BeginInit();
            shipImage.UriSource = new Uri(System.IO.Path.Combine(parentDir, "Assets\\Images/Ship1.png"));
            shipImage.EndInit();

            //----------------------------------
            ship.Source = shipImage;
            ship.Width = 20;
            //----------------------------------



            InitializeComponent();
            this.KeyDown += Window_KeyDown;
            this.KeyUp += Window_KeyUp;

            Game = new Game(cheat);
            Game.GameUpdatedEvent += Render;
            Game.Initialize();
        }

        public GameWindow(bool cheat, Window parentWindow)
        {
            wellDict = new List<Image>();
            destableDict = new List<Image>();
            orbDict = new List<Image>();
            ship = new Image();




            string parentDir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\");

            wellImages = new List<BitmapImage>();
            string[] imagePaths = new string[6] { "Assets\\Images/WellBasic1.png", "Assets\\Images/WellOrange.png", "Assets\\Images/WellYellow.png", "Assets\\Images/WellGreen.png", "Assets\\Images/WellBlue.png", "Assets\\Images/WellPurple.png" };
            for (int i = 0; i < 6; ++i)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(System.IO.Path.Combine(parentDir, imagePaths[i]));
                img.EndInit();
                wellImages.Add(img);
            }

            destabilizedImage = new BitmapImage();
            destabilizedImage.BeginInit();
            destabilizedImage.UriSource = new Uri(System.IO.Path.Combine(parentDir, "Assets\\Images/destabilized1.png"));
            destabilizedImage.EndInit();

            orbImages = new List<BitmapImage>();
            imagePaths = new string[6] { "Assets\\Images/OrbRed.png", "Assets\\Images/OrbOrange.png", "Assets\\Images/OrbYellow.png", "Assets\\Images/OrbGreen.png", "Assets\\Images/OrbBlue.png", "Assets\\Images/OrbPurple.png" };
            for (int i = 0; i < 6; ++i)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(System.IO.Path.Combine(parentDir, imagePaths[i]));
                img.EndInit();
                orbImages.Add(img);
            }

            shipImage = new BitmapImage();
            shipImage.BeginInit();
            shipImage.UriSource = new Uri(System.IO.Path.Combine(parentDir, "Assets\\Images/Ship1.png"));
            shipImage.EndInit();

            //----------------------------------
            ship.Source = shipImage;
            ship.Width = 20;
            //----------------------------------

            this.parentWindow = parentWindow;
            InitializeComponent();
            this.KeyDown += Window_KeyDown;
            this.KeyUp += Window_KeyUp;

            Game = new Game(cheat);
            Game.GameUpdatedEvent += Render;
            Game.Initialize();
        }

        public void Render(object sender, int e)
        {
            int wellDiff = wellDict.Count - Game.ViewCamera.StableWells.Count;
            if (wellDiff > 0)
                RemoveGameObjects(wellDict, wellDiff);
            if (wellDiff < 0)
                AddGameObjects(wellDict, wellDiff);
            
            for (int i = 0; i < wellDict.Count; ++i)
            {
                
                //display the correct well image at the right place
            }

            int destableDiff = destableDict.Count - Game.ViewCamera.UnstableWells.Count;
            if (destableDiff > 0)
                RemoveGameObjects(destableDict, destableDiff);
            if (destableDiff < 0)
                AddGameObjects(destableDict, destableDiff);

            for (int i = 0; i < destableDict.Count; ++i)
            {
                //display the correct destabilized image at the right place
            }

            int orbDiff = orbDict.Count - Game.ViewCamera.Orbs.Count;
            if (orbDiff > 0)
                RemoveGameObjects(orbDict, orbDiff);
            if (orbDiff < 0)
                AddGameObjects(orbDict, orbDiff);

            for (int i = 0; i < orbDict.Count; ++i)
            {
                //display the correct orb image at the right place
            }

            //TODO: Render the player ship

            Canvas.SetLeft(ship, Game.ViewCamera.PlayerShip.Item1);
            Canvas.SetTop(ship, Game.ViewCamera.PlayerShip.Item2);
            try
            {
                DrawCanvas.Children.Add(ship);
            }
            catch { }



            
            //Button b = new Button();
            //b.Margin = new Thickness(Game.ViewCamera.PlayerShip.Item1, Game.ViewCamera.PlayerShip.Item2, 0.0, 0.0);
            //DrawCanvas.Children.Add(b);


            //to be implemented with AI
            /*
            int shipDiff = shipDict.Count - Game.ViewCamera.PlayerShip.Count;
            if (destableDiff > 0)
                RemoveGameObjects(destableDict, destableDiff);
            if (destableDiff < 0)
                AddGameObjects(destableDict, destableDiff);

            for (int i = 0; i < destableDict.Count; ++i)
            {
                //display the correct destabilized image at the right place
            }
            */
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Game.Timer.Stop();
                PauseWindow pauseWin = new PauseWindow(Game, this);
                pauseWin.Show();
            }
            else
            {
                switch (e.Key)
                {
                    case (Key.W):
                        Game.KeyPressed('w');
                        break;
                    case (Key.A):
                        Game.KeyPressed('a');
                        break;
                    case (Key.S):
                        Game.KeyPressed('s');
                        break;
                    case (Key.D):
                        Game.KeyPressed('d');
                        break;
                    case (Key.Space):
                        Game.KeyPressed(' ');
                        break;
                    case (Key.Q):
                        Game.KeyPressed('q');
                        break;
                    case (Key.F):
                        Game.KeyPressed('f');
                        break;
                    case (Key.E):
                        Game.KeyPressed('e');
                        break;
                    default:
                        break;
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case (Key.W):
                    Game.KeyReleased('w');
                    break;
                case (Key.A):
                    Game.KeyReleased('a');
                    break;
                case (Key.S):
                    Game.KeyReleased('s');
                    break;
                case (Key.D):
                    Game.KeyReleased('d');
                    break;
                default:
                    break;
            }
        }

        public void AddGameObjects(List<Image> gameObjs, int add)
        {
            for (int i = 0; i < add; ++i)
            {
                gameObjs.Add(new Image());//TODO
                DrawCanvas.Children.Add(gameObjs[gameObjs.Count - 1]);
            }
        }

        public void RemoveGameObjects(List<Image> gameObjs, int remove)
        {
            for (int i = 0; i < remove; ++i)
            {
                DrawCanvas.Children.Remove(gameObjs[0]);
                gameObjs.RemoveAt(0);
            }
        }

        private void GameWindow_Closed(object sender, EventArgs e)
        {
            parentWindow.Show();
        }
    }
}
