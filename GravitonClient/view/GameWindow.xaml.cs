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
    public enum SoundEffect {OrbGrab, PowerupGrab, Neutralize, Destabilize, OrbDrop, Ghost, Collapse };

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
        Image background;
        Image[] planets;
        Image[] rings;
        Image[] twins;
        Image[] stars;

        private DateTime startTime;
        private TimeSpan gameDuration;
        List<BitmapImage> wellImages;
        BitmapImage destabilizedImage;
        List<BitmapImage> orbImages;
        BitmapImage shipImage;
        BitmapImage AiImage;
        BitmapImage NeutralizeImage;
        BitmapImage DestabilizeImage;
        BitmapImage GhostImage;
        BitmapImage BackgroundImage;
        BitmapImage PlanetImage;
        BitmapImage RingImage;
        BitmapImage TwinImage;
        BitmapImage StarImage;

        Image[] HudOrbs = new Image[6];
        Image[] HudPowerups = new Image[3];

        private List<int> currentOrbs = new List<int>();
        private List<Powerup.powerups> DisplayedPowerups = new List<Powerup.powerups>();

        MediaPlayer unstable;
        MediaPlayer orbGrab;
        MediaPlayer neutralize;
        MediaPlayer deposit;
        MediaPlayer powerup;
        MediaPlayer collapse;

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
            
            background = new Image();
            BackgroundImage = new BitmapImage();
            BackgroundImage.BeginInit();
            BackgroundImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/parallax-space-backgound.png");
            BackgroundImage.EndInit();
            background.Source = BackgroundImage;
            RenderOptions.SetBitmapScalingMode(background, BitmapScalingMode.NearestNeighbor);
            Canvas.SetLeft(background, 0);
            Canvas.SetTop(background, 0);
            Canvas.SetZIndex(background, 0);
            DrawCanvas.Children.Add(background);

            planets = new Image[4] { new Image(), new Image() , new Image() , new Image() };
            PlanetImage = new BitmapImage();
            PlanetImage.BeginInit();
            PlanetImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/parallax-space-big-planet.png");
            PlanetImage.EndInit();

            rings = new Image[4] { new Image(), new Image(), new Image(), new Image() };
            RingImage = new BitmapImage();
            RingImage.BeginInit();
            RingImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/parallax-space-ring-planet.png");
            RingImage.EndInit();

            twins = new Image[4] { new Image(), new Image(), new Image(), new Image() };
            TwinImage = new BitmapImage();
            TwinImage.BeginInit();
            TwinImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/parallax-space-far-planets.png");
            TwinImage.EndInit();

            stars = new Image[4] { new Image(), new Image(), new Image(), new Image() };
            StarImage = new BitmapImage();
            StarImage.BeginInit();
            StarImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/parallax-space-stars.png");
            StarImage.EndInit();

            for (int i = 0; i < 4; ++i)
            {
                planets[i].Source = PlanetImage;
                rings[i].Source = RingImage;
                twins[i].Source = TwinImage;
                stars[i].Source = StarImage;

                RenderOptions.SetBitmapScalingMode(planets[i], BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetBitmapScalingMode(rings[i], BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetBitmapScalingMode(twins[i], BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetBitmapScalingMode(stars[i], BitmapScalingMode.NearestNeighbor);

                Canvas.SetZIndex(planets[i], 4);
                Canvas.SetZIndex(rings[i], 3);
                Canvas.SetZIndex(twins[i], 2);
                Canvas.SetZIndex(stars[i], 1);

                DrawCanvas.Children.Add(planets[i]);
                DrawCanvas.Children.Add(rings[i]);
                DrawCanvas.Children.Add(twins[i]);
                DrawCanvas.Children.Add(stars[i]);
            }

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
                Canvas.SetZIndex(HudOrbs[i], 50);
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
                Canvas.SetZIndex(HudPowerups[i], 50);
                HudPowerups[i].Width = 70;
                DrawCanvas.Children.Add(HudPowerups[i]);
                Canvas.SetRight(HudPowerups[i], 75);
                Canvas.SetTop(HudPowerups[i], 80 + 70 * i);
            }
            HudPowerups[0].Source = NeutralizeImage;
            HudPowerups[1].Source = DestabilizeImage;
            HudPowerups[2].Source = GhostImage;

            UpdateHudPowerups();

            this.KeyDown += Window_KeyDown;
            this.KeyUp += Window_KeyUp;
        }
        public GameWindow(bool cheat)
        {
            Game = new Game(cheat);
            Game.GameUpdatedEvent += Render;
            Game.GameInvokeSoundEvent += PlaySound;
            Game.Initialize();
            Game.Player.GamePowerup.GameInvokeSoundEvent += PlaySound;
            SetupGameWindow();
        }

        public GameWindow(bool cheat, Game game)
        {
            Game = game;
            Game.GameUpdatedEvent += Render;
            Game.GameInvokeSoundEvent += PlaySound;
            Game.InitializeWithShipCreated();
            SetupGameWindow();
        }

        public void Render(object sender, int e)
        {
            CameraFrame currentFrame = Game.ViewCamera.GetCameraFrame();


            for (int i = 0; i < 4; ++i)
            {
                Canvas.SetLeft(planets[i], currentFrame.Backgrounds[3][i].Item1);
                Canvas.SetTop(planets[i], currentFrame.Backgrounds[3][i].Item2);
                Canvas.SetLeft(rings[i], currentFrame.Backgrounds[2][i].Item1);
                Canvas.SetTop(rings[i], currentFrame.Backgrounds[2][i].Item2);
                Canvas.SetLeft(twins[i], currentFrame.Backgrounds[1][i].Item1);
                Canvas.SetTop(twins[i], currentFrame.Backgrounds[1][i].Item2);
                Canvas.SetLeft(stars[i], currentFrame.Backgrounds[0][i].Item1);
                Canvas.SetTop(stars[i], currentFrame.Backgrounds[0][i].Item2);
            }

            int wellDiff = wellDict.Count - currentFrame.StableWells.Count;
            if (wellDiff > 0)
                RemoveGameObjects(wellDict, wellDiff);
            if (wellDiff < 0)
                AddGameObjects(wellDict, -wellDiff);
            
            for (int i = 0; i < wellDict.Count; ++i)
            {
                int color = currentFrame.StableWells[i].Item3;
                wellDict[i].Source = wellImages[color];
                Canvas.SetLeft(wellDict[i], currentFrame.StableWells[i].Item1);
                Canvas.SetTop(wellDict[i], currentFrame.StableWells[i].Item2);
                Canvas.SetZIndex(wellDict[i], 5);
            }
            

            gameDuration = DateTime.Now - startTime;
            if (gameDuration.TotalMinutes > 5) {
                Game.GameOver();
            }
            txtTimeLeft.Text = (int) (5 - gameDuration.TotalMinutes) + ":" + ((60 - (int) gameDuration.TotalSeconds % 60) % 60).ToString("D2");



            int destableDiff = destableDict.Count - currentFrame.UnstableWells.Count;
            if (destableDiff > 0)
                RemoveGameObjects(destableDict, destableDiff);
            if (destableDiff < 0)
            {
                AddGameObjects(destableDict, -destableDiff);
            }

            for (int i = 0; i < destableDict.Count; ++i)
            {
                destableDict[i].Source = destabilizedImage;
                Canvas.SetLeft(destableDict[i], currentFrame.UnstableWells[i].Item1);
                Canvas.SetTop(destableDict[i], currentFrame.UnstableWells[i].Item2);
                Canvas.SetZIndex(destableDict[i], 6);
            }

            int orbDiff =  orbDict.Count - currentFrame.Orbs.Count;
            if (orbDiff > 0)
                RemoveGameObjects(orbDict, orbDiff);
            if (orbDiff < 0)
                AddGameObjects(orbDict, -orbDiff);

            for (int i = 0; i < orbDict.Count; ++i)
            {
                int color = currentFrame.Orbs[i].Item3;
                orbDict[i].Source = orbImages[color];
                Canvas.SetLeft(orbDict[i], currentFrame.Orbs[i].Item1);
                Canvas.SetTop(orbDict[i], currentFrame.Orbs[i].Item2);
                Canvas.SetZIndex(orbDict[i], 7);
            }

            Canvas.SetLeft(ship, currentFrame.PlayerShip.Item1);
            Canvas.SetTop(ship, currentFrame.PlayerShip.Item2);
            Canvas.SetZIndex(ship, 10);
            txtScore.Text = "Score: " + game.Points;



            //to be implemented with AI
            


            int shipDiff = AiImages.Count - currentFrame.AIShips.Count;

            if (shipDiff > 0)
                RemoveGameObjects(AiImages, shipDiff);
            if (shipDiff < 0)
                AddGameObjects(AiImages, -shipDiff);
            
            for (int i = 0; i < AiImages.Count; ++i)
            {
                AiImages[i].Source = AiImage;
                AiImages[i].Width = 50;
                Canvas.SetLeft(AiImages[i], currentFrame.AIShips[i].Item1);
                Canvas.SetTop(AiImages[i], currentFrame.AIShips[i].Item2);
                Canvas.SetZIndex(AiImages[i], 9);
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
            unstable.Close();
            neutralize.Close();
            deposit.Close();
            orbGrab.Close();
            powerup.Close();
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
            unstable.Close();
            neutralize.Close();
            deposit.Close();
            orbGrab.Close();
            powerup.Close();
            App.Current.MainWindow.Show();
        }
        
        void PlaySound(object sender, SoundEffect value)
        {
            switch (value)
            {
                case SoundEffect.Destabilize:
                    unstable.Volume = .5;
                    unstable.Position = new TimeSpan(0);
                    unstable.Play();
                    break;
                case SoundEffect.Neutralize:
                    neutralize.Volume = .5;
                    neutralize.Position = new TimeSpan(0);
                    neutralize.Play();
                    break;
                case SoundEffect.OrbDrop:
                    deposit.Volume = .5;
                    deposit.Position = new TimeSpan(0);
                    deposit.Play();
                    break;
                case SoundEffect.OrbGrab:
                    orbGrab.Volume = .5;
                    orbGrab.Position = new TimeSpan(0);
                    orbGrab.Play();
                    break;
                case SoundEffect.PowerupGrab:
                    powerup.Volume = .5;
                    powerup.Position = new TimeSpan(0);
                    powerup.Play();
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
            double heldOpacity = 1.0;
            double notHeldOpacity = 0.3;

            if (game.Player.GamePowerup.CarryingNeutralize)
                HudPowerups[0].Opacity = heldOpacity;
            else
                HudPowerups[0].Opacity = notHeldOpacity;
            if (game.Player.GamePowerup.CarryingDestabilize)
                HudPowerups[1].Opacity = heldOpacity;
            else
                HudPowerups[1].Opacity = notHeldOpacity;
            if (game.Player.GamePowerup.CarryingGhost)
                HudPowerups[2].Opacity = heldOpacity;
            else
                HudPowerups[2].Opacity = notHeldOpacity;
        }

        private void GameWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //background.Width = DrawCanvas.ActualWidth;
            background.Height = DrawCanvas.ActualHeight;

            for (int i = 0; i < 4; ++i)
            {
                planets[i].Width = DrawCanvas.ActualWidth;
                rings[i].Width = DrawCanvas.ActualWidth;
                twins[i].Width = DrawCanvas.ActualWidth;
                stars[i].Width = DrawCanvas.ActualWidth;
            }

            unstable = new MediaPlayer();
            unstable.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/destabilize.mp3")));
            orbGrab = new MediaPlayer();
            orbGrab.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/SFX2.mp3")));
            neutralize = new MediaPlayer();
            neutralize.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/PowerDown5.mp3")));
            deposit = new MediaPlayer();
            deposit.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/Space entity(deposit).mp3")));
            powerup = new MediaPlayer();
            powerup.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/Power-Up-KP-1879176533 (packet pickup).mp3")));

            unstable.Volume = 0;
            orbGrab.Volume = 0;
            neutralize.Volume = 0;
            deposit.Volume = 0;
            powerup.Volume = 0;

            unstable.Play();
            neutralize.Play();
            deposit.Play();
            orbGrab.Play();
            powerup.Play();
        }
    }
}
