using Makman.Middle.Entities;

namespace Makman.Middle.EntityManagementServices
{
    public interface IUnitManagementService
    {
        /// <returns>Созданные сущности</returns>
        IEnumerable<Unit> CreateMany(IEnumerable<string> fullFileNames);

        /// <returns>Количество обнаруженных в директориях, но не добавленных файлов</returns>
        int AddUnitsFromFilesOfFolderAndSyncToDatabase(string directoryPath, bool allowSubDirectories = true);

        public Bunch? GetBunchOf(IEnumerable<Unit> units);

        bool IsSameOrNullBunch(IEnumerable<Unit>? units);

        void BindToBunch(IEnumerable<Unit> bindingUnits, Bunch? nullableBunch);

        /// <returns>TRUE if binded</returns>
        bool TryBindToBunch(IEnumerable<Unit>? bindingUnits, IEnumerable<Unit>? exampleUnits, bool includeExamples = false);
    }
}
