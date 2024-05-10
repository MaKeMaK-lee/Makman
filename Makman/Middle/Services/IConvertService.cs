 
namespace Makman.Middle.Services
{
    public interface IConvertService
    {
        object? FilePathToImage(object? value, bool justThumbnail = false);

        object? StringToColorBrush(object? value);
    }
}
