using Microsoft.VisualBasic;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VocabHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                InitialDirectory = "C:\\",
                Filter = "text file (*.txt)|*.txt|csv file(*.csv)|*.csv|Any file (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == true)
            { IO.ReadFile(ofd.FileName); }

            // Ask for grid size
            int size = int.Parse(Interaction.InputBox("Enter the size of the grid", "Set grid size", "15"));

            // Create grid
            Random r = new();
            Grid g = new()
            { ShowGridLines = true };

            for (int i = 0; i < size; i++)
            {
                g.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star)});
                g.RowDefinitions.Add(new() { Height = new(1, GridUnitType.Star) });

                for (int j = 0; j < size; j++)
                {
                    TextBlock block = new()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text = Convert.ToChar(r.Next(65, 91)).ToString()
                    };

                    Grid.SetColumn(block, j);
                    Grid.SetRow(block, i);
                    g.Children.Add(block);
                }
            }

            this.Content = g;
        }
    }
}
