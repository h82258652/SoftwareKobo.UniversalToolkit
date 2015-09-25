using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetValues<T>()
        {
            if (typeof(T).GetTypeInfo().IsEnum == false)
            {
                throw new InvalidOperationException("T is not enum type.");
            }

            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}