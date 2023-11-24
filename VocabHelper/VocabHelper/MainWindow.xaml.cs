using System.Windows;
using VocabHelper.Crossword;
using VocabHelper.Wordsoup;
using FontAwesome.Sharp;

namespace VocabHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoupWindow? soupWindow;
        private CrossWindow? crossWindow;
        public static bool Restart { get; set; }

        public MainWindow()
        { InitializeComponent(); }

        private void soupButton_Click(object sender, RoutedEventArgs e)
        {
            do
            {
                Restart = false;
                soupWindow = new();

                Application.Current.MainWindow = soupWindow;
                soupWindow.ShowDialog();
            } while (Restart);
        }

        private void crossButton_Click(object sender, RoutedEventArgs e)
        {
            do
            {
                Restart = false;
                crossWindow = new();

                Application.Current.MainWindow = crossWindow;
                crossWindow.ShowDialog();
            } while(Restart);
        }
    }
}
