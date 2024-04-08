
using Microsoft.WindowsAPICodePack.Shell;
using System.Windows.Data;

namespace Makman.Visual.Converters
{
    [ValueConversion(typeof(string), typeof(object))]
    public class FilePathToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //string fullFileName = (string)value;
            //long fileSize = (long)parameter;
            //string fileExtention = fullFileName.Split('.').Last();

            //if ((fileExtention == "jpg" || fileExtention == "jpeg" || fileExtention == "png") && fileSize < 1024 * 1024)
            //    return fullFileName;

            var shellObject = ShellObject.FromParsingName((string)value);
            //ulong size = (ulong)shellObject.Properties.GetProperty("System.Size").ValueAsObject;
            //string contentType = (string)shellObject.Properties.GetProperty("System.ContentType").ValueAsObject;

            //if (contentType.StartsWith("image") && size < 1024 * 1024)
            //    return fullFileName;

            return shellObject.Thumbnail.MediumBitmapSource;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
