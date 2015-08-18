using Newtonsoft.Json;
using System;
using Windows.Storage;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    internal class ApplicationFileSettings : ISettings
    {
        private StorageFolder _folder;

        public ApplicationFileSettings(StorageFolder folder)
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
                string json = FileIO.ReadTextAsync(file).AsTask().Result;
                return JsonConvert.DeserializeObject<T>(json);
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
                string json = JsonConvert.SerializeObject(value);
                var file = _folder.GetFileAsync(key).AsTask().Result;
                FileIO.WriteTextAsync(file, json).AsTask().Wait();
            }
            catch
            {
            }
        }
    }
}