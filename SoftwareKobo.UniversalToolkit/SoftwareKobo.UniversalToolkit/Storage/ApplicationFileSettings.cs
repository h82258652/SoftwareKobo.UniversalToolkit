using Newtonsoft.Json;
using System;
using Windows.Storage;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    internal class ApplicationFileSettings : ISettings
    {
        private StorageFolder _folder;

        internal ApplicationFileSettings(StorageFolder folder)
        {
            _folder = folder;
        }

        public bool Exist(string key)
        {
            try
            {
                var file = _folder.GetFileAsync(key).AsTask().Result;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public T Read<T>(string key)
        {
            try
            {
                var file = _folder.GetFileAsync(key).AsTask().Result;
                var str = FileIO.ReadTextAsync(file).AsTask().Result;
                if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(str, typeof(T));
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(str);
                }
            }
            catch
            {
                return default(T);
            }
        }

        public bool Remove(string key)
        {
            try
            {
                var file = _folder.GetFileAsync(key).AsTask().Result;
                file.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask().Wait();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Write<T>(string key, T value)
        {
            try
            {
                string str;
                if (typeof(T) == typeof(string))
                {
                    str = value.ToString();
                }
                else
                {
                    str = JsonConvert.SerializeObject(value);
                }
                var file = _folder.GetFileAsync(key).AsTask().Result;
                FileIO.WriteTextAsync(file, str).AsTask().Wait();
            }
            catch
            {
            }
        }
    }
}