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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GravitonClient.view
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        bool cheat;

        public HomePage()
        {
            InitializeComponent();
            cheat = false;
        }

        private void BtnHighScores_Click(object sender, RoutedEventArgs e)
        {
            HighScoreWindow hs = new HighScoreWindow();
            hs.Show();
            App.Current.MainWindow.Hide();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }
        
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PlayPage(this));
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow h = new HelpWindow();
            h.Show();
            App.Current.MainWindow.Hide();
        }
    }
}