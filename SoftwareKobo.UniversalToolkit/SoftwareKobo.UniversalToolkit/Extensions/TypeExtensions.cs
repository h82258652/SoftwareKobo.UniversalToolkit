using System;
using System.Reflection;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsEnum(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetTypeInfo().IsEnum;
        }

        public static bool IsNullable(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}