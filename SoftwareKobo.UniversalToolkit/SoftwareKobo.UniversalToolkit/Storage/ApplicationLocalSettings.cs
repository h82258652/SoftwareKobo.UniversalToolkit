namespace SoftwareKobo.UniversalToolkit.Storage
{
    public static class ApplicationLocalSettings
    {
        public static bool Exists(string key)
        {
            return ApplicationSettings.Exist(key, SettingsStrategy.Local);
        }

        public static T Read<T>(string key)
        {
            return ApplicationSettings.Read<T>(key, SettingsStrategy.Local);
        }

        public static bool Remove(string key)
        {
            return ApplicationSettings.Remove(key, SettingsStrategy.Local);
        }

        public static void Write<T>(string key, T value)
        {
            ApplicationSettings.Write(key, value, SettingsStrategy.Local);
        }
    }
}