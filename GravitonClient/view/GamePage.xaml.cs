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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace GravitonClient.view
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        List<Animator> wellList;
        List<Animator> destableList;
        List<Animator> orbList;
        List<Animator> AIList;
        Animator playerShip;

        List<Ellipse> waveDict;

        //to be deleted with rise of animations
        List<Image> wellDict;
        List<Image> destableDict;
        List<Image> orbDict;
        List<Image> AiImages;

        Image background;
        Image[] planets;
        Image[] rings;
        Image[] twins;
        Image[] stars;

        public bool NextRound;

        private DateTime startTime;
        private TimeSpan gameDuration;
        private DateTime pauseStartTime;
        private TimeSpan pauseDuration;

        private bool isPaused;

        public Page ParentPage { get; set; }
        public Window Window { get; set; }

        private Rectangle pauseRectangle;
        private Button btnResume;
        private Button btnLoad;
        private Button btnExit;
        private Button btnHelp;
        private TextBlock announcement;

        public const string SaveFileName = "..\\..\\Saved Games\\game1.json";

        Animator destabilizedTemplate;
        Animator wellTemplate;
        Animator playerTemplate;
        Animator orbTemplate;
        Animator aiTemplate;

        Animation redWellReg;
        Animation orangeWellReg;
        Animation yellowWellReg;
        Animation greenWellReg;
        Animation blueWellReg;
        Animation purpleWellReg;
        Animation redOrb;
        Animation orangeOrb;
        Animation yellowOrb;
        Animation greenOrb;
        Animation blueOrb;
        Animation purpleOrb;
        Animation destabilized;
        Animation player;
        Animation ai;
        
        List<BitmapImage> wellImages;
        BitmapImage destabilizedImage;
        List<BitmapImage> orbImages;
        BitmapImage shipImage1;
        BitmapImage shipImage2;
        BitmapImage shipImage3;
        BitmapImage shipImage4;
        BitmapImage shipImage5;
        BitmapImage shipImage6;
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
        MediaPlayer ghost;
        MediaPlayer boost;

        private Game game;
        public Game Game
        {
            get { return game; }
            set { game = value; }
        }

        private void SetupGameWindow()
        {
            InitializeComponent();
            SetupAssets();
            
            startTime = DateTime.Now;
            pauseDuration = new TimeSpan(0);
            
            for (int i = 0; i < HudOrbs.Length; i++)
            {
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

            //Set up pause elements
            pauseRectangle = new Rectangle();
            pauseRectangle.Fill = Brushes.Black;
            pauseRectangle.Opacity = 0.75;

            btnResume = new Button();
            btnResume.Content = "Resume";
            btnResume.FontSize = 40;
            btnResume.FontFamily = (FontFamily)this.FindResource("Azonix");
            btnResume.Margin = new Thickness(20);
            btnResume.Padding = new Thickness(10, 5, 10, 0);
            btnResume.Background = Brushes.Black;
            btnResume.Foreground = Brushes.Red;
            btnResume.Click += btnResume_Click;
            btnResume.Width = 500;

            btnExit = new Button();
            btnExit.Content = "Save and Exit";
            btnExit.FontSize = 40;
            btnExit.FontFamily = (FontFamily)this.FindResource("Azonix");
            btnExit.Margin = new Thickness(20);
            btnExit.Padding = new Thickness(10, 5, 10, 0);
            btnExit.Background = Brushes.Black;
            btnExit.Foreground = Brushes.Red;
            btnExit.Click += btnExit_Click;
            btnExit.Width = 500;

            btnLoad = new Button();
            btnLoad.Content = "Load Last Save";
            btnLoad.FontSize = 40;
            btnLoad.FontFamily = (FontFamily)this.FindResource("Azonix");
            btnLoad.Margin = new Thickness(20);
            btnLoad.Padding = new Thickness(10, 5, 10, 0);
            btnLoad.Background = Brushes.Black;
            btnLoad.Foreground = Brushes.Red;
            btnLoad.Click += btnLoad_Click;
            btnLoad.Width = 500;

            btnHelp = new Button();
            btnHelp.Content = "Help";
            btnHelp.FontSize = 40;
            btnHelp.FontFamily = (FontFamily)this.FindResource("Azonix");
            btnHelp.Margin = new Thickness(20);
            btnHelp.Padding = new Thickness(10, 5, 10, 0);
            btnHelp.Background = Brushes.Black;
            btnHelp.Foreground = Brushes.Red;
            btnHelp.Click += btnHelp_Click;
            btnHelp.Width = 500;
        }

        private void SetupAssets()
        {
            waveDict = new List<Ellipse>();
            wellDict = new List<Image>();
            destableDict = new List<Image>();
            orbDict = new List<Image>();
            AiImages = new List<Image>();

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

            planets = new Image[4] { new Image(), new Image(), new Image(), new Image() };
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

            wellImages = new List<BitmapImage>();
            string[] imagePaths = new string[6] { "Assets/Images/WellRed1.png", "Assets/Images/WellOrange1.png", "Assets/Images/WellYellow1.png", "Assets/Images/WellGreen1.png", "Assets/Images/WellBlue1.png", "Assets/Images/WellPurple1.png" };
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

            shipImage1 = new BitmapImage();
            shipImage1.BeginInit();
            shipImage1.UriSource = new Uri(@"pack://application:,,,/Assets/Images/Ship1.png");
            shipImage1.EndInit();

            shipImage2 = new BitmapImage();
            shipImage2.BeginInit();
            shipImage2.UriSource = new Uri(@"pack://application:,,,/Assets/Images/Ship2.png");
            shipImage2.EndInit();

            shipImage3 = new BitmapImage();
            shipImage3.BeginInit();
            shipImage3.UriSource = new Uri(@"pack://application:,,,/Assets/Images/Ship3.png");
            shipImage3.EndInit();

            shipImage4 = new BitmapImage();
            shipImage4.BeginInit();
            shipImage4.UriSource = new Uri(@"pack://application:,,,/Assets/Images/Ship4.png");
            shipImage4.EndInit();

            shipImage5 = new BitmapImage();
            shipImage5.BeginInit();
            shipImage5.UriSource = new Uri(@"pack://application:,,,/Assets/Images/Ship5.png");
            shipImage5.EndInit();

            shipImage6 = new BitmapImage();
            shipImage6.BeginInit();
            shipImage6.UriSource = new Uri(@"pack://application:,,,/Assets/Images/Ship6.png");
            shipImage6.EndInit();

            AiImage = new BitmapImage();
            AiImage.BeginInit();
            AiImage.UriSource = new Uri(@"pack://application:,,,/Assets/Images/AI1.png");
            AiImage.EndInit();

            announcement = new TextBlock();
            
            player = new Animation(new BitmapImage[10] { shipImage1, shipImage2, shipImage3, shipImage4, shipImage5, shipImage6, shipImage5, shipImage4, shipImage3, shipImage2 }, new int[10] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 });
            redWellReg = new Animation(new BitmapImage[1] { wellImages[0] }, new int[1] { 20 });
            orangeWellReg = new Animation(new BitmapImage[1] { wellImages[1] }, new int[1] { 20 });
            yellowWellReg = new Animation(new BitmapImage[1] { wellImages[2] }, new int[1] { 20 });
            greenWellReg = new Animation(new BitmapImage[1] { wellImages[3] }, new int[1] { 20 });
            blueWellReg = new Animation(new BitmapImage[1] { wellImages[4] }, new int[1] { 20 });
            purpleWellReg = new Animation(new BitmapImage[1] { wellImages[5] }, new int[1] { 20 });
            redOrb = new Animation(new BitmapImage[1] { orbImages[0] }, new int[1] { 20 });
            orangeOrb = new Animation(new BitmapImage[1] { orbImages[1] }, new int[1] { 20 });
            yellowOrb = new Animation(new BitmapImage[1] { orbImages[2] }, new int[1] { 20 });
            greenOrb = new Animation(new BitmapImage[1] { orbImages[3] }, new int[1] { 20 });
            blueOrb = new Animation(new BitmapImage[1] { orbImages[4] }, new int[1] { 20 });
            purpleOrb = new Animation(new BitmapImage[1] { orbImages[5] }, new int[1] { 20 });
            destabilized = new Animation(new BitmapImage[1] { orbImages[5] }, new int[1] { 20 });
            ai = new Animation(new BitmapImage[1] { AiImage }, new int[1] { 20 });

            playerShip = new Animator(DrawCanvas, new Animation[1] { player }, 0, 10, 50);
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HelpPage(this));
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Game = GameLoader.Load(SaveFileName, false);
                GamePage newWindow = new GamePage(Game.IsCheat, Game, ParentPage, Window);
                this.NavigationService.Navigate(newWindow);
                Game.Timer.Start();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Cannot find file.");
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Directory.GetCurrentDirectory(), SaveFileName)));
            GameLoader.Save(Game, SaveFileName);
            this.NavigationService.Navigate(ParentPage);
        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            UnPause();
        }

        public GamePage(bool cheat, Page parentPage, Window w)
        {
            ParentPage = parentPage;
            Window = w;
            Window.KeyDown += Window_KeyDown;
            Window.KeyUp += Window_KeyUp;
            Game = new Game(cheat);
            isPaused = false;
            Game.GameUpdatedEvent += Render;
            Game.GameInvokeSoundEvent += PlaySound;
            Game.Initialize();
            Game.Player.GamePowerup.GameInvokeSoundEvent += PlaySound;
            Game.Player.GameInvokeSoundEvent += PlaySound;
            SetupGameWindow();
        }

        public GamePage(bool cheat, Game game, Page parentPage, Window w)
        {
            ParentPage = parentPage;
            Window = w;
            Window.KeyDown += Window_KeyDown;
            Window.KeyUp += Window_KeyUp;
            Game = game;
            isPaused = false;
            Game.GameUpdatedEvent += Render;
            Game.GameInvokeSoundEvent += PlaySound;
            Game.InitializeWithShipCreated();
            Game.Player.GameInvokeSoundEvent += PlaySound;
            game.Player.GamePowerup.GameInvokeSoundEvent += PlaySound;
            SetupGameWindow();
        }

        public void Render(object sender, CameraFrame frame)
        {
            for (int i = 0; i < 4; ++i)
            {
                Canvas.SetLeft(planets[i], frame.Backgrounds[3][i].Item1);
                Canvas.SetTop(planets[i], frame.Backgrounds[3][i].Item2);
                Canvas.SetLeft(rings[i], frame.Backgrounds[2][i].Item1);
                Canvas.SetTop(rings[i], frame.Backgrounds[2][i].Item2);
                Canvas.SetLeft(twins[i], frame.Backgrounds[1][i].Item1);
                Canvas.SetTop(twins[i], frame.Backgrounds[1][i].Item2);
                Canvas.SetLeft(stars[i], frame.Backgrounds[0][i].Item1);
                Canvas.SetTop(stars[i], frame.Backgrounds[0][i].Item2);
            }

            int wellDiff = wellDict.Count - frame.StableWells.Count;
            if (wellDiff > 0)
                RemoveGameObjects(wellDict, wellDiff);
            if (wellDiff < 0)
                AddGameObjects(wellDict, -wellDiff);

            for (int i = 0; i < wellDict.Count; ++i)
            {
                int color = frame.StableWells[i].Item3;
                wellDict[i].Source = wellImages[color];
                Canvas.SetLeft(wellDict[i], frame.StableWells[i].Item1);
                Canvas.SetTop(wellDict[i], frame.StableWells[i].Item2);
                Canvas.SetZIndex(wellDict[i], 5);
            }

            //Time Limit
            gameDuration = DateTime.Now - startTime - pauseDuration;
            if (gameDuration.TotalMinutes > 5)
            {
                Game.Timer.Stop();
                Game.IsOver = true;

                Button b2 = new Button();
                b2.Content = "Start Next Round";
                b2.FontSize = 40;
                b2.FontFamily = (FontFamily)this.FindResource("Azonix");
                b2.Margin = new Thickness(20);
                b2.Padding = new Thickness(10, 5, 10, 0);
                b2.Background = Brushes.Black;
                b2.Foreground = Brushes.Red;
                b2.Click += NextRound_Click;
                b2.Width = 500;
                Canvas.SetZIndex(b2, 101);
                Canvas.SetLeft(b2, (DrawCanvas.ActualWidth - b2.Width) / 2);
                Canvas.SetTop(b2, DrawCanvas.ActualHeight / 4);
                DrawCanvas.Children.Add(b2);
            }
            int sLeft = 300 - (int)gameDuration.TotalSeconds;
            txtTimeLeft.Text = (sLeft / 60) + ":" + (sLeft % 60).ToString("D2");


            int destableDiff = destableDict.Count - frame.UnstableWells.Count;
            if (destableDiff > 0)
                RemoveGameObjects(destableDict, destableDiff);
            if (destableDiff < 0)
            {
                AddGameObjects(destableDict, -destableDiff);
            }

            for (int i = 0; i < destableDict.Count; ++i)
            {
                destableDict[i].Source = destabilizedImage;
                Canvas.SetLeft(destableDict[i], frame.UnstableWells[i].Item1);
                Canvas.SetTop(destableDict[i], frame.UnstableWells[i].Item2);
                Canvas.SetZIndex(destableDict[i], 6);
            }

            foreach (Ellipse e in waveDict)
            {
                DrawCanvas.Children.Remove(e);
            }
            waveDict = new List<Ellipse>();
            foreach (Tuple<double, double, int> t in frame.ShockWaves)
            {
                Ellipse c = new Ellipse();
                c.Width = t.Item3;
                c.Height = t.Item3;
                Canvas.SetLeft(c, t.Item1);
                Canvas.SetTop(c, t.Item2);
                Canvas.SetZIndex(c, 1000);
                c.Stroke = Brushes.Yellow;

                DrawCanvas.Children.Add(c);
                waveDict.Add(c);
            }

            int orbDiff = orbDict.Count - frame.Orbs.Count;
            if (orbDiff > 0)
                RemoveGameObjects(orbDict, orbDiff);
            if (orbDiff < 0)
                AddGameObjects(orbDict, -orbDiff);

            for (int i = 0; i < orbDict.Count; ++i)
            {
                int color = frame.Orbs[i].Item3;
                orbDict[i].Source = orbImages[color];
                Canvas.SetLeft(orbDict[i], frame.Orbs[i].Item1);
                Canvas.SetTop(orbDict[i], frame.Orbs[i].Item2);
                Canvas.SetZIndex(orbDict[i], 7);
            }
            
            playerShip.Animate(frame.PlayerShip.Item1, frame.PlayerShip.Item2);
            txtScore.Text = "Score: " + game.Points;



            //to be implemented with AI



            int shipDiff = AiImages.Count - frame.AIShips.Count;

            if (shipDiff > 0)
                RemoveGameObjects(AiImages, shipDiff);
            if (shipDiff < 0)
                AddGameObjects(AiImages, -shipDiff);

            for (int i = 0; i < AiImages.Count; ++i)
            {
                AiImages[i].Source = AiImage;
                AiImages[i].Width = 50;
                Canvas.SetLeft(AiImages[i], frame.AIShips[i].Item1);
                Canvas.SetTop(AiImages[i], frame.AIShips[i].Item2);
                Canvas.SetZIndex(AiImages[i], 9);
            }



            Game.ViewCamera.Width = DrawCanvas.ActualWidth;
            Game.ViewCamera.Height = DrawCanvas.ActualHeight;



            //==========================
            //HUD
            //==========================


            if (!Enumerable.SequenceEqual(currentOrbs, game.Player.Orbs))
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
                b.Width = 500;
                Canvas.SetZIndex(b, 101);
                Canvas.SetLeft(b, (DrawCanvas.ActualWidth - b.Width) / 2);
                Canvas.SetTop(b, (DrawCanvas.ActualHeight / 4 * 3));
                DrawCanvas.Children.Add(b);
                if (!DrawCanvas.Children.Contains(pauseRectangle)) { DrawCanvas.Children.Add(pauseRectangle); }
                Canvas.SetZIndex(pauseRectangle, 100);
                pauseRectangle.Width = DrawCanvas.ActualWidth;
                pauseRectangle.Height = DrawCanvas.ActualHeight;
            }
        }

        private void NextRound_Click(object sender, RoutedEventArgs e)
        {
            int newWellSpawnFreq = Game.WellSpawnFreq - 50;
            int newWellDestabFreq = Game.WellDestabFreq - 250;
            int points = Game.Points;
            bool isCheat = Game.IsCheat;
            string username = Game.Username;

            NextRound = true;

            GamePage g = new GamePage(isCheat, ParentPage, Window);
            g.Game.WellSpawnFreq = newWellSpawnFreq;
            g.Game.WellDestabFreq = newWellDestabFreq;
            g.Game.Points = points;
            g.game.Username = username;
            if (DrawCanvas.Children.Contains(pauseRectangle)) { DrawCanvas.Children.Remove(pauseRectangle); }
            
            this.NavigationService.Navigate(g);
            GameWindow_Closed();
        }

        private void GameOver_Click(object sender, RoutedEventArgs e)
        {
            NextRound = false;

            unstable.Close();
            neutralize.Close();
            deposit.Close();
            orbGrab.Close();
            powerup.Close();
            collapse.Close();
            ghost.Close();
            boost.Close();
            Game.GameOver();
            this.NavigationService.Navigate(ParentPage);
            GameWindow_Closed();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (!isPaused)
                {
                    Pause();
                }
                else if (isPaused)
                {
                    UnPause();
                }
                //PausePage pauseWin = new PausePage(Game, this, Window);
                //this.NavigationService.Navigate(pauseWin);
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

        public void Pause()
        {
            isPaused = true;
            Game.Timer.Stop();
            unstable.Pause();
            neutralize.Pause();
            deposit.Pause();
            orbGrab.Pause();
            powerup.Pause();
            collapse.Pause();
            ghost.Pause();
            boost.Pause();
            pauseStartTime = DateTime.Now;
            DrawCanvas.Children.Add(btnResume);
            DrawCanvas.Children.Add(btnExit);
            DrawCanvas.Children.Add(btnHelp);
            DrawCanvas.Children.Add(btnLoad);
            if (!DrawCanvas.Children.Contains(pauseRectangle)) { DrawCanvas.Children.Add(pauseRectangle); }
            Canvas.SetZIndex(pauseRectangle, 100);
            pauseRectangle.Width = DrawCanvas.ActualWidth;
            pauseRectangle.Height = DrawCanvas.ActualHeight;
            Canvas.SetZIndex(btnResume, 101);
            Canvas.SetLeft(btnResume, (DrawCanvas.ActualWidth - btnResume.Width) / 2);
            Canvas.SetTop(btnResume, DrawCanvas.ActualHeight / 5);
            Canvas.SetZIndex(btnExit, 101);
            Canvas.SetLeft(btnExit, (DrawCanvas.ActualWidth - btnExit.Width) / 2);
            Canvas.SetTop(btnExit, DrawCanvas.ActualHeight / 5 * 4);
            Canvas.SetZIndex(btnLoad, 101);
            Canvas.SetLeft(btnLoad, (DrawCanvas.ActualWidth - btnLoad.Width) / 2);
            Canvas.SetTop(btnLoad, DrawCanvas.ActualHeight / 5 * 3);
            Canvas.SetZIndex(btnHelp, 101);
            Canvas.SetLeft(btnHelp, (DrawCanvas.ActualWidth - btnHelp.Width) / 2);
            Canvas.SetTop(btnHelp, DrawCanvas.ActualHeight / 5 * 2);
        }

        public void UnPause()
        {
            isPaused = false;
            Game.Timer.Start();
            unstable.Play();
            neutralize.Play();
            deposit.Play();
            orbGrab.Play();
            powerup.Play();
            collapse.Play();
            ghost.Play();
            boost.Play();
            pauseDuration += DateTime.Now - pauseStartTime;
            DrawCanvas.Children.Remove(btnResume);
            DrawCanvas.Children.Remove(btnExit);
            DrawCanvas.Children.Remove(btnHelp);
            DrawCanvas.Children.Remove(btnLoad);
            if (DrawCanvas.Children.Contains(pauseRectangle)) { DrawCanvas.Children.Remove(pauseRectangle); }
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

        private void GameWindow_Closed()
        {
            unstable.Close();
            neutralize.Close();
            deposit.Close();
            orbGrab.Close();
            powerup.Close();
            collapse.Close();
            ghost.Close();
            boost.Close();
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
                case SoundEffect.Collapse:
                    collapse.Volume = .5;
                    collapse.Position = new TimeSpan(0);
                    collapse.Play();
                    break;
                case SoundEffect.Ghost:
                    ghost.Volume = .5;
                    ghost.Position = new TimeSpan(0);
                    ghost.Play();
                    break;
                case SoundEffect.Boost:
                    boost.Volume = .5;
                    boost.Position = new TimeSpan(0);
                    boost.Play();
                    break;
                default:
                    break;
            }
        }

        public void UpdateAnimation(bool isTransition, AnimationType type, int objIndex, int animIndex, int transitionIndex)
        {

        }

        private void UpdateHudOrbs()
        {


            for (int i = 0; i < HudOrbs.Length; i++)
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
            if ((DrawCanvas.ActualWidth / 272) * 160 < DrawCanvas.ActualHeight)
            {
                background.Height = DrawCanvas.ActualHeight;
                for (int i = 0; i < 4; ++i)
                {
                    planets[i].Height = DrawCanvas.ActualHeight;
                    rings[i].Height = DrawCanvas.ActualHeight;
                    twins[i].Height = DrawCanvas.ActualHeight;
                    stars[i].Height = DrawCanvas.ActualHeight;
                }
            }

            else
            {
                background.Height = DrawCanvas.ActualWidth;
                for (int i = 0; i < 4; ++i)
                {
                    planets[i].Width = DrawCanvas.ActualWidth;
                    rings[i].Width = DrawCanvas.ActualWidth;
                    twins[i].Width = DrawCanvas.ActualWidth;
                    stars[i].Width = DrawCanvas.ActualWidth;
                }
            }

            unstable = new MediaPlayer();
            unstable.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets/Sound/SFX/destabilize.mp3")));
            orbGrab = new MediaPlayer();
            orbGrab.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets/Sound/SFX/SFX2.mp3")));
            neutralize = new MediaPlayer();
            neutralize.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets/Sound/SFX/Shut-down-sound-effect.mp3")));
            deposit = new MediaPlayer();
            deposit.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets/Sound/SFX/Space entity(deposit).mp3")));
            powerup = new MediaPlayer();
            powerup.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets/Sound/SFX/Power-Up-KP-1879176533 (packet pickup).mp3")));
            collapse = new MediaPlayer();
            collapse.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets/Sound/SFX/PowerDown5.mp3")));
            ghost = new MediaPlayer();
            ghost.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets/Sound/SFX/ghost.mp3")));
            boost = new MediaPlayer();
            boost.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets/Sound/SFX/boost.mp3")));

            unstable.Volume = 0;
            orbGrab.Volume = 0;
            neutralize.Volume = 0;
            deposit.Volume = 0;
            powerup.Volume = 0;
            collapse.Volume = 0;
            ghost.Volume = 0;
            boost.Volume = 0;

            unstable.Play();
            neutralize.Play();
            deposit.Play();
            orbGrab.Play();
            powerup.Play();
            collapse.Play();
            ghost.Play();
            boost.Play();
        }
        private void DisplayMessage(string s)
        {
            announcement.Text = s;
            Canvas.SetZIndex(announcement, 1000);
            announcement.FontSize = 20;
            //announcement.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Azonix.otf");
            Canvas.SetLeft(announcement, (DrawCanvas.ActualWidth - announcement.ActualWidth) / 2);
            Canvas.SetTop(announcement, 400);
            DrawCanvas.Children.Add(announcement);
        }
    }
}
