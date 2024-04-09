 
using System.Windows.Data;
using System.Windows.Media;

namespace Makman.Visual.Converters
{
    [ValueConversion(typeof(string), typeof(object))]
    public class StringColoroBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string stringValue = (string)value;
            if (String.IsNullOrEmpty(stringValue)) { return Colors.Black; }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(stringValue));
        }

        public object? ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
