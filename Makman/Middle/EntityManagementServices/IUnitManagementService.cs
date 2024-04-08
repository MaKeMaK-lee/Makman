using Makman.Middle.Entities;

namespace Makman.Middle.EntityManagementServices
{
    public interface IUnitManagementService
    {
        /// <returns>Созданные сущности</returns>
        IEnumerable<Unit> CreateMany(IEnumerable<string> fullFileNames);

        /// <returns>Количество обнаруженных в директориях, но не добавленных файлов</returns>
        int AddUnitsFromFilesOfFolderAndSyncToDatabase(string directoryPath, bool allowSubDirectories = true);
    }
}
