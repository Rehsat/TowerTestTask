namespace Game.Services.Localization
{
    public class MockLocalizationService : ILocalizationService
    {
        public string Localize(string localizableString)
        {
            return localizableString;
        }
    }
}