﻿using SoftwareKobo.UniversalToolkit.Controls;
using SoftwareKobo.UniversalToolkit.Extensions;
using SoftwareKobo.UniversalToolkit.Utils.AppxManifest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        /// 存放所有需要 RootFrame 创造后执行的方法。
        /// </summary>
        internal static List<Func<Task>> _waitForRootFrameCreatedActions = new List<Func<Task>>();

        /// <summary>
        /// 存放所有需要在构造函数结束后执行的方法。
        /// </summary>
        internal List<Func<Task>> _waitForConstructedActions = new List<Func<Task>>();

        private Type _defaultNavigatePage;

        /// <summary>
        /// 初始化 Bootstrapper 类的新实例。
        /// </summary>
        protected Bootstrapper()
        {
            IsInConstructing = true;

            Resuming += OnResuming;
            Suspending += async (sender, e) =>
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                try
                {
                    await OnSuspendingAsync(sender, e);
                }
                finally
                {
                    deferral.Complete();
                }
            };
            UnhandledException += OnUnhandledException;
        }

        [SuppressMessage("Microsoft.Design", "CA1009")]
        public event EventHandler<WindowCreatedEventArgs> WindowCreated;

        /// <summary>
        /// 获取当前应用程序的 Bootstrapper 对象。
        /// </summary>
        public static new Bootstrapper Current => Application.Current as Bootstrapper;

        /// <summary>
        /// 获取当前窗口的根导航框架。
        /// </summary>
        public static Frame RootFrame => Window.Current.Content as Frame;

        /// <summary>
        /// 指示当前执行是否处于 App 类的构造函数中。
        /// </summary>
        public bool IsInConstructing
        {
            get;
            private set;
        }

        /// <summary>
        /// 设置或获取 App 的默认扩展启动屏幕。
        /// </summary>
        protected internal Func<ExtendedSplashScreenContent> DefaultExtendedSplashScreen
        {
            get;
            set;
        }

        /// <summary>
        /// 设置或获取 App 默认的导航页面。
        /// </summary>
        protected internal Type DefaultNavigatePage
        {
            get
            {
                return _defaultNavigatePage;
            }
            set
            {
                VerifyIsPageType(value);
                _defaultNavigatePage = value;
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

            var newWindow = await CreateNewWindowAsync();

            await InitializeRootFrameAsync(newWindow);

            await NavigateToFirstPageAsync(newWindow, pageType, parameter);

            await newWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                newWindow.Activate();
            });

            await SwitchToWindowAsync(newWindow);

            return newWindow;
        }

        /// <summary>
        /// 验证类型是否继承自 Page。
        /// </summary>
        /// <param name="type">需要验证的类型。</param>
        /// <exception cref="ArgumentException">验证的类型不为空，且不继承自 Page 类。</exception>
        [Conditional("DEBUG")]
        internal static void VerifyIsPageType(Type type)
        {
            if (type != null && typeof(Page).IsAssignableFrom(type) == false)
            {
                throw new ArgumentException($"parameter {nameof(type)} must be sub type of {nameof(Page)}", nameof(type));
            }
        }

        protected override sealed async void OnActivated(IActivatedEventArgs args)
        {
            await HandleWaitForConstructedActionsAsync();

            var info = AppStartInfo.Default;
            await OnPreStartAsync(args, info);

            #region 所有激活类型

            switch (args.Kind)
            {
                case ActivationKind.Launch:
                case ActivationKind.Search:
                case ActivationKind.ShareTarget:
                case ActivationKind.File:
                    goto default;

                case ActivationKind.Protocol:
                    var protocolArgs = (ProtocolActivatedEventArgs)args;
                    await OnProtocolStartAsync(protocolArgs, info);
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
                    var voiceCommandArgs = (VoiceCommandActivatedEventArgs)args;
                    await OnVoiceCommandStartAsync(voiceCommandArgs, info);
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
                    await OnOtherStartAsync(args, info);
                    break;
            }

            #endregion 所有激活类型

            InternalStartAsync(args, info);
        }

        protected override sealed async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args)
        {
            await HandleWaitForConstructedActionsAsync();

            var info = AppStartInfo.Default;
            await OnPreStartAsync(args, info);
            await OnCacheFileUpdaterStartAsync(args, info);
            InternalStartAsync(args, info);
        }

        /// <summary>
        /// 应用程序作为缓存文件更新器时激活使用此方法作为入口。
        /// </summary>
        /// <param name="args">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnCacheFileUpdaterStartAsync(CachedFileUpdaterActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnFileActivated(FileActivatedEventArgs args)
        {
            await HandleWaitForConstructedActionsAsync();

            var info = AppStartInfo.Default;
            await OnPreStartAsync(args, info);
            await OnFileTypeAssociationStartAsync(args, info);
            InternalStartAsync(args, info);
        }

        protected override sealed async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            await HandleWaitForConstructedActionsAsync();

            var info = AppStartInfo.Default;
            await OnPreStartAsync(args, info);
            await OnFileOpenPickerStartAsync(args, info);
            InternalStartAsync(args, info);
        }

        /// <summary>
        /// 通过打开文件对话框激活应用程序时使用此方法作为入口。
        /// </summary>
        /// <param name="args">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnFileOpenPickerStartAsync(FileOpenPickerActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args)
        {
            await HandleWaitForConstructedActionsAsync();

            var info = AppStartInfo.Default;
            await OnPreStartAsync(args, info);
            await OnFileSavePickerStartAsync(args, info);
            InternalStartAsync(args, info);
        }

        /// <summary>
        /// 通过保存文件对话框激活应用程序时使用此方法作为入口。
        /// </summary>
        /// <param name="args">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnFileSavePickerStartAsync(FileSavePickerActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 应用程序关联文件协议，打开文件激活时使用此方法作为入口。
        /// </summary>
        /// <param name="args">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnFileTypeAssociationStartAsync(FileActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnLaunched(LaunchActivatedEventArgs args)
        {
            await HandleWaitForConstructedActionsAsync();

            var info = AppStartInfo.Default;
            info.Parameter = args.Arguments;
            await OnPreStartAsync(args, info);

            #region 确定 Launch 类型。

            // 获取定义在 AppxManifest 中的所有 Id。
            IList<string> tileIds = PackageManifest.Current.Applications.Select(temp => temp.Id).ToList();
            // 应用程序启动的磁贴 Id。
            var launchTileId = args.TileId;
            // 应用程序启动参数。
            var launchArguments = args.Arguments;

            if (tileIds.Contains(launchTileId) && string.IsNullOrEmpty(launchArguments))
            {
                // 主要启动。
                await OnPrimaryStartAsync(args, info);
            }
            else if (tileIds.Contains(launchTileId) && string.IsNullOrEmpty(launchArguments) == false)
            {
                // 吐司通知启动。
                await OnToastStartAsync(args, info);
            }
            else if (tileIds.Contains(launchTileId) == false)
            {
                // 二级磁贴启动。
                await OnSecondaryTileStartAsync(args, info);
            }
            else
            {
                // 其它启动方式。
                await OnOtherStartAsync(args, info);
            }

            #endregion 确定 Launch 类型。

            InternalStartAsync(args, info);
        }

        /// <summary>
        /// 通过其它未列出的方式启动、激活时，使用此方法作为入口。
        /// </summary>
        /// <param name="args">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnOtherStartAsync(IActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 应用程序预启动。该方法会在启动其它 Start 方法前执行。
        /// </summary>
        /// <param name="args">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnPreStartAsync(IActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 应用程序通过主要启动（例如点击主磁贴）时，使用此方法作为入口。
        /// </summary>
        /// <param name="args">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnPrimaryStartAsync(LaunchActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 应用程序通过关联协议激活时使用此方法作为入口。
        /// </summary>
        /// <param name="protocolArgs">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnProtocolStartAsync(ProtocolActivatedEventArgs protocolArgs, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual void OnResuming(object sender, object e)
        {
        }

        protected override sealed async void OnSearchActivated(SearchActivatedEventArgs args)
        {
            await HandleWaitForConstructedActionsAsync();

            var info = AppStartInfo.Default;
            await OnPreStartAsync(args, info);
            await OnSearchStartAsync(args, info);
            InternalStartAsync(args, info);
        }

        /// <summary>
        /// 通过搜索关联激活该应用程序是使用此方法作为入口。
        /// </summary>
        /// <param name="args">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnSearchStartAsync(SearchActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 应用程序通过二级磁贴启动时使用此方法作为入口。
        /// </summary>
        /// <param name="args">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnSecondaryTileStartAsync(LaunchActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            await HandleWaitForConstructedActionsAsync();

            var info = AppStartInfo.Default;
            await OnPreStartAsync(args, info);
            await OnShareTargetStartAsync(args, info);
            InternalStartAsync(args, info);
        }

        /// <summary>
        /// 作为系统共享目标时使用此方法作为入口。
        /// </summary>
        /// <param name="args">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnShareTargetStartAsync(ShareTargetActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task OnSuspendingAsync(object sender, SuspendingEventArgs e)
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 应用程序通过吐司通知启动时使用此方法作为入口。
        /// </summary>
        /// <param name="args">事件的事件数据。</param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task OnToastStartAsync(LaunchActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        protected virtual Task OnVoiceCommandStartAsync(VoiceCommandActivatedEventArgs voiceCommandArgs, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnWindowCreated(WindowCreatedEventArgs args)
        {
            if (WindowCreated != null)
            {
                WindowCreated(this, args);
            }
            await OnWindowCreatedAsync(args);
        }

        protected virtual Task OnWindowCreatedAsync(WindowCreatedEventArgs args)
        {
            return Task.FromResult<object>(null);
        }

        private static async Task<UIElement> BuildExtendedSplashScreenAsync(Window hostWindow, ExtendedSplashScreenContent extendedSplashScreenContent, IActivatedEventArgs args)
        {
            // 用于等待 ExtendedSplashScreen 构造完成。因为 RunAsync 方法第二个参数签名是 async void，不是 async Task。
            var containerTcs = new TaskCompletionSource<UIElement>();
            await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var extendedSplashScreen = await ExtendedSplashScreen.CreateAsync(args.SplashScreen, extendedSplashScreenContent);
                containerTcs.SetResult(extendedSplashScreen);
            });
            return await containerTcs.Task;
        }

        private static async Task<Tuple<ExtendedSplashScreenContent, TaskCompletionSource<object>>> CreateExtendedSplashScreenContentAsync(Window hostWindow, AppStartInfo info)
        {
            // 创建创建扩展启动屏幕方法为 null。
            if (info.ExtendedSplashScreen == null)
            {
                return Tuple.Create<ExtendedSplashScreenContent, TaskCompletionSource<object>>(null, null);
            }

            ExtendedSplashScreenContent extendedSplashScreenContent = null;
            TaskCompletionSource<object> extendedSplashScreenTcs = null;
            await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                extendedSplashScreenContent = info.ExtendedSplashScreen();
                if (extendedSplashScreenContent != null)
                {
                    extendedSplashScreenTcs = new TaskCompletionSource<object>();

                    EventHandler extendedSplashScreenFinishedHandler = null;
                    extendedSplashScreenFinishedHandler = delegate
                    {
                        extendedSplashScreenContent.Finished -= extendedSplashScreenFinishedHandler;
                        extendedSplashScreenTcs.SetResult(null);
                    };
                    extendedSplashScreenContent.Finished += extendedSplashScreenFinishedHandler;
                }
            });
            return Tuple.Create<ExtendedSplashScreenContent, TaskCompletionSource<object>>(extendedSplashScreenContent, extendedSplashScreenTcs);
        }

        private static async Task<bool> GetIsWindowExistContentAsync(Window hostWindow)
        {
            var hadContent = false;
            await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                hadContent = hostWindow.Content != null;
            });
            return hadContent;
        }

        private static async Task InitializeRootFrameAsync(Window hostWindow)
        {
            await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (hostWindow.Content == null)
                {
                    var frame = new Frame()
                    {
                        Language = ApplicationLanguages.Languages[0]
                    };
                    SetContent(hostWindow, frame);
                }
            });
        }

        private static void SetContent(Window hostWindow, UIElement content)
        {
            hostWindow.Content = content;
            ColorExtensions.ReInitAccentColorChanged();
        }

        private static async Task NavigateToFirstPageAsync(Window hostWindow, Type pageType, object parameter)
        {
            if (pageType == null)
            {
                // 用户设置了该启动、激活方式不导航。
                return;
            }

            await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var rootFrame = hostWindow.Content as Frame;
                rootFrame?.Navigate(pageType, parameter);
            });
        }

        private static async Task ShowExtendedSplashScreenAsync(Window hostWindow, bool isNewWindow, IActivatedEventArgs args, AppStartInfo info)
        {
            // 窗口中是否已经有内容。
            if (await GetIsWindowExistContentAsync(hostWindow))
            {
                return;
            }

            var temp = await CreateExtendedSplashScreenContentAsync(hostWindow, info);

            // 扩展屏幕内容。
            var extendedSplashScreenContent = temp.Item1;

            // 扩展启动屏幕结束回调信号。
            var extendedSplashScreenTcs = temp.Item2;

            // 窗口内容。
            UIElement hostWindowContent = null;

            if (extendedSplashScreenContent != null)
            {
                // 如果扩展屏幕窗口内容不为 null，则创建扩展启动屏幕容器并赋值到 hostWindowContent 变量。
                hostWindowContent = await BuildExtendedSplashScreenAsync(hostWindow, extendedSplashScreenContent, args);
            }

            if (hostWindowContent == null && isNewWindow)
            {
                // 对于新窗口，必须确保存在内容。
                await hostWindowContent.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    hostWindowContent = new ContentControl();
                });
            }

            await ShowWindowContentAsync(hostWindow, hostWindowContent);

            if (isNewWindow)
            {
                // 切换到新窗口。
                await SwitchToWindowAsync(hostWindow);
            }

            // 使用了扩展启动屏幕。
            if (extendedSplashScreenTcs != null)
            {
                // 等待回调信号。
                await extendedSplashScreenTcs.Task;
            }

            // 清除窗口内容，准备导航操作。
            await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SetContent(hostWindow, null);
            });
        }

        private static async Task ShowWindowContentAsync(Window hostWindow, UIElement hostWindowContent)
        {
            if (hostWindowContent != null)
            {
                await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    SetContent(hostWindow, hostWindowContent);
                    hostWindow.Activate();
                });
            }
        }

        private static async Task SwitchToWindowAsync(Window hostWindow)
        {
            var viewId = 0;
            // 获取新窗口 Id。
            await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                viewId = ApplicationView.GetForCurrentView().Id;
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId);
        }

        private async Task<Window> CreateNewWindowAsync()
        {
            var newCoreAppView = CoreApplication.CreateNewView();
            Window newWindow = null;
            await newCoreAppView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                newWindow = Window.Current;
            });
            return newWindow;
        }

        private async Task HandleWaitForConstructedActionsAsync()
        {
            IsInConstructing = false;
            while (_waitForConstructedActions.Count > 0)
            {
                var asyncAction = _waitForConstructedActions[0];
                await asyncAction();
                _waitForConstructedActions.RemoveAt(0);
            }
        }

        private async void InternalStartAsync(IActivatedEventArgs args, AppStartInfo info)
        {
            Window hostWindow = null;
            // 当前创建的窗口是否是新窗口。
            var isNewWindow = false;

            // 当前窗口是否是应用程序初启动窗口。
            var isMainWindow = args.PreviousExecutionState == ApplicationExecutionState.NotRunning || args.PreviousExecutionState == ApplicationExecutionState.Terminated || args.PreviousExecutionState == ApplicationExecutionState.ClosedByUser;

            if (info.IsShowInNewWindow == false)
            {
                // 不需要创建新窗口。
                hostWindow = Window.Current;
            }
            else
            {
                if (isMainWindow)
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

            await InitializeRootFrameAsync(hostWindow);

            await NavigateToFirstPageAsync(hostWindow, info.NavigatePage, info.Parameter);

            await hostWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // 确保激活窗口。
                hostWindow.Activate();
            });
        }
    }
}