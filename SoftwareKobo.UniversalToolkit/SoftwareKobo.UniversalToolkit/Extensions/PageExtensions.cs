using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    /// <summary>
    /// Page 扩展类。
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        /// 导致 Frame 加载指定的 Page 派生数据类型表示的内容。
        /// </summary>
        /// <typeparam name="TPage">要加载的内容的数据类型。</typeparam>
        /// <param name="frame">指定 Frame。</param>
        /// <returns>为 false，如果 NavigationFailed 事件处理程序将 Handled 设置为 true）；否则为 true。有关更多信息，请参见“备注”。</returns>
        public static bool Navigate<TPage>(this Frame frame) where TPage : Page
        {
            return frame.Navigate(typeof(TPage));
        }

        /// <summary>
        /// 导致 Frame 加载指定的 Page 派生数据类型表示的内容，同时传递将由导航目标解释的参数。
        /// </summary>
        /// <typeparam name="TPage">要加载的内容的数据类型。</typeparam>
        /// <param name="frame">指定 Frame。</param>
        /// <param name="parameter">要传递到目标页的导航参数；必须具有一种基本类型（字符串、字符、数值或 GUID），以便支持使用 GetNavigationState 进行参数序列化。</param>
        /// <returns>为 false，如果 NavigationFailed 事件处理程序将 Handled 设置为 true）；否则为 true。有关更多信息，请参见“备注”。</returns>
        public static bool Navigate<TPage>(this Frame frame, object parameter) where TPage : Page
        {
            return frame.Navigate(typeof(TPage), parameter);
        }

        /// <summary>
        /// 使 Frame 加载由指定的 Page 派生数据类型表示的内容，同时传递将由导航目标解释的参数和用来指示要使用的动画转换的值。
        /// </summary>
        /// <typeparam name="TPage">要加载的内容的数据类型。</typeparam>
        /// <param name="frame">指定 Frame。</param>
        /// <param name="parameter">要传递到目标页的导航参数；必须具有一种基本类型（字符串、字符、数值或 GUID），以便支持使用 GetNavigationState 进行参数序列化。</param>
        /// <param name="infoOverride">有关动画过渡的信息。</param>
        /// <returns>为 false，如果 NavigationFailed 事件处理程序将 Handled 设置为 true）；否则为 true。有关更多信息，请参见“备注”。</returns>
        public static bool Navigate<TPage>(this Frame frame, object parameter, NavigationTransitionInfo infoOverride) where TPage : Page
        {
            return frame.Navigate(typeof(TPage), parameter, infoOverride);
        }
    }
}