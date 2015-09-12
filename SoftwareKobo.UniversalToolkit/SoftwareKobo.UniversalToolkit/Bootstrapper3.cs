using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit
{
    public abstract partial class Bootstrapper3 : Application
    {
        protected Bootstrapper3()
        {
            this.Resuming += OnResuming;
            this.Suspending += async (sender, e) =>
            {
                SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
                try
                {
                    await this.OnSuspendingAsync(sender, e);
                }
                finally
                {
                    deferral.Complete();
                }
            };
        }

        public static new Bootstrapper3 Current
        {
            get
            {
                return Application.Current as Bootstrapper3;
            }
        }

        public static Frame RootFrame
        {
            get
            {
                return Window.Current.Content as Frame;
            }
        }

        protected virtual Task OnCachedFileUpdaterStartAsync(CachedFileUpdaterActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnFileOpenPickerStartAsync(FileOpenPickerActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnFileSavePickerStartAsync(FileSavePickerActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnFileTypeAssociationStartAsync(FileActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnOtherStartAsync(IActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnPrimaryStartAsync(LaunchActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnProtocolStartAsync(ProtocolActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual void OnResuming(object sender, object e)
        {
        }

        protected virtual Task OnSearchStartAsync(SearchActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnSecondaryTileStartAsync(LaunchActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnShareTargetStartAsync(ShareTargetActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnSuspendingAsync(object sender, SuspendingEventArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnToastStartAsync(LaunchActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnVoiceCommandStartAsync(VoiceCommandActivatedEventArgs args, AppStartArgs e)
        {
            return Task.FromResult<object>(null);
        }
    }
}