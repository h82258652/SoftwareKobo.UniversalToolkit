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
        /// 当 Convert 方法中的 parameter 不是字符串类型或者为空时，才使用该参数。
        /// </summary>
        public string DefaultFormat
        {
            get;
            set;
        }

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
            var format = parameter as string ?? DefaultFormat;
            if (format == null)
            {
                return value;
            }
            return string.Format(format, value);
        }

        /// <summary>
        /// 未实现。
        /// </summary>
        /// <param name="value">未使用该参数。</param>
        /// <param name="targetType">未使用该参数。</param>
        /// <param name="parameter">未使用该参数。</param>
        /// <param name="language">未使用该参数。</param>
        /// <returns>未实现。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}