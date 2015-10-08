using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.AwaitableUI
{
    public static class FrameworkElementExtensions
    {
        public static Task WaitForLayoutUpdatedAsync(this FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
            {
                throw new ArgumentNullException(nameof(frameworkElement));
            }

            return EventAsync.FromEvent<object>(handler => frameworkElement.LayoutUpdated += handler, handler => frameworkElement.LayoutUpdated -= handler);
        }

        public static Task WaitForLoadedAsync(this FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
            {
                throw new ArgumentNullException(nameof(frameworkElement));
            }

            return EventAsync.FromRoutedEvent(handler => frameworkElement.Loaded += handler, handler => frameworkElement.Loaded -= handler);
        }

        public static async Task WaitForNonZeroSizeAsync(this FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
            {
                throw new ArgumentNullException(nameof(frameworkElement));
            }

            while (frameworkElement.ActualWidth == 0 && frameworkElement.ActualHeight == 0)
            {
                TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

                SizeChangedEventHandler handler = null;

                handler = (sender, e) =>
                {
                    frameworkElement.SizeChanged -= handler;
                    tcs.SetResult(null);
                };

                frameworkElement.SizeChanged += handler;

                await tcs.Task;
            }
        }
    }
}