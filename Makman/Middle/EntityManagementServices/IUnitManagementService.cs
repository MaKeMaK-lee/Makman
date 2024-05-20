using Makman.Middle.Entities;

namespace Makman.Middle.EntityManagementServices
{
    public interface IUnitManagementService
    {
        /// <returns>Созданные сущности</returns>
        IEnumerable<Unit> CreateMany(IEnumerable<string> fullFileNames);

        /// <returns>Количество обнаруженных в директориях, но не добавленных файлов</returns>
        int AddUnitsFromFilesOfFolderAndSyncToDatabase(string directoryPath, bool allowSubDirectories = true);

        void AddUnitsToDatabase(IEnumerable<Unit> units);

        public Bunch? GetBunchOf(IEnumerable<Unit> units);

        bool IsSameOrNullBunch(IEnumerable<Unit>? units);


        public void UnbindTag(Unit unit, Tag removingTag);

        public void TryUnbindTag(IEnumerable<Unit>? units, Tag? removingTag);

        public void BindTag(Unit unit, Tag bindingTag);

        public void TryBindTag(IEnumerable<Unit>? units, Tag? bindingTag);

        void BindToBunch(IEnumerable<Unit> bindingUnits, Bunch? nullableBunch);

        void AddNamePostfix(IEnumerable<Unit> postfixingUnits);

        void ActualizeFileName(Unit unit);

        /// <returns>TRUE if binded</returns>
        bool TryBindToBunch(IEnumerable<Unit>? bindingUnits, IEnumerable<Unit>? exampleUnits, bool includeExamples = false);

        void MoveUnitsToDirectory(IEnumerable<Unit> units, CollectionDirectory directory, Action<string>? statusUpdateAction = null);

    }
}
