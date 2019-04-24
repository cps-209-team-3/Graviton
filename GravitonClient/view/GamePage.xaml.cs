//-----------------------------------------------------------
//File:   GamePage.xaml.cs
//Desc:   Counterpart for GamePage.xaml, contains logic for view.
//----------------------------------------------------------- 

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
using Path = System.IO.Path;

namespace GravitonClient
{
    //-----------------------------------------------------------
    //        This class controls the view logic for the 
    //        actual game. This is where most asset 
    //        initialization happens.
    //----------------------------------------------------------- 
    public partial class GamePage : Page
    {
        //Lists of animator objects for wells, black holes, orbs, and AI respectively
        List<Animator> wellList;
        List<Animator> destableList;
        List<Animator> orbList;
        List<Animator> AIList;
        //Player Animator object
        Animator playerShip;
        //List of shockwave ellipses (for display)
        List<Ellipse> waveDict;

        //to be deleted with rise of animations
        List<Image> destableDict;
        List<Image> orbDict;
        List<Image> AiImages;

        //Image components for the various background layers. Moving parts have arrays of 4 Images to allow for wrapping.
        Image background;
        Image[] planets;
        Image[] rings;
        Image[] twins;
        Image[] stars;

        //TODO
        private DateTime startTime;
        private TimeSpan gameDuration;
        private DateTime pauseStartTime;
        private TimeSpan pauseDuration;

        //Determines if the game is paused
        private bool isPaused;

        //The page that created the current instance of GamePage.
        public Page ParentPage { get; set; }
        //The window that this page is displayed in.
        public Window Window { get; set; }

        //The semi-transparent overlay when pause or game over occurs.
        private Rectangle pauseRectangle;
        //Buttons to resume game, load from save, exit to menu, or go to help page respectively
        private Button btnResume;
        private Button btnLoad;
        private Button btnExit;
        private Button btnHelp;
        //Textblock to display announcments such as "Game Over"
        private TextBlock announcement;

        //Filepath for game save.
        public const string SaveFileName = "..\\..\\Saved Games\\game1.json";
        
        //Animation objects for orbs, black holes, the player, and AI respectively.
        Animation redOrb, orangeOrb, yellowOrb, greenOrb, blueOrb, purpleOrb;
        Animation destabilized;
        Animation player;
        Animation ai;
        
        //Bitmap image variables for wells, black holes, orbs, the player, and AI respectively.
        List<BitmapImage> wellImages;
        List<BitmapImage> destabilizedImages;
        List<BitmapImage> orbImages;
        BitmapImage shipImage1, shipImage2, shipImage3, shipImage4, shipImage5, shipImage6;
        BitmapImage AiImage;

        //Bitmap image variables for the powerup indicators (neutralize, destabilize, and ghost respectively) in the HUD.
        BitmapImage NeutralizeImage;
        BitmapImage DestabilizeImage;
        BitmapImage GhostImage;

        //Bitmap image variables for the background layers.
        BitmapImage BackgroundImage;
        BitmapImage PlanetImage;
        BitmapImage RingImage;
        BitmapImage TwinImage;
        BitmapImage StarImage;

        //Image components for displaying powerups and currently held orbs to the HUD.
        Image[] HudOrbs = new Image[6];
        Image[] HudPowerups = new Image[3];

        //List of currently held energy orbs.
        private List<int> currentOrbs = new List<int>();
        //List of currently held powerups.
        private List<Powerup.powerups> DisplayedPowerups = new List<Powerup.powerups>();

        //Media player objects for sound effects:
        //stable well converts to black hole
        MediaPlayer unstable;
        //player picks up orb
        MediaPlayer orbGrab;
        //player neutralizes a regular well without getting a powerup
        MediaPlayer neutralize;
        //player deposits an orb in a well
        MediaPlayer deposit;
        //player gets a powerup from neutralizing a well
        MediaPlayer powerup;
        //black hole collapses (disappears from screen)
        MediaPlayer collapse;
        //player uses ghost powerup on a well
        MediaPlayer ghost;
        //player uses boost ability
        MediaPlayer boost;

        //Instance of the game model, field and property
        private Game game;
        public Game Game
        {
            get { return game; }
            set { game = value; }
        }

