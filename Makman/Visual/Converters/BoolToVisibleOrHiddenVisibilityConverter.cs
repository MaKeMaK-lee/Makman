
using Microsoft.WindowsAPICodePack.Shell;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Makman.Visual.Converters
{
    [ValueConversion(typeof(bool), typeof(object))]
    public class BoolToVisibleOrHiddenVisibilityConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) 
                return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
