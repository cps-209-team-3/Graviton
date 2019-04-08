using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GravitonClient
{
    /// <summary>
    /// Interaction logic for HighScoreWindow.xaml
    /// </summary>
    public partial class HighScoreWindow : Window
    {
        Window parentWindow;
        HighScores hs;

        public HighScoreWindow(Window window)
        {
            InitializeComponent();
            parentWindow = window;
            string parentDir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\");
            hs = HighScores.Load(System.IO.Path.Combine(parentDir, "Saves/HighScoreSave.txt"));
        }

        private void HighScoresWindow_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; ++i)
            {
                Grid g = new Grid();
                ColumnDefinition gridCol1 = new ColumnDefinition();
                ColumnDefinition gridCol2 = new ColumnDefinition();
                ColumnDefinition gridCol3 = new ColumnDefinition();
                g.ColumnDefinitions.Add(gridCol1);
                g.ColumnDefinitions.Add(gridCol2);
                g.ColumnDefinitions.Add(gridCol3);

                TextBlock userTxt = new TextBlock();
                userTxt.FontFamily = (FontFamily)FindResource("Azonix");
                userTxt.FontSize = 20;
                userTxt.Margin = new Thickness(75, 5, 0, 5);
                userTxt.Width = 300;
                userTxt.HorizontalAlignment = HorizontalAlignment.Left;
                userTxt.Foreground = Brushes.Aqua;

                TextBlock scoreTxt = new TextBlock();
                scoreTxt.FontFamily = (FontFamily)FindResource("Azonix");
                scoreTxt.FontSize = 20;
                scoreTxt.Margin = new Thickness(0, 5, 125, 5);
                scoreTxt.Width = 200;
                scoreTxt.HorizontalAlignment = HorizontalAlignment.Right;
                scoreTxt.Foreground = Brushes.Aqua;

                if (i < hs.HiScores.Count)
                {
                    userTxt.Text = hs.HiScores[i].User;
                    scoreTxt.Text = hs.HiScores[i].Score.ToString();
                }

                else
                {
                    userTxt.Text = "---";
                    scoreTxt.Text = "---";
                }
                
                Grid.SetColumn(userTxt, 0);
                Grid.SetRow(userTxt, 0);
                Grid.SetColumn(scoreTxt, 2);
                Grid.SetRow(scoreTxt, 0);

                g.Children.Add(userTxt);
                g.Children.Add(scoreTxt);

                Line l = new Line();
                l.Stretch = Stretch.Fill;
                l.Stroke = Brushes.Aqua;
                l.X2 = 1;
                l.StrokeThickness = 2;

                pnlScores.Children.Add(g);
                pnlScores.Children.Add(l);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
            Close();
        }

        private void HSWindow_Closed(object sender, EventArgs e)
        {
            parentWindow.Show();
        }
    }
}
