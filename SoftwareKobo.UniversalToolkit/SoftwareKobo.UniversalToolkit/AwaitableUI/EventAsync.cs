using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.AwaitableUI
{
    public static class EventAsync
    {
        public static Task<object> FromEvent<TEventArgs>(Action<EventHandler<TEventArgs>> addEventHandler, Action<EventHandler<TEventArgs>> removeEventHandler)
        {
            return FromEvent(addEventHandler, removeEventHandler, null);
        }

        public static Task<object> FromEvent<TEventArgs>(Action<EventHandler<TEventArgs>> addEventHandler, Action<EventHandler<TEventArgs>> removeEventHandler, Action beginAction)
        {
            return new EventHandlerTaskSource<TEventArgs>(addEventHandler, removeEventHandler, beginAction).Task;
        }

        public static Task<RoutedEventArgs> FromRoutedEvent(Action<RoutedEventHandler> addEventHandler, Action<RoutedEventHandler> removeEventHandler)
        {
            return FromRoutedEvent(addEventHandler, removeEventHandler);
        }

        public static Task<RoutedEventArgs> FromRoutedEvent(Action<RoutedEventHandler> addEventHandler, Action<RoutedEventHandler> removeEventHandler, Action beginAction)
        {
            return new RoutedEventHandlerTaskSource(addEventHandler, removeEventHandler, beginAction).Task;
        }
    }
}