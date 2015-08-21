using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit
{
    public abstract class Bootstrapper : Application
    {
        protected Bootstrapper()
        {
            this.Resuming += this.OnResuming;
            this.Suspending += async (sender, e) =>
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                try
                {
                    await this.OnSuspendingAsync(sender, e);
                }
                catch
                {
                    deferral.Complete();
                }
            };
            this.UnhandledException += this.OnUnhandledException;
        }

        protected virtual Task OnSuspendingAsync(object sender, SuspendingEventArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual void OnResuming(object sender, object e)
        {
        }

        protected virtual void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }
    }
}