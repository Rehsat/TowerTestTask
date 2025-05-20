using System.Collections.Generic;

namespace Game.Services.LogService
{
    public class LocalizableLogData
    {
        private List<string> _localizableStrings;

        public List<string> LocalizableStrings => _localizableStrings;

        public LocalizableLogData(List<string> localizableStrings)
        {
            _localizableStrings = localizableStrings;
        }
    }
}