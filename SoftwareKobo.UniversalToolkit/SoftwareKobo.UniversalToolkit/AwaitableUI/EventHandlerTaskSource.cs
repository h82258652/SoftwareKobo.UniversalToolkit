using System;
using System.Threading.Tasks;

namespace SoftwareKobo.UniversalToolkit.AwaitableUI
{
    internal class EventHandlerTaskSource<TEventArgs>
    {
        private readonly Action<EventHandler<TEventArgs>> _removeEventHandler;

        private readonly TaskCompletionSource<object> _tcs;

        internal EventHandlerTaskSource(Action<EventHandler<TEventArgs>> addEventHandler, Action<EventHandler<TEventArgs>> removeEventHandler) : this()
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

        internal EventHandlerTaskSource(Action<EventHandler<TEventArgs>> addEventHandler, Action<EventHandler<TEventArgs>> removeEventHandler, Action beginAction) : this(addEventHandler, removeEventHandler)
        {
            beginAction?.Invoke();
        }

        private EventHandlerTaskSource()
        {
            _tcs = new TaskCompletionSource<object>();
        }

        internal Task<object> Task => _tcs.Task;

        private void EventCompleted(object sender, TEventArgs args)
        {
            _removeEventHandler(EventCompleted);
            _tcs.SetResult(args);
        }
    }
}