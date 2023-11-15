using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

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
            SoupInputBox inputBox = new();
            inputBox.ShowDialog();

            CSVFile csv = SoupInputBox.CSVFilePath;
            int sizeX = (int)SoupInputBox.SizeX;
            int sizeY = (int)SoupInputBox.SizeY;

            Grid soupGrid = new();
            Wordsoup wordsoup = new(soupGrid, sizeX, sizeY, csv, labelPanel);

            this.Title = $"Ordsuppe ({csv.GetLocalName()} -> {csv.GetForeignName()})";

            wordsoup.CreateSoup();
            wordsoup.FillGridWithWords();
            Grid.SetRow(soupGrid, 0);
            Grid.SetColumn(soupGrid, 1);

            mainGrid.Children.Add(soupGrid);
        }
    }
}
