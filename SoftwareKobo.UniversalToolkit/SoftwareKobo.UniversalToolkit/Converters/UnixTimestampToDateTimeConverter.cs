using System;
using Windows.UI.Xaml.Data;

namespace SoftwareKobo.UniversalToolkit.Converters
{
    /// <summary>
    /// Unix 时间戳到 DateTime 转换器。
    /// </summary>
    public class UnixTimestampToDateTimeConverter : IValueConverter
    {
        public bool IsReversed
        {
            get;
            set;
        }

        /// <summary>
        /// 转换 Unix 时间戳到 DateTime。
        /// </summary>
        /// <param name="value">Unix 时间戳。</param>
        /// <param name="targetType">未使用该参数。</param>
        /// <param name="parameter">未使用该参数。</param>
        /// <param name="language">未使用该参数。</param>
        /// <returns>转换后的 DateTime。</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (IsReversed == false)
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((long)value).ToLocalTime();
            }
            else
            {
                return (long)(((DateTime)value).ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            }
        }

        /// <summary>
        /// 转换 DateTime 到 Unix 时间戳。
        /// </summary>
        /// <param name="value">DateTime。</param>
        /// <param name="targetType">未使用该参数。</param>
        /// <param name="parameter">未使用该参数。</param>
        /// <param name="language">未使用该参数。</param>
        /// <returns>转换后的 Unix 时间戳。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (IsReversed == false)
            {
                return (long)(((DateTime)value).ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            }
            else
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((long)value).ToLocalTime();
            }
        }
    }
}