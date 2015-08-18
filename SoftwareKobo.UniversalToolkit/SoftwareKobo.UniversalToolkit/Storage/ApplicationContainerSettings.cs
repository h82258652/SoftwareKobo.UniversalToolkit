using Newtonsoft.Json;
using Windows.Storage;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    public class ApplicationContainerSettings : ISettings
    {
        private ApplicationDataContainer _container;

        public ApplicationContainerSettings(ApplicationDataContainer container)
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
                string json = (string)_container.Values[key];
                return JsonConvert.DeserializeObject<T>(json);
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
            string json = JsonConvert.SerializeObject(value);
            _container.Values[key] = json;
        }
    }
}