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

            wellImages = new List<BitmapImage>();
            destabilizedImage = new BitmapImage();
            orbImages = new List<BitmapImage>();
            shipImage = new BitmapImage();
            
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

            wellImages = new List<BitmapImage>();
            destabilizedImage = new BitmapImage();
            orbImages = new List<BitmapImage>();
            shipImage = new BitmapImage();
            
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
            }
        }

        public void RemoveGameObjects(List<Image> gameObjs, int remove)
        {
            for (int i = 0; i < remove; ++i)
            {
                gameObjs.RemoveAt(0);
            }
        }
    }
}
