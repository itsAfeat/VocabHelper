using System.Diagnostics;
using System.Windows;

namespace VocabHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoupWindow? soupWindow;
        public static bool Restart { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

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
    }
}
