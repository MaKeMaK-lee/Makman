
using Microsoft.WindowsAPICodePack.Shell;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Makman.Visual.Converters
{
    [ValueConversion(typeof(string), typeof(object))]
    public class FilePathToImageConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            string path = (string)value;
            if (string.IsNullOrEmpty(path))
                return null;
            if (!File.Exists(path)) 
                return null; 


            var shellObject = ShellObject.FromParsingName(path);
            string contentType = (string)shellObject.Properties.GetProperty("System.ContentType").ValueAsObject;

            if (contentType.StartsWith("image"))
            {
                try
                {
                    return new BitmapImage(new Uri(path));
                }
                catch (Exception)
                {

                }
            }

            return shellObject.Thumbnail.ExtraLargeBitmapSource;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
