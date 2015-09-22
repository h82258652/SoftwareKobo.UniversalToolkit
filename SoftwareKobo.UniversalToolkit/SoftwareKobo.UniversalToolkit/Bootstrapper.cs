using SoftwareKobo.UniversalToolkit.Controls;
using SoftwareKobo.UniversalToolkit.Utils.AppxManifest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SoftwareKobo.UniversalToolkit
{
    public abstract class Bootstrapper : Application
    {
        private Type _defaultMainPage;

        protected Bootstrapper()
        {
            this.Resuming += this.OnResuming;
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

        public static new Bootstrapper Current
        {
            get
            {
                return Application.Current as Bootstrapper;
            }
        }

        public static Frame RootFrame
        {
            get
            {
                return Window.Current.Content as Frame;
            }
        }

        /// <summary>
        /// App 的默认扩展启动屏幕。
        /// </summary>
        protected internal Func<ExtendedSplashScreenContent> DefaultExtendedSplashScreen
        {
            get;
            set;
        }

        /// <summary>
        /// App 的默认主页。
        /// </summary>
        protected internal Type DefaultMainPage
        {
            get
            {
                return this._defaultMainPage;
            }
            set
            {
                VerifyDefaultMainPageType(value);
                this._defaultMainPage = value;
            }
        }

        protected override sealed async void OnActivated(IActivatedEventArgs args)
        {
            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);

            #region 所有激活类型

            switch (args.Kind)
            {
                case ActivationKind.Launch:
                case ActivationKind.Search:
                case ActivationKind.ShareTarget:
                case ActivationKind.File:
                    await this.OnOtherStartAsync(args, info);
                    break;

                case ActivationKind.Protocol:
                    ProtocolActivatedEventArgs protocolArgs = (ProtocolActivatedEventArgs)args;
                    await this.OnProtocolStartAsync(protocolArgs, info);
                    break;

                case ActivationKind.FileOpenPicker:
                case ActivationKind.FileSavePicker:
                case ActivationKind.CachedFileUpdater:
                case ActivationKind.ContactPicker:
                case ActivationKind.Device:
                case ActivationKind.PrintTaskSettings:
                case ActivationKind.CameraSettings:
                case ActivationKind.RestrictedLaunch:
                case ActivationKind.AppointmentsProvider:
                case ActivationKind.Contact:
                case ActivationKind.LockScreenCall:
                    await this.OnOtherStartAsync(args, info);
                    break;

                case ActivationKind.VoiceCommand:
                    VoiceCommandActivatedEventArgs voiceCommandArgs = (VoiceCommandActivatedEventArgs)args;
                    await this.OnVoiceCommandStartAsync(voiceCommandArgs, info);
                    break;

                case ActivationKind.LockScreen:
                case ActivationKind.PickerReturned:
                case ActivationKind.WalletAction:
                case ActivationKind.PickFileContinuation:
                case ActivationKind.PickSaveFileContinuation:
                case ActivationKind.PickFolderContinuation:
                case ActivationKind.WebAuthenticationBrokerContinuation:
                case ActivationKind.WebAccountProvider:
                case ActivationKind.ComponentUI:
                case ActivationKind.ProtocolForResults:
                case ActivationKind.ToastNotification:
                case ActivationKind.DialReceiver:
                default:
                    await this.OnOtherStartAsync(args, info);
                    break;
            }

            #endregion 所有激活类型

            this.InternalStartAsync(args, info);
        }

        protected override sealed async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args)
        {
            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnCachedFileUpdaterStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnCachedFileUpdaterStartAsync(CachedFileUpdaterActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnFileActivated(FileActivatedEventArgs args)
        {
            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnFileTypeAssociationStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected override sealed async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnFileOpenPickerStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnFileOpenPickerStartAsync(FileOpenPickerActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args)
        {
            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnFileSavePickerStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnFileSavePickerStartAsync(FileSavePickerActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnFileTypeAssociationStartAsync(FileActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnLaunched(LaunchActivatedEventArgs args)
        {
            AppStartInfo info = AppStartInfo.Default;
            info.Parameter = args.Arguments;
            await this.OnPreStartAsync(args, info);

            #region 确定 Launch 类型

            IList<string> tileIds = PackageManifest.Current.Applications.Select(temp => temp.Id).ToList();
            string launchedTileId = args.TileId;
            string launchedArguments = args.Arguments;

            if (tileIds.Contains(launchedTileId) && string.IsNullOrEmpty(launchedArguments))
            {
                // Primary Launch.
                await this.OnPrimaryStartAsync(args, info);
            }
            else if (tileIds.Contains(launchedTileId) && string.IsNullOrEmpty(launchedArguments) == false)
            {
                // Toast Launch.
                await this.OnToastStartAsync(args, info);
            }
            else if (tileIds.Contains(launchedTileId) == false)
            {
                // Secondary Tile Launch.
                await this.OnSecondaryTileStartAsync(args, info);
            }
            else
            {
                // Other Launch.
                await this.OnOtherStartAsync(args, info);
            }

            #endregion 确定 Launch 类型

            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnOtherStartAsync(IActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnPreStartAsync(IActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnPrimaryStartAsync(LaunchActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnProtocolStartAsync(ProtocolActivatedEventArgs protocolArgs, object e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual void OnResuming(object sender, object e)
        {
        }

        protected override sealed async void OnSearchActivated(SearchActivatedEventArgs args)
        {
            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnSearchStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnSearchStartAsync(SearchActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnSecondaryTileStartAsync(LaunchActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnShareTargetStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnShareTargetStartAsync(ShareTargetActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnSuspendingAsync(object sender, SuspendingEventArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnToastStartAsync(LaunchActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnVoiceCommandStartAsync(VoiceCommandActivatedEventArgs voiceCommandArgs, object e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual void RootFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
        }

        [Conditional("DEBUG")]
        private static void VerifyDefaultMainPageType(Type defaultMainPageType)
        {
            if (defaultMainPageType != null && typeof(Page).IsAssignableFrom(defaultMainPageType) == false)
            {
                throw new ArgumentException($"parameter {nameof(DefaultMainPage)} must sub type of {nameof(Page)}", nameof(DefaultMainPage));
            }
        }

        private void InitializeRootFrame(IActivatedEventArgs args)
        {
            if (RootFrame == null)
            {
                Window.Current.Content = new Frame()
                {
                    Language = ApplicationLanguages.Languages[0]
                };
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    string navigationState = (string)ApplicationData.Current.LocalSettings.Values["RootFrameNavigationState"];
                    RootFrame.SetNavigationState(navigationState);
                }

                /*
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }
                */
                RootFrame.Navigated += (sender, e) =>
                {
                    string navigationState = RootFrame.GetNavigationState();
                    ApplicationData.Current.LocalSettings.Values["RootFrameNavigationState"] = navigationState;
                };
                RootFrame.NavigationFailed += RootFrameNavigationFailed;
            }
        }

        private async void InternalStartAsync(IActivatedEventArgs args, AppStartInfo info)
        {
            await this.ShowExtendedSplashScreenAsync(args, info);

            this.InitializeRootFrame(args);

            this.NavigateToFirstPage(args, info);

            Window.Current.Activate();
        }

        private void NavigateToFirstPage(IActivatedEventArgs args, AppStartInfo info)
        {
            if (RootFrame.Content == null)
            {
                RootFrame.Navigate(info.MainPage, info.Parameter);
            }
        }

        private async Task ShowExtendedSplashScreenAsync(IActivatedEventArgs args, AppStartInfo info)
        {
            TaskCompletionSource<object> extendedSplashScreenTcs = null;

            // App 已运行则不显示扩展启动屏幕。
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                ExtendedSplashScreenContent extendedSplashScreenContent = info?.ExtendedSplashScreen();

                if (extendedSplashScreenContent != null)
                {
                    #region 扩展启动屏幕结束，设置回调信号。

                    extendedSplashScreenTcs = new TaskCompletionSource<object>();
                    extendedSplashScreenContent.Finished += (sender, e) =>
                    {
                        extendedSplashScreenTcs.SetResult(null);
                    };

                    #endregion 扩展启动屏幕结束，设置回调信号。

                    ExtendedSplashScreen extendedSplashScreen = await ExtendedSplashScreen.CreateAsync(args.SplashScreen, extendedSplashScreenContent);
                    Window.Current.Content = extendedSplashScreen;
                    Window.Current.Activate();

                    // 等待回调信号。
                    await extendedSplashScreenTcs.Task;
                }
            }
        }
    }
}