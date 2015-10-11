using Newtonsoft.Json;
using SoftwareKobo.UniversalToolkit.Extensions;
using System;
using System.Reflection;
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
                if (IsPrimitive<T>())
                {
                    return (T)_container.Values[key];
                }
                else if (IsEnum<T>())
                {
                    return (T)Enum.Parse(typeof(T), _container.Values[key].ToString());
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
            if (IsPrimitive<T>())
            {
                _container.Values[key] = value;
            }
            else if (IsEnum<T>())
            {
                Type underlyingType = Enum.GetUnderlyingType(typeof(T));
                _container.Values[key] = Convert.ChangeType(value, underlyingType);
            }
            else
            {
                string json = JsonConvert.SerializeObject(value);
                _container.Values[key] = json;
            }
        }

        private bool IsPrimitive<T>()
        {
            Type type = typeof(T);
            if (type == typeof(bool)
                || type == typeof(sbyte)
                || type == typeof(byte)
                || type == typeof(short)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(decimal)
                || type == typeof(char)
                || type == typeof(string))
            {
                return true;
            }
            return false;
        }

        private bool IsEnum<T>()
        {
            return typeof(T).IsEnum();
        }
    }
}