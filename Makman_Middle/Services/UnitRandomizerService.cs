using Makman_Entities;
using RandomOrg.CoreApi;

namespace Makman_Middle.Services
{
    public class UnitRandomizerService
    {
        ISettingsService _settingsService;

        RandomOrgClient randomOrgClient;

        public UnitRandomizerService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            randomOrgClient = RandomOrgClient.GetRandomOrgClient(_settingsService.CurrentSettings.RandomOrgApiKey);
        }

        /// <summary>
        /// Получает случайные юниты
        /// </summary>
        /// <param name="units">Из юнитов</param>
        /// <param name="count">Кол-во случайных</param>
        /// <returns></returns>
        public IEnumerable<Unit> GetRandomUnits(IEnumerable<Unit> units, int count)
        {
            HashSet<int> randomIndexes = [.. randomOrgClient.GenerateIntegers(count, 0, units.Count() - 1, false)];
            var randomUnits = new List<Unit>(count);

            //Извлечение из исходного списка по индексу, если поддерживается
            if (units is IList<Unit> || units is IReadOnlyList<Unit>)
            {
                var unitsList = units as IReadOnlyList<Unit>;
                foreach (var pos in randomIndexes)
                {
                    randomUnits.Add(unitsList![pos]);
                }
            }
            else //Извлечение из исходного списка по перебором и проверкой
            {
                int index = 0;
                foreach (var unit in units)
                {
                    if (randomIndexes.Contains(index))
                    {
                        randomUnits.Add(unit);
                    }
                    index++;
                }
            }

            return randomUnits;
        }
    }
}
