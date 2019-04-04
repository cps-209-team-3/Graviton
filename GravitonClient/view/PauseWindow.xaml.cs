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

namespace GravitonClient.view
{
    /// <summary>
    /// Interaction logic for PauseWindow.xaml
    /// </summary>
    public partial class PauseWindow : Window
    {
        //private Game Game { get; set; } ?

        public PauseWindow()
        {
            InitializeComponent();
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            //Game.Timer.Start();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            //save the game
            //close game window and return to main menu
            Close();
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            //load previous game
            Close();
        }
    }
}
