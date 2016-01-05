using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.AwaitableUI
{
    internal class RoutedEventHandlerTaskSource
    {
        private readonly Action<RoutedEventHandler> _removeEventHandler;

        private readonly TaskCompletionSource<RoutedEventArgs> _tcs;

        internal RoutedEventHandlerTaskSource(Action<RoutedEventHandler> addEventHandler, Action<RoutedEventHandler> removeEventHandler) : this()
        {
            if (addEventHandler == null)
            {
                throw new ArgumentNullException(nameof(addEventHandler));
            }
            if (removeEventHandler == null)
            {
                throw new ArgumentNullException(nameof(removeEventHandler));
            }

            _removeEventHandler = removeEventHandler;
            addEventHandler(EventCompleted);
        }

        internal RoutedEventHandlerTaskSource(Action<RoutedEventHandler> addEventHandler, Action<RoutedEventHandler> removeEventHandler, Action beginAction) : this(addEventHandler, removeEventHandler)
        {
            if (beginAction != null)
            {
                beginAction();
            }
        }

        private RoutedEventHandlerTaskSource()
        {
            _tcs = new TaskCompletionSource<RoutedEventArgs>();
        }

        internal Task<RoutedEventArgs> Task => _tcs.Task;

        private void EventCompleted(object sender, RoutedEventArgs args)
        {
            _removeEventHandler(EventCompleted);
            _tcs.SetResult(args);
        }
    }
}