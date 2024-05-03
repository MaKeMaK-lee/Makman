
using Microsoft.WindowsAPICodePack.Shell;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Makman.Visual.Converters
{
    [ValueConversion(typeof(string), typeof(object))]
    public class FilePathToThumbImageConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) { return null; }
            string path = (string)value;
            if (string.IsNullOrEmpty(path)) { return null; }
           
            //if (parameter == null)
            //{
            ////string fullFileName = (string)value;
            ////long fileSize = (long)parameter;
            ////string fileExtention = fullFileName.Split('.').Last();

            ////if ((fileExtention == "jpg" || fileExtention == "jpeg" || fileExtention == "png") && fileSize < 1024 * 1024)
            ////    return fullFileName;

            var shellObject = ShellObject.FromParsingName(path);
            ////ulong size = (ulong)shellObject.Properties.GetProperty("System.Size").ValueAsObject;
            ////string contentType = (string)shellObject.Properties.GetProperty("System.ContentType").ValueAsObject;

            ////if (contentType.StartsWith("image") && size < 1024 * 1024)
            ////    return fullFileName;

            return shellObject.Thumbnail.MediumBitmapSource;
            //}
            //if (!string.IsNullOrEmpty((string)parameter))
            //{
            //    if ((string)parameter == "MaxPreview")
            //    {
            //        if (!string.IsNullOrEmpty(path))
            //        {
            //            var shellObject = ShellObject.FromParsingName(path);
            //            return shellObject.Thumbnail.ExtraLargeBitmapSource;
            //        }
            //        return null;
            //    }
            //}
            //return path;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
