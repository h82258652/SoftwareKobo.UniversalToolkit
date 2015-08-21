using System;
using System.Linq;
using Windows.Storage;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    public static class ApplicationSettings
    {
        public static bool Exist(string key, SettingsStrategy strategy)
        {
            return GetSettingsContainer(strategy).Exist(key);
        }

        public static T Read<T>(string key, SettingsStrategy strategy)
        {
            return GetSettingsContainer(strategy).Read<T>(key);
        }

        public static bool Remove(string key, SettingsStrategy strategy)
        {
            return GetSettingsContainer(strategy).Remove(key);
        }

        public static void Write<T>(string key, T value, SettingsStrategy strategy)
        {
            GetSettingsContainer(strategy).Write(key, value);
        }

        private static ISettings GetSettingsContainer(SettingsStrategy strategy)
        {
            switch (strategy)
            {
                case SettingsStrategy.Local:
                    return new ApplicationContainerSettings(ApplicationData.Current.LocalSettings);

                case SettingsStrategy.Roaming:
                    return new ApplicationContainerSettings(ApplicationData.Current.RoamingSettings);

                case SettingsStrategy.Temp:
                    return new ApplicationFileSettings(ApplicationTempSettings.TempSettingsFolder);

                default:
                    throw new ArgumentException("unknown settings strategy", nameof(strategy));
            }
        }

        internal static bool IsPrimitive<T>()
        {
            return IsPrimitive(typeof(T));
        }

        private static readonly Type[] primitives = new Type[]
        {
            typeof(int),
            typeof(string),
        };

        internal static bool IsPrimitive(Type type)
        {
            return primitives.Contains(type);
        }
    }
}