using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public static class PageExtensions
    {
        public static bool Navigate<TPage>(this Frame frame) where TPage : Page
        {
            return frame.Navigate(typeof(TPage));
        }

        public static bool Navigate<TPage>(this Frame frame, object parameter) where TPage : Page
        {
            return frame.Navigate(typeof(TPage), parameter);
        }

        public static bool Navigate<TPage>(this Frame frame, object parameter, NavigationTransitionInfo infoOverride) where TPage : Page
        {
            return frame.Navigate(typeof(TPage), parameter, infoOverride);
        }
    }
}