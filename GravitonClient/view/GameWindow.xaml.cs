using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
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
    public enum SoundEffect {OrbGrab, PowerupGrab, Neutralize, Destabilize, OrbDrop };

    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        List<Image> wellDict;
        List<Image> destableDict;
        List<Image> orbDict;
        List<Image> AiImages;
        Image ship;


        private DateTime startTime; 
        List<BitmapImage> wellImages;
        BitmapImage destabilizedImage;
        List<BitmapImage> orbImages;
        BitmapImage shipImage;
        BitmapImage AiImage;
        BitmapImage NeutralizeImage;
        BitmapImage DestabilizeImage;
        BitmapImage GhostImage;

        Image[] HudOrbs = new Image[6];
        Image[] HudPowerups = new Image[3];

        private List<int> currentOrbs = new List<int>();
        private List<Powerup.powerups> DisplayedPowerups = new List<Powerup.powerups>();


        SoundPlayer collapse;
        SoundPlayer orbGrab;
        SoundPlayer neutralize;
        SoundPlayer deposit;

        private Game game;
        public Game Game
        {
            get { return game; }
            set { game = value; }
        }

        

        private void SetupGameWindow()
        {
            InitializeComponent();
            wellDict = new List<Image>();
            destableDict = new List<Image>();
            orbDict = new List<Image>();
            AiImages = new List<Image>();
            ship = new Image();
            startTime = DateTime.Now;
            wellImages = new List<BitmapImage>();
            string[] imagePaths = new string[6] { "Assets/Images/WellBasic1.png", "Assets/Images/WellOrange.png", "Assets/Images/WellYellow.png", "Assets/Images/WellGreen.png", "Assets/Images/WellBlue.png", "Assets/Images/WellPurple.png" };
            for (int i = 0; i < 6; ++i)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(@"pack://application:,,,/" + imagePaths[i]);
                img.EndInit();
                wellImages.Add(img);
            }

            destabilizedImage = new BitmapImage();
            destabilizedImage.BeginInit();
            destabilizedImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/destabilized1.png");
            destabilizedImage.EndInit();

            orbImages = new List<BitmapImage>();
            imagePaths = new string[6] { "Assets/Images/OrbRed.png", "Assets/Images/OrbOrange.png", "Assets/Images/OrbYellow.png", "Assets/Images/OrbGreen.png", "Assets/Images/OrbBlue.png", "Assets/Images/OrbPurple.png" };
            for (int i = 0; i < 6; ++i)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(@"pack://application:,,,/" + imagePaths[i]);
                img.EndInit();
                orbImages.Add(img);
            }

            shipImage = new BitmapImage();
            shipImage.BeginInit();
            shipImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/Ship1.png");
            shipImage.EndInit();

            AiImage = new BitmapImage();
            AiImage.BeginInit();
            AiImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/AI1.png");
            AiImage.EndInit();

            collapse = new SoundPlayer("../../Assets/Sound/SFX/destabilize.wav");
            collapse.Load();
            orbGrab = new SoundPlayer("../../Assets/Sound/SFX/SFX2.wav");
            orbGrab.Load();
            neutralize = new SoundPlayer("../../Assets/Sound/SFX/SFX1.wav");
            neutralize.Load();
            deposit = new SoundPlayer("../../Assets/Sound/SFX/Space entity(deposit).wav");
            deposit.Load();

            AiImages = new List<Image>();
            //----------------------------------
            ship.Source = shipImage;
            ship.Width = 50;
            DrawCanvas.Children.Add(ship);
            //----------------------------------

            
            
            for (int i = 0; i < HudOrbs.Length; i++) {
                HudOrbs[i] = new Image();
                HudOrbs[i].Opacity = 0.80;
                HudOrbs[i].Width = 30;
                Canvas.SetZIndex(HudOrbs[i], 10);
                DrawCanvas.Children.Add(HudOrbs[i]);
            }

            NeutralizeImage = new BitmapImage();
            NeutralizeImage.BeginInit();
            NeutralizeImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/Neutralize.png");
            NeutralizeImage.EndInit();


            GhostImage = new BitmapImage();
            GhostImage.BeginInit();
            GhostImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/Ghosting.png");
            GhostImage.EndInit();


            DestabilizeImage = new BitmapImage();
            DestabilizeImage.BeginInit();
            DestabilizeImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/powerup.png");
            DestabilizeImage.EndInit();
            for (int i = 0; i < HudPowerups.Length; i++)
            {
                HudPowerups[i] = new Image();
                Canvas.SetZIndex(HudPowerups[i], 10);
                HudPowerups[i].Width = 70;
                DrawCanvas.Children.Add(HudPowerups[i]);
                Canvas.SetRight(HudPowerups[i], 75);
                Canvas.SetTop(HudPowerups[i], 80 + 70 * i);
                HudPowerups[i].Opacity = 0.50;
            }
            HudPowerups[0].Source = NeutralizeImage;
            HudPowerups[1].Source = DestabilizeImage;
            HudPowerups[2].Source = GhostImage;


            collapse.Play();
            neutralize.Play();
            deposit.Play();
            orbGrab.Play();

            this.KeyDown += Window_KeyDown;
            this.KeyUp += Window_KeyUp;
        }
        public GameWindow(bool cheat)
        {

            SetupGameWindow();
            Game = new Game(cheat);
            Game.GameUpdatedEvent += Render;
            Game.GameInvokeSoundEvent += PlaySound;
            Game.Initialize();
        }

        public GameWindow(bool cheat, Game game)
        {
            SetupGameWindow();

            Game = game;
            Game.GameUpdatedEvent += Render;
            Game.GameInvokeSoundEvent += PlaySound;
            Game.InitializeWithShipCreated();
        }

        public void Render(object sender, int e)
        {
            int wellDiff = wellDict.Count - Game.ViewCamera.StableWells.Count;
            if (wellDiff > 0)
                RemoveGameObjects(wellDict, wellDiff);
            if (wellDiff < 0)
                AddGameObjects(wellDict, -wellDiff);
            
            for (int i = 0; i < wellDict.Count; ++i)
            {
                int color = Game.ViewCamera.StableWells[i].Item3;
                wellDict[i].Source = wellImages[color];
                Canvas.SetLeft(wellDict[i], Game.ViewCamera.StableWells[i].Item1);
                Canvas.SetTop(wellDict[i], Game.ViewCamera.StableWells[i].Item2);
                Canvas.SetZIndex(wellDict[i], 2);
            }

            TimeSpan gameDuration = DateTime.Now - startTime;

            txtTimeLeft.Text =(int) (5 - gameDuration.TotalMinutes) + ":" + (60 - (int) gameDuration.TotalSeconds % 60).ToString("D2");
            


            int destableDiff = destableDict.Count - Game.ViewCamera.UnstableWells.Count;
            if (destableDiff > 0)
                RemoveGameObjects(destableDict, destableDiff);
            if (destableDiff < 0)
            {
                AddGameObjects(destableDict, -destableDiff);
            }

            for (int i = 0; i < destableDict.Count; ++i)
            {
                destableDict[i].Source = destabilizedImage;
                Canvas.SetLeft(destableDict[i], Game.ViewCamera.UnstableWells[i].Item1);
                Canvas.SetTop(destableDict[i], Game.ViewCamera.UnstableWells[i].Item2);
                Canvas.SetZIndex(destableDict[i], 3);
            }

            int orbDiff =  orbDict.Count - Game.ViewCamera.Orbs.Count;
            if (orbDiff > 0)
                RemoveGameObjects(orbDict, orbDiff);
            if (orbDiff < 0)
                AddGameObjects(orbDict, -orbDiff);

            for (int i = 0; i < orbDict.Count; ++i)
            {
                int color = Game.ViewCamera.Orbs[i].Item3;
                orbDict[i].Source = orbImages[color];
                Canvas.SetLeft(orbDict[i], Game.ViewCamera.Orbs[i].Item1);
                Canvas.SetTop(orbDict[i], Game.ViewCamera.Orbs[i].Item2);
                Canvas.SetZIndex(orbDict[i], 2);
            }

            Canvas.SetLeft(ship, Game.ViewCamera.PlayerShip.Item1);
            Canvas.SetTop(ship, Game.ViewCamera.PlayerShip.Item2);
            Canvas.SetZIndex(ship, 50);
            txtScore.Text = "Score: " + game.Points;



            //to be implemented with AI
            


            int shipDiff = AiImages.Count - Game.ViewCamera.AIShips.Count;

            if (shipDiff > 0)
                RemoveGameObjects(AiImages, shipDiff);
            if (shipDiff < 0)
                AddGameObjects(AiImages, -shipDiff);
            
            for (int i = 0; i < AiImages.Count; ++i)
            {
                AiImages[i].Source = AiImage;
                AiImages[i].Width = 50;
                Canvas.SetLeft(AiImages[i], Game.ViewCamera.AIShips[i].Item1);
                Canvas.SetTop(AiImages[i], Game.ViewCamera.AIShips[i].Item2);
                Canvas.SetZIndex(AiImages[i], 4);
                //display the correct destabilized image at the right place
            }
            


            Game.ViewCamera.Width = DrawCanvas.ActualWidth;
            Game.ViewCamera.Height = DrawCanvas.ActualHeight;



            //==========================
            //HUD
            //==========================
            
            
            if(!Enumerable.SequenceEqual(currentOrbs, game.Player.Orbs))
            {
                UpdateHudOrbs();
                currentOrbs = game.Player.Orbs.ToList();
            }


            if (!Enumerable.SequenceEqual(DisplayedPowerups, game.Player.GamePowerup.CurrentPowerups))
            {
                DisplayedPowerups = game.Player.GamePowerup.CurrentPowerups.ToList();
                UpdateHudPowerups();
            }
            


            if (Game.IsOver)
            {
                Button b = new Button();
                b.Content = "Return to Menu";
                b.FontSize = 40;
                b.FontFamily = (FontFamily)this.FindResource("Azonix");
                b.Margin = new Thickness(20);
                b.Padding = new Thickness(10, 5, 10, 0);
                b.Background = Brushes.Black;
                b.Foreground = Brushes.Red;
                b.Click += GameOver_Click;
                b.Width = 450;
                Canvas.SetZIndex(b, 100);
                Canvas.SetLeft(b, (DrawCanvas.ActualWidth - b.Width) / 2);
                Canvas.SetTop(b, (DrawCanvas.ActualHeight / 4 * 3));
                DrawCanvas.Children.Add(b);
            }
        }

        private void GameOver_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
            App.Current.MainWindow.Show();
        }

        //"../../Assets/Sound/SFX/destabilize.wav"
        void PlaySound(object sender, SoundEffect value)
        {
            switch (value)
            {
                case SoundEffect.Destabilize:
                    Task.Run(() => collapse.Play());
                    break;
                case SoundEffect.Neutralize:
                    Task.Run(() => neutralize.Play());
                    break;
                case SoundEffect.OrbDrop:
                    Task.Run(() => deposit.Play());
                    break;
                case SoundEffect.OrbGrab:
                    Task.Run(() => orbGrab.Play());
                    break;
                default:
                    break;
            }
        }

        private void UpdateHudOrbs()
        {

            
            for( int i = 0; i < HudOrbs.Length; i++)
            {
                try
                {
                    HudOrbs[i].Source = orbImages[game.Player.Orbs[i]];
                    Canvas.SetTop(HudOrbs[i], 30);
                    Canvas.SetLeft(HudOrbs[i], 50 * i + 30);
                }
                catch
                {
                    HudOrbs[i].Source = null;
                }
            }
        }

        private void UpdateHudPowerups()
        {
            /*
            HudPowerups[0].Source = NeutralizeImage;
            HudPowerups[1].Source = DestabilizeImage;
            HudPowerups[2].Source = GhostImage;
            */
            if (game.Player.GamePowerup.CarryingNeutralize)
                HudPowerups[0].Opacity = 1;
            else
                HudPowerups[0].Opacity = 0.50;
            if (game.Player.GamePowerup.CarryingDestabilize)
                HudPowerups[1].Opacity = 1;
            else
                HudPowerups[1].Opacity = 0.50;
            if (game.Player.GamePowerup.CarryingGhost)
                HudPowerups[2].Opacity = 1;
            else
                HudPowerups[2].Opacity = 0.50;
        }
    }
}
