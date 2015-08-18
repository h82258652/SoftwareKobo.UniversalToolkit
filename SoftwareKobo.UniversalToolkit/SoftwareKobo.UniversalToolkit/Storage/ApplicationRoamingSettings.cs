namespace SoftwareKobo.UniversalToolkit.Storage
{
    public static class ApplicationRoamingSettings
    {
        public static bool Exists(string key)
        {
            return ApplicationSettings.Exist(key, SettingsStrategy.Roaming);
        }

        public static T Read<T>(string key)
        {
            return ApplicationSettings.Read<T>(key, SettingsStrategy.Roaming);
        }

        public static bool Remove(string key)
        {
            return ApplicationSettings.Remove(key, SettingsStrategy.Roaming);
        }

        public static void Write<T>(string key, T value)
        {
            ApplicationSettings.Write(key, value, SettingsStrategy.Roaming);
        }
    }
}