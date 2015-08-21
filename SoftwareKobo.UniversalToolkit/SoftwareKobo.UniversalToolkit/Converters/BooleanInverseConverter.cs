using System;
using Windows.UI.Xaml.Data;

namespace SoftwareKobo.UniversalToolkit.Converters
{
    /// <summary>
    /// 反转布尔值转换器。
    /// </summary>
    public class BooleanInverseConverter : IValueConverter
    {
        /// <summary>
        /// 布尔值取反。
        /// </summary>
        /// <param name="value">需要取反的布尔值。</param>
        /// <param name="targetType">未使用该参数。</param>
        /// <param name="parameter">未使用该参数。</param>
        /// <param name="language">未使用该参数。</param>
        /// <returns>取反后的布尔值。</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value == false;
        }

        /// <summary>
        /// 布尔值取反。
        /// </summary>
        /// <param name="value">需要取反的布尔值。</param>
        /// <param name="targetType">未使用该参数。</param>
        /// <param name="parameter">未使用该参数。</param>
        /// <param name="language">未使用该参数。</param>
        /// <returns>取反后的布尔值。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (bool)value == false;
        }
    }
}