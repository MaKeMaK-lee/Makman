using Makman.Middle.Services;
using System.Windows.Data;

namespace Makman.Visual.Converters
{
    [ValueConversion(typeof(string), typeof(object))]
    public class FilePathToThumbImageConverter : IValueConverter, IConvertServiceConnectable
    {
        public IConvertService _ConvertService { get; set; }
        public object? Convert(object? value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return _ConvertService.FilePathToImage(value, true);
        }

        public object? ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
