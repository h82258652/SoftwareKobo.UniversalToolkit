using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.AwaitableUI
{
    public static class FrameworkElementExtensions
    {
        public static Task WaitForLoadedAsync(this FrameworkElement frameworkElement)
        {
            return EventAsync.FromRoutedEvent(handler => frameworkElement.Loaded += handler, handler => frameworkElement.Loaded -= handler);
        }
    }
}