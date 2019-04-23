using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GravitonClient
{
    

    /// <summary>
    /// Interaction logic for NetworkedGameWindow.xaml
    /// </summary>
    public partial class NetworkedGameWindow : Window, GameReporter
    {
        List<Image> wellDict;
        List<Image> destableDict;
        List<Image> orbDict;
        List<Image> AiImages;
        List<Image> OtherHumanImages;
        List<Label> OtherHumanNames = new List<Label>();
        Image ship;
        Image background;
        Image[] planets;
        Image[] rings;
        Image[] twins;
        Image[] stars;

        private DateTime startTime;
        private TimeSpan gameDuration;
        public TimeSpan PauseDuration { get; set; }
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

        MediaPlayer unstable;
        MediaPlayer orbGrab;
        MediaPlayer neutralize;
        MediaPlayer deposit;
        MediaPlayer powerup;
        MediaPlayer collapse;
        MediaPlayer ghost;
        MediaPlayer boost;

        private NetworkedGame game;
        private bool gameStarted;

        private void SetupGameWindow()
        {
            
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

            startTime = DateTime.Now;
            PauseDuration = new TimeSpan(0);
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
            OtherHumanImages = new List<Image>();
            //----------------------------------
            ship.Source = shipImage;
            ship.Width = 50;
            //DrawCanvas.Children.Add(ship);
            //----------------------------------

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

            UpdateHudPowerups(false, false, false);

            this.KeyDown += Window_KeyDown;
            this.KeyUp += Window_KeyUp;
            if (272 / 160 > DrawCanvas.ActualWidth / DrawCanvas.ActualHeight)
            {
                background.Height = DrawCanvas.ActualHeight;
                background.Width = DrawCanvas.ActualWidth;
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
                background.Height = DrawCanvas.ActualHeight;
                for (int i = 0; i < 4; ++i)
                {
                    planets[i].Width = DrawCanvas.ActualWidth;
                    rings[i].Width = DrawCanvas.ActualWidth;
                    twins[i].Width = DrawCanvas.ActualWidth;
                    stars[i].Width = DrawCanvas.ActualWidth;
                }
            }

            unstable = new MediaPlayer();
            unstable.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/destabilize.mp3")));
            orbGrab = new MediaPlayer();
            orbGrab.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/SFX2.mp3")));
            neutralize = new MediaPlayer();
            neutralize.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/Shut-down-sound-effect.mp3")));
            deposit = new MediaPlayer();
            deposit.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/Space entity(deposit).mp3")));
            powerup = new MediaPlayer();
            powerup.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/Power-Up-KP-1879176533 (packet pickup).mp3")));
            collapse = new MediaPlayer();
            collapse.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/PowerDown5.mp3")));
            ghost = new MediaPlayer();
            ghost.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/ghost.mp3")));
            boost = new MediaPlayer();
            boost.Open(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\", "Assets/Sound/SFX/boost.mp3")));

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
        

        public NetworkedGameWindow(NetworkedGame game)
        {
            UDPGameClient.SetCurrentGameReporter(this);
            this.game = game;
            UDPGameClient.StartListening();
            game.GameUpdatedEvent += Render;
            InitializeComponent();
            gameStarted = false;
            


        }

        public void Render(object sender, NetworkedCameraFrame currentFrame)
        {
            Dispatcher.Invoke(() =>
            {
               if (!gameStarted)
               {
                    gameStarted = true;
                    DrawCanvas.Children.Remove(grid_secondsLeft);
                    SetupGameWindow();
                    Render(null, currentFrame);
               }
               else
               {

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

                   txtTimeLeft.Text = (int)(5 - Math.Ceiling((double) currentFrame.Seconds)/60) + ":" + ((60 - (int)currentFrame.Seconds % 60) % 60).ToString("D2");
                   
                   int playerdiff = OtherHumanImages.Count - currentFrame.OtherHumanShips.Count;
                   if (playerdiff > 0)
                   {
                       RemoveGameObjects(OtherHumanImages, playerdiff);
                       RemoveGameObjects(OtherHumanNames, playerdiff);
                   }
                   if (playerdiff < 0)
                   {
                       AddGameObjects(OtherHumanImages, -playerdiff);
                       AddGameObjects(OtherHumanNames, -playerdiff);
                   }
                   for (int i = 0; i < OtherHumanImages.Count; ++i)
                   {

                       OtherHumanImages[i].Source = shipImage;
                       OtherHumanImages[i].Width = 50;
                       Canvas.SetLeft(OtherHumanImages[i], currentFrame.OtherHumanShips[i].Item1);
                       Canvas.SetTop(OtherHumanImages[i], currentFrame.OtherHumanShips[i].Item2);
                       Canvas.SetLeft(OtherHumanNames[i], currentFrame.OtherHumanShips[i].Item1);
                       Canvas.SetTop(OtherHumanNames[i], currentFrame.OtherHumanShips[i].Item2 -30);
                       OtherHumanNames[i].Content = currentFrame.OtherHumanShips[i].Item3;
                       Canvas.SetZIndex(OtherHumanNames[i], 7);
                       Canvas.SetZIndex(OtherHumanImages[i], 7);
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

                   int orbDiff = orbDict.Count - currentFrame.Orbs.Count;
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
                   txtScore.Text = "Score: " + currentFrame.Points;



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
                   }


                    //==========================
                    //HUD
                    //==========================


                    if (!Enumerable.SequenceEqual(currentOrbs, currentFrame.PlayerOrbs))
                   {
                       UpdateHudOrbs(currentFrame.PlayerOrbs);
                       currentOrbs = currentFrame.PlayerOrbs.ToList();
                   }

                   UpdateHudPowerups(currentFrame.HasNeutralizePowerup,
                       currentFrame.HasDestabilizePowerup,
                       currentFrame.HasGhostingPowerup);

                   
               }
           });
        }

        

        private void GameOver_Click(object sender, RoutedEventArgs e)
        {
            unstable.Close();
            neutralize.Close();
            deposit.Close();
            orbGrab.Close();
            powerup.Close();
            collapse.Close();
            ghost.Close();
            boost.Close();
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
            
                switch (e.Key)
                {
                    case (Key.W):
                        game.KeyPressed('w');
                        break;
                    case (Key.A):
                        game.KeyPressed('a');
                        break;
                    case (Key.S):
                        game.KeyPressed('s');
                        break;
                    case (Key.D):
                        game.KeyPressed('d');
                        break;
                    case (Key.Space):
                        game.KeyPressed(' ');
                        break;
                    case (Key.Q):
                        game.KeyPressed('q');
                        break;
                    case (Key.F):
                        game.KeyPressed('f');
                        break;
                    case (Key.E):
                        game.KeyPressed('e');
                        break;
                    default:
                        break;
                
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case (Key.W):
                    game.KeyReleased('w');
                    break;
                case (Key.A):
                    game.KeyReleased('a');
                    break;
                case (Key.S):
                    game.KeyReleased('s');
                    break;
                case (Key.D):
                    game.KeyReleased('d');
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

        public void AddGameObjects(List<Label> gameObjs, int add)
        {
            for (int i = 0; i < add; ++i)
            {
                var l = new Label
                {
                    FontSize = 15,
                    FontFamily = (FontFamily)FindResource("Azonix"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = (Brush)new BrushConverter().ConvertFrom("#ffffff")
                };
                gameObjs.Add(l);
                DrawCanvas.Children.Add(gameObjs[gameObjs.Count - 1]);
            }
        }

        public void RemoveGameObjects(List<Label> gameObjs, int remove)
        {
            for (int i = 0; i < remove; ++i)
            {
                DrawCanvas.Children.Remove(gameObjs[0]);
                gameObjs.RemoveAt(0);
            }
        }

        private void GameWindow_Closed(object sender, EventArgs e)
        {
            if (gameStarted)
            {
                unstable.Close();
                neutralize.Close();
                deposit.Close();
                orbGrab.Close();
                powerup.Close();
                collapse.Close();
                ghost.Close();
                boost.Close();
                UDPGameClient.StopListening();
            }
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

        private void UpdateHudOrbs(List<int> orbs)
        {


            for (int i = 0; i < HudOrbs.Length; i++)
            {
                try
                {
                    HudOrbs[i].Source = orbImages[orbs[i]];
                    Canvas.SetTop(HudOrbs[i], 30);
                    Canvas.SetLeft(HudOrbs[i], 50 * i + 30);
                }
                catch
                {
                    HudOrbs[i].Source = null;
                }
            }
        }

        private void UpdateHudPowerups(bool neutralize, bool destabilize, bool ghost)
        {
            double heldOpacity = 1.0;
            double notHeldOpacity = 0.3;

            if (neutralize)
                HudPowerups[0].Opacity = heldOpacity;
            else
                HudPowerups[0].Opacity = notHeldOpacity;
            if (destabilize)
                HudPowerups[1].Opacity = heldOpacity;
            else
                HudPowerups[1].Opacity = notHeldOpacity;
            if (ghost)
                HudPowerups[2].Opacity = heldOpacity;
            else
                HudPowerups[2].Opacity = notHeldOpacity;
        }

        

        public void GameOver()
        {
            Dispatcher.Invoke(() =>
            {
                var GameOverRect = new Rectangle();
                GameOverRect.Fill = Brushes.Black;
                GameOverRect.Opacity = 0.75;
                DrawCanvas.Children.Add(GameOverRect);
                Canvas.SetZIndex(GameOverRect, 100);
                GameOverRect.Width = DrawCanvas.ActualWidth;
                GameOverRect.Height = DrawCanvas.ActualHeight;
            });
        }

        public void DisplayStats(GameStats gameStats)
        {
            Dispatcher.Invoke(() => {

                lbl_secondsLeft.Content = "Game Over";
                Canvas.SetZIndex(grid_secondsLeft, 10000);
                Canvas.SetZIndex(lbl_secondsLeft, 10000);
            });
            UDPGameClient.StopListening();
        }


        public void DisplaySecondsTillStart(int seconds)
        {
            Dispatcher.Invoke(()=>lbl_secondsLeft.Content = "Seconds to start: " + seconds);
        }

        public void DisplayError(string s)
        {
            MessageBox.Show(s);
        }
    }
}