        //Sets up game page HUD and general GUI.
        //Accepts nothing.
        //Returns nothing.
        private void SetupGameWindow()
        {
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

        //Sets up game assets (background, images, animations, sound effects, etc.)
        //Accepts nothing.
        //Returns nothing.
        private void SetupAssets()
        {
            waveDict = new List<Ellipse>();
            destableDict = new List<Image>();
            orbDict = new List<Image>();
            AiImages = new List<Image>();
            
            wellImages = new List<BitmapImage>();
            string[] imagePaths = new string[36] { "Assets/Images/WellRed1.png", "Assets/Images/WellRed2.png", "Assets/Images/WellRed3.png",
                "Assets/Images/WellRed4.png", "Assets/Images/WellRed5.png", "Assets/Images/WellRed6.png", "Assets/Images/WellOrange1.png",
                "Assets/Images/WellOrange2.png", "Assets/Images/WellOrange3.png", "Assets/Images/WellOrange4.png", "Assets/Images/WellOrange5.png",
                "Assets/Images/WellOrange6.png", "Assets/Images/WellYellow1.png", "Assets/Images/WellYellow2.png", "Assets/Images/WellYellow3.png",
                "Assets/Images/WellYellow4.png", "Assets/Images/WellYellow5.png", "Assets/Images/WellYellow6.png", "Assets/Images/WellGreen1.png",
                "Assets/Images/WellGreen2.png", "Assets/Images/WellGreen3.png", "Assets/Images/WellGreen4.png", "Assets/Images/WellGreen5.png",
                "Assets/Images/WellGreen6.png", "Assets/Images/WellBlue1.png", "Assets/Images/WellBlue2.png", "Assets/Images/WellBlue3.png",
                "Assets/Images/WellBlue4.png", "Assets/Images/WellBlue5.png", "Assets/Images/WellBlue6.png", "Assets/Images/WellPurple1.png",
                "Assets/Images/WellPurple2.png", "Assets/Images/WellPurple3.png", "Assets/Images/WellPurple4.png", "Assets/Images/WellPurple5.png",
                "Assets/Images/WellPurple6.png" };
            for (int i = 0; i < 36; ++i)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(@"pack://application:,,,/" + imagePaths[i]);
                img.EndInit();
                wellImages.Add(img);
            }

            destabilizedImages = new List<BitmapImage>();
            imagePaths = new string[24] {"Assets/Images/destabilized1.png", "Assets/Images/destabilized2.png", "Assets/Images/destabilized3.png",
                "Assets/Images/destabilized4.png", "Assets/Images/destabilized5.png", "Assets/Images/destabilized6.png", "Assets/Images/destabilized7.png",
                "Assets/Images/destabilized8.png", "Assets/Images/destabilized9.png", "Assets/Images/destabilized10.png", "Assets/Images/destabilized11.png",
                "Assets/Images/destabilized12.png", "Assets/Images/destabilized13.png", "Assets/Images/destabilized14.png", "Assets/Images/destabilized15.png",
                "Assets/Images/destabilized16.png", "Assets/Images/destabilized17.png", "Assets/Images/destabilized18.png", "Assets/Images/destabilized19.png",
                "Assets/Images/destabilized20.png", "Assets/Images/destabilized21.png", "Assets/Images/destabilized22.png", "Assets/Images/destabilized23.png",
                "Assets/Images/destabilized24.png" };
            for (int i = 0; i < 24; ++i)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(@"pack://application:,,,/" + imagePaths[i]);
                img.EndInit();
                destabilizedImages.Add(img);
            }

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
            redOrb = new Animation(new BitmapImage[1] { orbImages[0] }, new int[1] { 20 });
            orangeOrb = new Animation(new BitmapImage[1] { orbImages[1] }, new int[1] { 20 });
            yellowOrb = new Animation(new BitmapImage[1] { orbImages[2] }, new int[1] { 20 });
            greenOrb = new Animation(new BitmapImage[1] { orbImages[3] }, new int[1] { 20 });
            blueOrb = new Animation(new BitmapImage[1] { orbImages[4] }, new int[1] { 20 });
            purpleOrb = new Animation(new BitmapImage[1] { orbImages[5] }, new int[1] { 20 });
            destabilized = new Animation(new BitmapImage[1] { orbImages[5] }, new int[1] { 20 });
            ai = new Animation(new BitmapImage[1] { AiImage }, new int[1] { 20 });
            
