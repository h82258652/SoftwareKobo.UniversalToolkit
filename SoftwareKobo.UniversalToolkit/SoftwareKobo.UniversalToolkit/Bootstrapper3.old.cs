using SoftwareKobo.UniversalToolkit.Controls;
using SoftwareKobo.UniversalToolkit.Utils.AppxManifest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SoftwareKobo.UniversalToolkit
{
    public abstract partial class Bootstrapper3 : Application
    {
        protected override sealed async void OnActivated(IActivatedEventArgs args)
        {
            AppStartArgs e = AppStartArgs.LoadDefaultSetting();

            #region 所有激活类型

            switch (args.Kind)
            {
                case ActivationKind.Launch:
                case ActivationKind.Search:
                case ActivationKind.ShareTarget:
                case ActivationKind.File:
                    await this.OnOtherStartAsync(args, e);
                    break;

                case ActivationKind.Protocol:
                    ProtocolActivatedEventArgs protocolArgs = (ProtocolActivatedEventArgs)args;
                    await this.OnProtocolStartAsync(protocolArgs, e);
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
                    break;

                case ActivationKind.VoiceCommand:
                    VoiceCommandActivatedEventArgs voiceCommandArgs = (VoiceCommandActivatedEventArgs)args;
                    await this.OnVoiceCommandStartAsync(voiceCommandArgs, e);
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
                    await this.OnOtherStartAsync(args, e);
                    break;
            }

            #endregion 所有激活类型

            this.InternalStartAsync(args, e);
        }

        protected override sealed async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args)
        {
            AppStartArgs e = AppStartArgs.LoadDefaultSetting();
            await this.OnCachedFileUpdaterStartAsync(args, e);
            this.InternalStartAsync(args, e);
        }

        protected override sealed async void OnFileActivated(FileActivatedEventArgs args)
        {
            AppStartArgs e = AppStartArgs.LoadDefaultSetting();
            await this.OnFileTypeAssociationStartAsync(args, e);
            this.InternalStartAsync(args, e);
        }

        protected override sealed async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            AppStartArgs e = AppStartArgs.LoadDefaultSetting();
            await this.OnFileOpenPickerStartAsync(args, e);
            this.InternalStartAsync(args, e);
        }

        protected override sealed async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args)
        {
            AppStartArgs e = AppStartArgs.LoadDefaultSetting();
            await this.OnFileSavePickerStartAsync(args, e);
            this.InternalStartAsync(args, e);
        }

        protected override sealed async void OnLaunched(LaunchActivatedEventArgs args)
        {
            AppStartArgs e = AppStartArgs.LoadDefaultSetting();
            e.Parameter = args.Arguments;

            #region 确定 Launch 类型

            IList<string> tileIds = PackageManifest.Current.Applications.Select(temp => temp.Id).ToList();
            string launchedTileId = args.TileId;
            string launchedArguments = args.Arguments;
            if (tileIds.Contains(launchedTileId) && string.IsNullOrEmpty(launchedArguments))
            {
                // Primary Launch.
                await this.OnPrimaryStartAsync(args, e);
            }
            else if (tileIds.Contains(launchedTileId) && string.IsNullOrEmpty(launchedArguments) == false)
            {
                // Toast Launch.
                await this.OnToastStartAsync(args, e);
            }
            else if (tileIds.Contains(launchedTileId) == false)
            {
                // Secondary Tile Launch.
                await this.OnSecondaryTileStartAsync(args, e);
            }
            else
            {
                // Other Launch.
                await this.OnOtherStartAsync(args, e);
            }

            #endregion 确定 Launch 类型

            this.InternalStartAsync(args, e);
        }

        protected override sealed async void OnSearchActivated(SearchActivatedEventArgs args)
        {
            AppStartArgs e = AppStartArgs.LoadDefaultSetting();
            await this.OnSearchStartAsync(args, e);
            this.InternalStartAsync(args, e);
        }

        protected override sealed async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            AppStartArgs e = AppStartArgs.LoadDefaultSetting();
            await this.OnShareTargetStartAsync(args, e);
            this.InternalStartAsync(args, e);
        }

        // --------------------------------------------------------------
        // 分隔线
        // --------------------------------------------------------------

        private void InitRootFrame()
        {
            if (RootFrame == null)
            {
                Window.Current.Content = new Frame()
                {
                    Language = ApplicationLanguages.Languages[0]
                };
                /*
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }
                */
                RootFrame.NavigationFailed += this.RootFrameNavigationFailed;
            }
        }

        protected virtual void RootFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
        }

        private async void InternalStartAsync(IActivatedEventArgs args, AppStartArgs e)
        {
            await this.ShowExtendedSplashScreenAsync(args, e);

                 this.InitRootFrame();
                 
                 this.NavigateToFirstPage(args, e);

                 Window.Current.Activate();
        }

        private void NavigateToFirstPage(IActivatedEventArgs args, AppStartArgs e)
        {
            RootFrame.Navigate(e.MainPage, e.Parameter);
        }

        private async Task ShowExtendedSplashScreenAsync(IActivatedEventArgs args, AppStartArgs e)
        {
            TaskCompletionSource<object> splashScreenTcs = null;
            
            // 已在运行则不显示启动屏幕。
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                ExtendedSplashScreenContent splashScreenContent = null;

                #region 设置启动屏幕内容。

                if (e.SplashScreen != null)
                {
                    splashScreenContent = e.SplashScreen();
                }

                #endregion

                if (splashScreenContent != null)
                {
                    #region 启动屏幕结束，设置信号。
                    
                     splashScreenTcs = new TaskCompletionSource<object>();
                    splashScreenContent.Finished += (sender, _e) =>
                    {
                                splashScreenTcs.SetResult(null);
                    };

                    #endregion 启动屏幕结束，设置信号。

                    ExtendedSplashScreen splashScreen = await ExtendedSplashScreen.CreateAsync(args.SplashScreen, splashScreenContent);
                    Window.Current.Content = splashScreen;
                    Window.Current.Activate();
                }
            }

            // 使用了启动屏幕，等待启动屏幕结束。
            if (splashScreenTcs != null)
            {
                await splashScreenTcs.Task;
            }
        }

        private Type _defaultMainPage;

        /// <summary>
        /// App 的默认主页。
        /// </summary>
        protected internal Type DefaultMainPage
        {
            get
            {
                return _defaultMainPage;
            }
            set
            {
                VerifyDefaultMainPageType(value);
                _defaultMainPage = value;
            }
        }

        [Conditional("DEBUG")]
        private static void VerifyDefaultMainPageType(Type defaultMainPageType)
        {
            if (typeof(Page).IsAssignableFrom(defaultMainPageType) == false)
            {
                throw new ArgumentException($"parameter {nameof(DefaultMainPage)} must sub type of {nameof(Page)}", nameof(DefaultMainPage));
            }
        }

        /// <summary>
        /// App 的默认扩展启动屏幕。
        /// </summary>
        protected internal Func<ExtendedSplashScreenContent> DefaultSplashScreen
        {
            get;
            set;
        }
    }
}