using Newtonsoft.Json;
using SoftwareKobo.UniversalToolkit.Extensions;
using System;
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
                var type = typeof(T);
                // 存储的值。
                var storeObject = _container.Values[key];
                if (IsSupportType(type))
                {
                    return (T)storeObject;
                }
                else
                {
                    if (type.IsEnum())
                    {
                        // 枚举类型。
                        var underlyingType = Enum.GetUnderlyingType(type);
                        if (IsSupportType(underlyingType))
                        {
                            // 枚举类型基类支持直接存储，从基类型转换为枚举类型。
                            return (T)Enum.Parse(typeof(T), storeObject.ToString());
                        }
                        else
                        {
                            // 枚举类型基类不支持直接存储，使用 Json 反序列化。
                            return JsonConvert.DeserializeObject<T>((string)storeObject);
                        }
                    }
                    else
                    {
                        // 不支持直接存储，使用 Json 反序列化。
                        return JsonConvert.DeserializeObject<T>((string)storeObject);
                    }
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
            var type = typeof(T);
            // 即将存储的值。
            object storeObject;
            if (IsSupportType(type))
            {
                // 支持直接存储。
                storeObject = value;
            }
            else
            {
                if (type.IsEnum())
                {
                    // 枚举类型。
                    var underlyingType = Enum.GetUnderlyingType(type);
                    if (IsSupportType(underlyingType))
                    {
                        // 枚举类型基类支持直接存储，转换为基类型。
                        storeObject = Convert.ChangeType(value, underlyingType);
                    }
                    else
                    {
                        // 枚举类型基类不支持直接存储，使用 Json 序列化。
                        storeObject = JsonConvert.SerializeObject(value);
                    }
                }
                else
                {
                    // 不支持直接存储，使用 Json 序列化。
                    storeObject = JsonConvert.SerializeObject(value);
                }
            }

            _container.Values[key] = storeObject;
        }

        private bool IsSupportType(Type type)
        {
            if (type == typeof(bool)
                || type == typeof(byte)
                || type == typeof(short)
                || type == typeof(ushort)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(char)
                || type == typeof(string))
            {
                // 不支持 sbyte 和 decimal
                return true;
            }
            return false;
        }
    }
}