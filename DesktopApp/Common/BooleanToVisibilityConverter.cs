﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ModernDesign
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public static BooleanToVisibilityConverter Instance { get; } = new BooleanToVisibilityConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
