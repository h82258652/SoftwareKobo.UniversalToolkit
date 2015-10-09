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
using Windows.ApplicationModel.Core;
using Windows.Globalization;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit
{
    public abstract class Bootstrapper : Application
    {
        /// <summary>
        /// 存放所有需要在构造函数结束后执行的方法。
        /// </summary>
        internal List<Func<Task>> _waitForConstructedActions = new List<Func<Task>>();

        private Type _defaultMainPage;

        /// <summary>
        /// 初始化 Bootstrapper 类的新实例。
        /// </summary>
        protected Bootstrapper()
        {
            this.IsInConstructing = true;

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
            this.UnhandledException += this.OnUnhandledException;
        }

        /// <summary>
        /// 获取当前应用程序的 Bootstrapper 对象。
        /// </summary>
        public static new Bootstrapper Current
        {
            get
            {
                return Application.Current as Bootstrapper;
            }
        }

        /// <summary>
        /// 获取当前窗口的 Frame。
        /// </summary>
        public static Frame RootFrame
        {
            get
            {
                return Window.Current.Content as Frame;
            }
        }

        /// <summary>
        /// 指示当前执行是否处于 App 类的构造函数中。
        /// </summary>
        public bool IsInConstructing
        {
            get;
            private set;
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
        /// App 的默认页面。
        /// </summary>
        protected internal Type DefaultMainPage
        {
            get
            {
                return this._defaultMainPage;
            }
            set
            {
                VerifyIsPageType(value);
                this._defaultMainPage = value;
            }
        }

        public Task<Window> ShowNewWindowAsync<TPage>() where TPage : Page
        {
            return ShowNewWindowAsync(typeof(TPage));
        }

        public Task<Window> ShowNewWindowAsync(Type pageType)
        {
            return ShowNewWindowAsync(pageType, null);
        }

        public Task<Window> ShowNewWindowAsync<TPage>(object parameter) where TPage : Page
        {
            return ShowNewWindowAsync(typeof(TPage), parameter);
        }

        public async Task<Window> ShowNewWindowAsync(Type pageType, object parameter)
        {
            if (pageType == null)
            {
                throw new ArgumentNullException(nameof(pageType));
            }

            Window newWindow = await CreateNewWindowAsync();
            int viewId = 0;
            await newWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InitializeRootFrame(newWindow);

                NavigateToFirstPage(newWindow, pageType, parameter);

                newWindow.Activate();
                viewId = ApplicationView.GetForCurrentView().Id;
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId);
            return newWindow;
        }

        protected override sealed async void OnActivated(IActivatedEventArgs args)
        {
            this.IsInConstructing = false;
            await this.InvokeForConstructedActions();

            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);

            #region 所有激活类型

            switch (args.Kind)
            {
                case ActivationKind.Launch:
                case ActivationKind.Search:
                case ActivationKind.ShareTarget:
                case ActivationKind.File:
                    goto default;

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
                    goto default;

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
            this.IsInConstructing = false;
            await this.InvokeForConstructedActions();

            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnCacheFileUpdaterStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnCacheFileUpdaterStartAsync(CachedFileUpdaterActivatedEventArgs args, AppStartInfo info)
        {
            // 缓存文件更新器启动。
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnFileActivated(FileActivatedEventArgs args)
        {
            this.IsInConstructing = false;
            await this.InvokeForConstructedActions();

            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnFileTypeAssociationStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected override sealed async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            this.IsInConstructing = false;
            await this.InvokeForConstructedActions();

            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnFileOpenPickerStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnFileOpenPickerStartAsync(FileOpenPickerActivatedEventArgs args, AppStartInfo info)
        {
            // 文件打开选择器启动。
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args)
        {
            this.IsInConstructing = false;
            await this.InvokeForConstructedActions();

            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnFileSavePickerStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnFileSavePickerStartAsync(FileSavePickerActivatedEventArgs args, AppStartInfo info)
        {
            // 文件保存选择器启动。
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnFileTypeAssociationStartAsync(FileActivatedEventArgs args, AppStartInfo info)
        {
            // 文件关联协议启动。
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnLaunched(LaunchActivatedEventArgs args)
        {
            this.IsInConstructing = false;
            await this.InvokeForConstructedActions();

            AppStartInfo info = AppStartInfo.Default;
            info.Parameter = args.Arguments;
            await this.OnPreStartAsync(args, info);

            #region 确定 Launch 类型

            IList<string> tileIds = PackageManifest.Current.Applications.Select(temp => temp.Id).ToList();// 定义在 AppxManifest 中的所有 Id。
            string launchTileId = args.TileId;// 应用程序启动的磁贴 Id。
            string launchArguments = args.Arguments;// 应用程序启动参数。

            if (tileIds.Contains(launchTileId) && string.IsNullOrEmpty(launchArguments))
            {
                // 主要启动。
                await this.OnPrimaryStartAsync(args, info);
            }
            else if (tileIds.Contains(launchTileId) && string.IsNullOrEmpty(launchArguments) == false)
            {
                // 吐司通知启动。
                await this.OnToastStartAsync(args, info);
            }
            else if (tileIds.Contains(launchTileId) == false)
            {
                // 二级磁贴启动。
                await this.OnSecondaryTileStartAsync(args, info);
            }
            else
            {
                // 其它启动方式。
                await this.OnOtherStartAsync(args, info);
            }

            #endregion 确定 Launch 类型

            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnOtherStartAsync(IActivatedEventArgs args, AppStartInfo info)
        {
            // 其它方式启动。
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnPreStartAsync(IActivatedEventArgs args, AppStartInfo info)
        {
            // 在所有 Start 方法前执行。
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnPrimaryStartAsync(LaunchActivatedEventArgs args, AppStartInfo info)
        {
            // 主要启动，例如直接点击主磁贴启动。
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnProtocolStartAsync(ProtocolActivatedEventArgs protocolArgs, AppStartInfo info)
        {
            // 协议启动。
            return Task.FromResult<object>(null);
        }

        protected virtual void OnResuming(object sender, object e)
        {
        }

        protected override sealed async void OnSearchActivated(SearchActivatedEventArgs args)
        {
            this.IsInConstructing = false;
            await this.InvokeForConstructedActions();

            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnSearchStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnSearchStartAsync(SearchActivatedEventArgs args, AppStartInfo info)
        {
            // 作为搜索建议启动。
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnSecondaryTileStartAsync(LaunchActivatedEventArgs args, AppStartInfo info)
        {
            // 二级磁贴启动。
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            this.IsInConstructing = false;
            await this.InvokeForConstructedActions();

            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnShareTargetStartAsync(args, info);
            this.InternalStartAsync(args, info);
        }

        protected virtual Task OnShareTargetStartAsync(ShareTargetActivatedEventArgs args, AppStartInfo info)
        {
            // 作为分享目标启动。
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnSuspendingAsync(object sender, SuspendingEventArgs e)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnToastStartAsync(LaunchActivatedEventArgs args, AppStartInfo info)
        {
            // 吐司通知启动。
            return Task.FromResult<object>(null);
        }

        protected virtual void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        protected virtual Task OnVoiceCommandStartAsync(VoiceCommandActivatedEventArgs voiceCommandArgs, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        private static void InitializeRootFrame(Window hostWindow)
        {
            if (hostWindow.Content == null)
            {
                Frame frame = new Frame()
                {
                    Language = ApplicationLanguages.Languages[0]
                };
                hostWindow.Content = frame;

                // 是否主窗口。
                //if (CoreApplication.Views[0] == CoreApplication.GetCurrentView())
                //{
                //    string navigationState = ApplicationLocalSettings.Read<string>("MainAppViewNavigationState");
                //    frame.SetNavigationState(navigationState);
                //    frame.Navigated += (sender, e) =>
                //    {
                //        ApplicationLocalSettings.Write<string>("MainAppViewNavigationState", frame.GetNavigationState());
                //    };
                //}
            }
        }

        private static void NavigateToFirstPage(Window hostWindow, Type pageType, object parameter)
        {
            Frame frame = hostWindow.Content as Frame;
            if (frame != null)
            {
                frame.Navigate(pageType, parameter);
            }
        }

        private static async Task ShowExtendedSplashScreenAsync(Window hostWindow, bool isNewWindow, IActivatedEventArgs args, AppStartInfo info)
        {
            // 窗口中是否已经有内容。
            bool hadContent = false;
            await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                hadContent = hostWindow.Content != null;
            });

            if (hadContent)
            {
                return;
            }

            // 扩展启动屏幕结束回调信号。
            TaskCompletionSource<object> extendedSplashScreenTcs = null;

            // 窗口内容。
            UIElement hostWindowContent = null;
            if (info.ExtendedSplashScreen != null)
            {
                ExtendedSplashScreenContent extendedSplashScreenContent = null;
                await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    extendedSplashScreenContent = info.ExtendedSplashScreen.Invoke();
                });
                if (extendedSplashScreenContent != null)
                {
                    // 使用自定义扩展窗口内容。
                    extendedSplashScreenTcs = new TaskCompletionSource<object>();
                    extendedSplashScreenContent.Finished += delegate
                    {
                        extendedSplashScreenTcs.SetResult(null);
                    };

                    // 用于等待 ExtenedSplashScreen 构造完成。
                    TaskCompletionSource<object> containerTcs = new TaskCompletionSource<object>();
                    await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        ExtendedSplashScreen extendedSplashScreen = await ExtendedSplashScreen.CreateAsync(args.SplashScreen, extendedSplashScreenContent);
                        hostWindowContent = extendedSplashScreen;
                        containerTcs.SetResult(null);
                    });
                    await containerTcs.Task;
                }
            }
            if (hostWindowContent == null && isNewWindow)
            {
                // 对于新窗口，必须存在内容。
                await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    hostWindowContent = new UserControl();
                });
            }

            if (hostWindowContent != null)
            {
                await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    hostWindow.Content = hostWindowContent;
                    hostWindow.Activate();
                });
            }

            // 切换到新窗口。
            if (isNewWindow)
            {
                int viewId = 0;
                // 获取新窗口 Id。
                await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    viewId = ApplicationView.GetForCurrentView().Id;
                });
                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId);
            }

            // 使用了扩展启动屏幕。
            if (extendedSplashScreenTcs != null)
            {
                // 等待回调信号。
                await extendedSplashScreenTcs.Task;
            }

            // 清除窗口内容，准备下一步导航。
            await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                hostWindow.Content = null;
            });
        }

        /// <summary>
        /// 验证类型是否继承自 Page。
        /// </summary>
        /// <param name="type">需要验证的类型。</param>
        [Conditional("DEBUG")]
        internal static void VerifyIsPageType(Type type)
        {
            if (type != null && typeof(Page).IsAssignableFrom(type) == false)
            {
                throw new ArgumentException($"parameter {nameof(type)} must sub type of {nameof(Page)}", nameof(type));
            }
        }

        private async Task<Window> CreateNewWindowAsync()
        {
            CoreApplicationView newCoreAppView = CoreApplication.CreateNewView();
            TaskCompletionSource<Window> tcs = new TaskCompletionSource<Window>();
            await newCoreAppView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                tcs.SetResult(Window.Current);
            });
            return await tcs.Task;
        }

        private async void InternalStartAsync(IActivatedEventArgs args, AppStartInfo info)
        {
            Window hostWindow = null;
            bool isNewWindow = false;
            if (info.IsShowInNewWindow == false)
            {
                // 不需要创建新窗口。
                hostWindow = Window.Current;
            }
            else
            {
                if (Window.Current.Content == null && CoreApplication.Views.Count <= 1)
                {
                    // 当前窗口是应用程序初启动窗口。
                    hostWindow = Window.Current;
                }
                else
                {
                    hostWindow = await CreateNewWindowAsync();
                    isNewWindow = true;
                }
            }

            await ShowExtendedSplashScreenAsync(hostWindow, isNewWindow, args, info);

            await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InitializeRootFrame(hostWindow);

                NavigateToFirstPage(hostWindow, info.MainPage, info.Parameter);

                hostWindow.Activate();
            });
        }

        private async Task InvokeForConstructedActions()
        {
            while (this._waitForConstructedActions.Count > 0)
            {
                Func<Task> asyncAction = this._waitForConstructedActions[0];
                await asyncAction.Invoke();
                this._waitForConstructedActions.RemoveAt(0);
            }
        }
    }
}