using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        { InitializeComponent(); }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CSVFile csv = new();
            OpenFileDialog ofd = new()
            {
                InitialDirectory = "C:\\",
                Filter = "text file (*.txt)|*.txt|csv file(*.csv)|*.csv|Any file (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == true)
            { csv = new(ofd.FileName); }
            else
            { Environment.Exit(0); }

            // Ask for grid size
            string inputStr = Interaction.InputBox("Enter the size of the grid\n(format: x,y)", "Set grid size", "15,15");
            if (!string.IsNullOrEmpty(inputStr))
            {
                string[] sizeStr = inputStr.Split(',');
                int sizeX = int.Parse(sizeStr[0]);
                int sizeY = int.Parse(sizeStr[1]);

                Grid soupGrid = new();
                Wordsoup wordsoup = new(soupGrid, sizeX, sizeY, csv);

                this.Title = $"Ordsuppe ({csv.GetLocalName()} -> {csv.GetForeignName()})";

                wordsoup.CreateSoup();
                wordsoup.FillGridWithWords(labelPanel);
                Grid.SetRow(soupGrid, 0);
                Grid.SetColumn(soupGrid, 1);

                mainGrid.Children.Add(soupGrid);
            }
            else
            { Environment.Exit(0); }
        }
    }
}
