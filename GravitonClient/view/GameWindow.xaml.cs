﻿using System;
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
        private Game Game { get; set; }

        public GameWindow(Game game)
        {
            Game = game;
            InitializeComponent();
            // this.KeyDown += Window_KeyDown;
            // this.KeyUp += Window_KeyUp;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Game.Timer.Stop();
                PauseWindow pauseWin = new PauseWindow(Game, this);
                pauseWin.Show();
            }
        }

        //private void Window_KeyUp(object sender, KeyEventArgs e)
        //{

        //}
    }
}
