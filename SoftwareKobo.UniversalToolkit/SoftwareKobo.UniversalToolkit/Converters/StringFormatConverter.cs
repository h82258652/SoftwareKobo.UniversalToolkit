using System;
using Windows.UI.Xaml.Data;

namespace SoftwareKobo.UniversalToolkit.Converters
{
    /// <summary>
    /// 格式化字符串转换器。
    /// </summary>
    public class StringFormatConverter : IValueConverter
    {
        /// <summary>
        /// 使用 ConverterParameter 格式化 value。
        /// </summary>
        /// <param name="value">需要格式化的值。</param>
        /// <param name="targetType">未使用该参数。</param>
        /// <param name="parameter">格式化字符串。</param>
        /// <param name="language">未使用该参数。</param>
        /// <returns>格式化后的字符串。</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null)
            {
                return value;
            }

            return string.Format((string)parameter, value);
        }

        /// <summary>
        /// 返回格式化后的字符串。
        /// </summary>
        /// <param name="value">格式化后的字符串。</param>
        /// <param name="targetType">未使用该参数。</param>
        /// <param name="parameter">未使用该参数。</param>
        /// <param name="language">未使用该参数。</param>
        /// <returns>格式化后的字符串。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}