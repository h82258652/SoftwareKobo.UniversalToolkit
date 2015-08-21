using Newtonsoft.Json;
using Windows.Storage;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    internal class ApplicationContainerSettings : ISettings
    {
        private ApplicationDataContainer _container;

        internal ApplicationContainerSettings(ApplicationDataContainer container)
        {
            _container = container;
        }

        public bool Exist(string key)
        {
            return _container.Values.ContainsKey(key);
        }

        public T Read<T>(string key)
        {
            try
            {
                if (ApplicationSettings.IsPrimitive<T>())
                {
                    return (T)_container.Values[key];
                }
                else
                {
                    string json = (string)_container.Values[key];
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch
            {
                return default(T);
            }
        }

        public bool Remove(string key)
        {
            return _container.Values.Remove(key);
        }

        public void Write<T>(string key, T value)
        {
            if (ApplicationSettings.IsPrimitive<T>())
            {
                _container.Values[key] = value;
            }
            else
            {
                string json = JsonConvert.SerializeObject(value);
                _container.Values[key] = json;
            }
        }
    }
}