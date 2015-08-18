using System;
using Windows.Storage;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    public static class ApplicationTempSettings
    {
        private static StorageFolder _tempSettingsFolder;

        internal static StorageFolder TempSettingsFolder
        {
            get
            {
                if (_tempSettingsFolder == null)
                {
                    _tempSettingsFolder = ApplicationData.Current.TemporaryFolder.CreateFolderAsync("TempSettings").AsTask().Result;
                }
                return _tempSettingsFolder;
            }
        }

        public static bool Exist(string key)
        {
            return ApplicationSettings.Exist(key, SettingsStrategy.Temp);
        }

        public static T Read<T>(string key)
        {
            return ApplicationSettings.Read<T>(key, SettingsStrategy.Temp);
        }

        public static bool Remove(string key)
        {
            return ApplicationSettings.Remove(key, SettingsStrategy.Temp);
        }

        public static void Write<T>(string key, T value)
        {
            ApplicationSettings.Write(key, value, SettingsStrategy.Temp);
        }
    }
}