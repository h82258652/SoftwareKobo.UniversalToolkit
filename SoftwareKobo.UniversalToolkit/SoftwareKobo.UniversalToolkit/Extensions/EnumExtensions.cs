using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<TEnum> GetValues<TEnum>()
        {
            if (typeof(TEnum).GetTypeInfo().IsEnum == false)
            {
                throw new InvalidOperationException("T is not enum type.");
            }

            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        public static bool IsDefined<TEnum>(object value)
        {
            return Enum.IsDefined(typeof(TEnum), value);
        }
    }
}