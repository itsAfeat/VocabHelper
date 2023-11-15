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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.Win32;

namespace VocabHelper
{
    /// <summary>
    /// Interaction logic for SoupInputBox.xaml
    /// </summary>
    public partial class SoupInputBox : Window
    {
        public static int? SizeX = null;
        public static int? SizeY = null;
        public static int? WordAmount = null;
        public static CSVFile? CSVFilePath = null;

        private int recommendedAmount = 0;

        private void pickFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                InitialDirectory = "C:\\",
                Filter = "text file (*.txt)|*.txt|csv file(*.csv)|*.csv|Any file (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == true)
            {
                CSVFilePath = new(ofd.FileName);
                fileLocationLabel.Text = ofd.SafeFileName;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            SizeX = int.Parse(sizeXBox.Text.Trim());
            SizeY = int.Parse(sizeYBox.Text.Trim());
            WordAmount = int.Parse(amountBox.Text.Trim());

            this.Close();
        }

        private void TextBox_OnlyNumberCheck(object sender, TextCompositionEventArgs e)
        { e.Handled = IsNumber(e.Text); }

        public SoupInputBox()
        {
            InitializeComponent();
            WordAmount = 10;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (SizeX == null || SizeY == null || WordAmount == null || CSVFilePath == null)
            { Environment.Exit(0); }
        }

        private bool IsNumber(string str)
        {
            Regex reg = new("[^0-9]");
            Debug.WriteLine(reg.Match(str).Value);
            return reg.IsMatch(str);
        }

        private void sizeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sizeXBox != null && sizeYBox != null && amountBox != null)
            {
                string xText = sizeXBox.Text;
                string yText = sizeYBox.Text;

                if (xText != string.Empty && !string.IsNullOrWhiteSpace(xText) &&
                    yText != string.Empty && !string.IsNullOrWhiteSpace(yText))
                {
                    recommendedAmount = (int.Parse(xText) + int.Parse(yText)) / 3;
                    amountBox.Text = recommendedAmount.ToString();
                    amountBox.ToolTip = $"Recommended: {recommendedAmount}";
                }
            }
        }
    }
}