            playerShip = new Animator(DrawCanvas, new Animation[1] { player }, 0, 10, 50);

            wellList = new List<Animator>();
            destableList = new List<Animator>();
        }

        //Logic for when Help Button is clicked (while game is paused). Navigates to help page.
        //Accepts regular eventhandler args.
        //Returns nothing.
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HelpPage(this));
        }

        //Logic for when Load from Save button is clicked (while game is paused). Loads current save file.
        //Accepts normal eventhandler args.
        //Returns nothing.
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

        //Logic for when Save and Exit button is clicked (while game is paused). Saves the game and returns to the play page.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Directory.GetCurrentDirectory(), SaveFileName)));
            GameLoader.Save(Game, SaveFileName);
            this.NavigationService.Navigate(ParentPage);
        }

        //Logic for when Resume button is clicked (while game is paused). Resumes the game.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            UnPause();
        }

        //Constructor overload 1
        public GamePage(bool cheat, Page parentPage, Window w)
        {
            InitializeComponent();
            SetupAssets();
            ParentPage = parentPage;
            Window = w;
            Window.KeyDown += Window_KeyDown;
            Window.KeyUp += Window_KeyUp;
            Game = new Game(cheat);
            isPaused = false;
            Game.GameUpdatedEvent += Render;
            Game.GameInvokeSoundEvent += PlaySound;
            game.UpdateAnimationEvent += UpdateAnimation;
            Game.Initialize();
            Game.Player.GamePowerup.GameInvokeSoundEvent += PlaySound;
            game.Player.GamePowerup.UpdateAnimationEvent += UpdateAnimation;
            Game.Player.GameInvokeSoundEvent += PlaySound;
            for (int i = 0; i < game.AIShips.Count; ++i)
            {
                game.AIShips[i].GamePowerup.UpdateAnimationEvent += UpdateAnimation;
            }
            SetupGameWindow();
        }

        //Constructor overload 2
        public GamePage(bool cheat, Game game, Page parentPage, Window w)
        {
            InitializeComponent();
            SetupAssets();
            ParentPage = parentPage;
            Window = w;
            Window.KeyDown += Window_KeyDown;
            Window.KeyUp += Window_KeyUp;
            Game = game;
            isPaused = false;
            Game.GameUpdatedEvent += Render;
            Game.GameInvokeSoundEvent += PlaySound;
            game.UpdateAnimationEvent += UpdateAnimation;
            Game.InitializeWithShipCreated();
            Game.Player.GameInvokeSoundEvent += PlaySound;
            game.Player.GamePowerup.GameInvokeSoundEvent += PlaySound;
            game.Player.GamePowerup.UpdateAnimationEvent += UpdateAnimation;
            for (int i = 0; i < game.AIShips.Count; ++i)
            {
                game.AIShips[i].GamePowerup.UpdateAnimationEvent += UpdateAnimation;
            }
            SetupGameWindow();
        }

        //Event handler. Called every tick. Displays game visually on GamePage's Canvas.
        //Accepts a sender object and a CameraFrame object.
        //Returns nothing.
        public void Render(object sender, CameraFrame frame)
        {
            //set background positions.
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
            
            //update well animation image sources
            for (int i = 0; i < wellList.Count; ++i)
            {
                wellList[i].Update();
            }

            //display well animations (in the camera frame) to screen
            for (int i = 0; i < frame.ScreenStables.Count; ++i)
            {
                wellList[frame.ScreenStables[i]].Animate(frame.StableWells[i].Item1, frame.StableWells[i].Item2);
            }

            //update black hole animation image sources
            for (int i = 0; i < destableList.Count; ++i)
            {
                destableList[i].Update();
            }

            //display black hole animations (in the camera frame) to screen
            for (int i = 0; i < frame.ScreenUnstables.Count; ++i)
            {
                destableList[frame.ScreenUnstables[i]].Animate(frame.UnstableWells[i].Item1, frame.UnstableWells[i].Item2);
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

            //display shockwaves
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
                c.Stroke = Brushes.DarkRed;
                c.StrokeThickness = 5;

                DrawCanvas.Children.Add(c);
                waveDict.Add(c);
            }

            //display energy orbs
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

            //display player 
            playerShip.Update();
            playerShip.Animate(frame.PlayerShip.Item1, frame.PlayerShip.Item2);

            //display score
            txtScore.Text = "Score: " + game.Points;
            
            //display ai
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
                DisplayMessage("Game Over.");
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
        
        //Logic for when Next Round button is clicked (when time has expired). Begins new round.
        //Accepts normal eventhandler args.
        //Returns nothing.
        private void NextRound_Click(object sender, RoutedEventArgs e)
        {
            int newWellSpawnFreq = Game.WellSpawnFreq - 50;
            int newWellDestabFreq = Game.WellDestabFreq - 250;
            int points = Game.Points;
            bool isCheat = Game.IsCheat;
            string username = Game.Username;

            GamePage g = new GamePage(isCheat, ParentPage, Window);
            g.Game.WellSpawnFreq = newWellSpawnFreq;
            g.Game.WellDestabFreq = newWellDestabFreq;
            g.Game.Points = points;
            g.game.Username = username;
            if (DrawCanvas.Children.Contains(pauseRectangle)) { DrawCanvas.Children.Remove(pauseRectangle); }
            
            this.NavigationService.Navigate(g);
            GameWindow_Closed();
        }
        
        //Logic for when Save and Exit button is clicked (when time expires or game is over). Returns to menu and processes high score.
        //Accepts normal eventhandler args.
        //Returns nothing.
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
            Game.GameOver();
            this.NavigationService.Navigate(ParentPage);
            GameWindow_Closed();
        }

        //Handles input when key is pressed.
        //Accepts normal eventhandler args.
        //Returns nothing.
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

        //Handles key input when key is released.
        //Accepts normal eventhandler args.
        //Returns nothing.
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

        //Pauses the game, overlaying the game and adding several buttons to the screen.
        //Accepts nothing.
        //Returns nothing.
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

        //Ends pause; undoes work done by pause method.
        //Accepts nothing.
        //Returns nothing.
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

        //adds game objects to the canvas (only for images, obsolete for animations)
        public void AddGameObjects(List<Image> gameObjs, int add)
        {
            for (int i = 0; i < add; ++i)
            {
                gameObjs.Add(new Image());//TODO
                DrawCanvas.Children.Add(gameObjs[gameObjs.Count - 1]);
            }
        }

        //removes game objects from the canvas (only for images, obsolete for animations)
        public void RemoveGameObjects(List<Image> gameObjs, int remove)
        {
            for (int i = 0; i < remove; ++i)
            {
                DrawCanvas.Children.Remove(gameObjs[0]);
                gameObjs.RemoveAt(0);
            }
        }

        //Called when navigating away from this page
        //Accepts nothing.
        //Returns nothing.
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

        //Handles sound effects.
        //Accepts a sender object and a SoundEffect enum.
        //Returns nothing.
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

        //Handles animation changes.
        //Accepts a sender object and an AnimationEventArgs object.
        //Returns nothing.
        public void UpdateAnimation(object sender, AnimationEventArgs e)
        {
            //if objIndex is one greater than the last animation index in the desired array, create new animator and add to canvas
            //if animIndex is one greater than the last animation index in the desired array, remove from canvas
            switch (e.Type)
            {
                case AnimationType.Stable:
                    if (e.ObjIndex == game.StableWells.Count)
                    {
                        Animation redWell = new Animation(new BitmapImage[10] { wellImages[0], wellImages[1], wellImages[2], wellImages[3], wellImages[4], wellImages[5], wellImages[4], wellImages[3], wellImages[2], wellImages[1] }, new int[10] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 });
                        Animation orangeWell = new Animation(new BitmapImage[10] { wellImages[6], wellImages[7], wellImages[8], wellImages[9], wellImages[10], wellImages[11], wellImages[10], wellImages[9], wellImages[8], wellImages[7] }, new int[10] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 });
                        Animation yellowWell = new Animation(new BitmapImage[10] { wellImages[12], wellImages[13], wellImages[14], wellImages[15], wellImages[16], wellImages[17], wellImages[16], wellImages[15], wellImages[14], wellImages[13] }, new int[10] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 });
                        Animation greenWell = new Animation(new BitmapImage[10] { wellImages[18], wellImages[19], wellImages[20], wellImages[21], wellImages[22], wellImages[23], wellImages[22], wellImages[21], wellImages[20], wellImages[19] }, new int[10] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 });
                        Animation blueWell = new Animation(new BitmapImage[10] { wellImages[24], wellImages[25], wellImages[26], wellImages[27], wellImages[28], wellImages[29], wellImages[28], wellImages[27], wellImages[26], wellImages[25] }, new int[10] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 });
                        Animation purpleWell = new Animation(new BitmapImage[10] { wellImages[30], wellImages[31], wellImages[32], wellImages[33], wellImages[34], wellImages[35], wellImages[34], wellImages[33], wellImages[32], wellImages[31] }, new int[10] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 });
                        Animation transRedWell = new Animation(new BitmapImage[5] { wellImages[5], wellImages[4], wellImages[3], wellImages[2], wellImages[1] }, new int[5] { 3, 3, 3, 3, 3 });
                        Animation transOrangeWell = new Animation(new BitmapImage[5] { wellImages[11], wellImages[10], wellImages[9], wellImages[8], wellImages[7] }, new int[5] { 3, 3, 3, 3, 3 });
                        Animation transYellowWell = new Animation(new BitmapImage[5] { wellImages[17], wellImages[16], wellImages[15], wellImages[14], wellImages[13] }, new int[5] { 3, 3, 3, 3, 3 });
                        Animation transGreenWell = new Animation(new BitmapImage[5] { wellImages[23], wellImages[22], wellImages[21], wellImages[20], wellImages[19] }, new int[5] { 3, 3, 3, 3, 3 });
                        Animation transBlueWell = new Animation(new BitmapImage[5] { wellImages[29], wellImages[28], wellImages[27], wellImages[26], wellImages[25] }, new int[5] { 3, 3, 3, 3, 3 });
                        Animation transPurpleWell = new Animation(new BitmapImage[5] { wellImages[35], wellImages[34], wellImages[33], wellImages[32], wellImages[31] }, new int[5] { 3, 3, 3, 3, 3 });
                        Animator anim = new Animator(DrawCanvas, new Animation[12] { redWell, orangeWell, yellowWell, greenWell, blueWell, purpleWell, transRedWell, transOrangeWell, transYellowWell, transGreenWell, transBlueWell, transPurpleWell }, 0, 5);
                        wellList.Add(anim);
                    }

                    else if (e.AnimIndex == 12)
                    {
                        wellList[e.ObjIndex].RemoveFromScreen();
                        wellList.RemoveAt(e.ObjIndex);
                    }

                    else if (e.IsTransition)
                    {
                        wellList[e.ObjIndex].Transition(e.TransitionIndex, e.AnimIndex);
                    }
                    break;
                case AnimationType.Unstable:
                    if (e.ObjIndex == game.UnstableWells.Count)
                    {
                        Animation blackhole = new Animation(new BitmapImage[24] { destabilizedImages[0], destabilizedImages[1], destabilizedImages[2],
                            destabilizedImages[3], destabilizedImages[4], destabilizedImages[5], destabilizedImages[6], destabilizedImages[7], destabilizedImages[8],
                            destabilizedImages[9], destabilizedImages[10], destabilizedImages[11], destabilizedImages[12], destabilizedImages[13], destabilizedImages[14],
                            destabilizedImages[15], destabilizedImages[16], destabilizedImages[17], destabilizedImages[18], destabilizedImages[19], destabilizedImages[20],
                            destabilizedImages[21], destabilizedImages[22], destabilizedImages[23]}, new int[24] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
                        Animator anim = new Animator(DrawCanvas, new Animation[1] { blackhole }, 0, 6);
                        destableList.Add(anim);
                    }

                    else if (e.AnimIndex == 24)
                    {
                        destableList[e.ObjIndex].RemoveFromScreen();
                        destableList.RemoveAt(e.ObjIndex);
                    }

                    break;
                default:
                    break;
            }
        }

        //Updates orb display in HUD.
        //Accepts nothing.
        //Returns nothing.
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

        //Updates powerup display in HUD.
        //Accepts nothing.
        //Returns nothing.
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

        //Activates when the game page is initialized. Finishes initializing the audio/visual elements of the page.
        //Accepts regular eventhandler args.
        //Returns nothing.
        private void GameWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //setup background images
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

            //setup sound players
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

        //Displays a message to the game screen.
        //Accepts a string to display.
        //Returns nothing.
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
