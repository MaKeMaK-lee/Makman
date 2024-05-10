 
using System.Windows.Media;

namespace Makman.Middle.Services
{
    public class ConvertService(IFileSystemAccessService fileSystemAccessService) : IConvertService
    {
        IFileSystemAccessService _fileSystemAccessService = fileSystemAccessService;

        public object? FilePathToImage(object? value, bool justThumbnail = false)
        {
            if (value == null)
                return null;
            string path = (string)value;
            if (string.IsNullOrEmpty(path))
                return null;
            if (!_fileSystemAccessService.FileExists(path))
                return null;

            return _fileSystemAccessService.GetImageOfFile(path, justThumbnail);

        }

        public object? StringToColorBrush(object? value)
        {
            string? stringValue = (string?)value;
            if (String.IsNullOrEmpty(stringValue))
                return Colors.Black;
            try
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(stringValue));
            }
            catch (Exception)
            {
                return Colors.Black;
            }
        }

    }
}
