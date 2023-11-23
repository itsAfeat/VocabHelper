using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace VocabHelper
{
    [ValueConversion(typeof(string), typeof(string))]
    public class LocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameters, CultureInfo culture)
        {
            string paramStr = (string)parameters;
            if (!string.IsNullOrEmpty(paramStr))
            {
                string[] paramArr = paramStr.Split('|');
                return "";
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
