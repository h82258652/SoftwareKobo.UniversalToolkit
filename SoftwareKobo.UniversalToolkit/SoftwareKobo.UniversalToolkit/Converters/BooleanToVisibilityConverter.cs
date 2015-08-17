using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SoftwareKobo.UniversalToolkit.Converters
{
    /// <summary>
    /// 表示将布尔值与 Visibility 枚举值相互转换的转换器。
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 如果为真，则转换 Visibility 到 Boolean。
        /// </summary>
        public bool IsReversed
        {
            get;
            set;
        }

        /// <summary>
        /// 如果为真，则转换 True 为 Collapsed，转换 False 为 Visible。
        /// </summary>
        public bool IsInversed
        {
            get;
            set;
        }

        /// <summary>
        /// 将布尔值转换为 Visibility 枚举值。
        /// </summary>
        /// <param name="value">要转换的布尔值。此值可以是标准布尔值或可以为 null 的布尔值。</param>
        /// <param name="targetType">未使用此参数。</param>
        /// <param name="parameter">未使用此参数。</param>
        /// <param name="language">未使用此参数。</param>
        /// <returns>如果 value 为 true，则为 Visibility.Visible；否则为 Visibility.Collapsed。</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (IsReversed)
            {
                bool bValue = false;
                if (value is Visibility)
                {
                    bValue = (Visibility)value == Visibility.Visible;
                }
                if (IsInversed)
                {
                    bValue = !bValue;
                }
                return bValue;
            }
            else
            {
                Visibility visibility = Visibility.Collapsed;
                if (value is bool)
                {
                    visibility = (bool)value ? Visibility.Visible : Visibility.Collapsed;
                }
                if (IsInversed)
                {
                    if (visibility == Visibility.Collapsed)
                    {
                        visibility = Visibility.Visible;
                    }
                    else
                    {
                        visibility = Visibility.Collapsed;
                    }
                }
                return visibility;
            }
        }

        /// <summary>
        /// 将 Visibility 枚举值转换为布尔值。
        /// </summary>
        /// <param name="value">一个 Visibility 枚举值。</param>
        /// <param name="targetType">未使用此参数。</param>
        /// <param name="parameter">未使用此参数。</param>
        /// <param name="language">未使用此参数。</param>
        /// <returns>如果 value 为 Visible，则为 true；否则为 false。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (IsReversed)
            {
                Visibility visibility = Visibility.Collapsed;
                if (value is bool)
                {
                    visibility = (bool)value ? Visibility.Visible : Visibility.Collapsed;
                }
                if (IsInversed)
                {
                    if (visibility == Visibility.Collapsed)
                    {
                        visibility = Visibility.Visible;
                    }
                    else
                    {
                        visibility = Visibility.Collapsed;
                    }
                }
                return visibility;
            }
            else
            {
                bool bValue = false;
                if (value is Visibility)
                {
                    bValue = (Visibility)value == Visibility.Visible;
                }
                if (IsInversed)
                {
                    bValue = !bValue;
                }
                return bValue;
            }
        }
    }
}