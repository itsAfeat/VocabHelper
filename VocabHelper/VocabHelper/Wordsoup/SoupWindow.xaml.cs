using System.Windows;
using System.Windows.Controls;

namespace VocabHelper.Wordsoup
{
    /// <summary>
    /// Interaction logic for SoupWindow.xaml
    /// </summary>
    public partial class SoupWindow : Window
    {
        private Grid? soupGrid;

        public SoupWindow()
        { InitializeComponent(); }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SoupInputBox inputBox = new();
            inputBox.ShowDialog();

            if (SoupInputBox.CSVFilePath != null && SoupInputBox.SizeX != null && SoupInputBox.SizeY != null)
            {
                CSVFile csv = SoupInputBox.CSVFilePath;
                int sizeX = (int)SoupInputBox.SizeX;
                int sizeY = (int)SoupInputBox.SizeY;

                soupGrid = new();
                WordsoupMaker wordsoup = new(soupGrid, sizeX, sizeY, csv, labelPanel);

                this.Title = $"Ordsuppe ({csv.GetLocalName()} -> {csv.GetForeignName()})";

                wordsoup.CreateSoup();
                wordsoup.FillGridWithWords();
                Grid.SetRow(soupGrid, 0);
                Grid.SetColumn(soupGrid, 2);

                mainGrid.Children.Add(soupGrid);
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F1 && soupGrid != null)
            { soupGrid.ShowGridLines = !soupGrid.ShowGridLines; }
        }
    }
}
