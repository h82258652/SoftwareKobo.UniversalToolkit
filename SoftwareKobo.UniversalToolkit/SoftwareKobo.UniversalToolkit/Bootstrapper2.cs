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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit
{
    public abstract class Bootstrapper2 : Application
    {
        internal List<Func<Task>> _waitForConstructedActions = new List<Func<Task>>();

        private Func<ExtendedSplashScreenContent> _defaultExtendedSplashScreen;

        private Type _defaultMainPage;

        private bool _isInConstructing;

        /// <summary>
        /// 初始化 Bootstrapper 类的新实例。
        /// </summary>
        protected Bootstrapper2()
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
        public static new Bootstrapper2 Current
        {
            get
            {
                return Application.Current as Bootstrapper2;
            }
        }

        /// <summary>
        /// 指示当前执行是否处于 App 类的构造函数中。
        /// </summary>
        public bool IsInConstructing
        {
            get
            {
                return this._isInConstructing;
            }
            private set
            {
                this._isInConstructing = value;
            }
        }

        /// <summary>
        /// App 的默认扩展启动屏幕。
        /// </summary>
        protected internal Func<ExtendedSplashScreenContent> DefaultExtendedSplashScreen
        {
            get
            {
                return this._defaultExtendedSplashScreen;
            }
            set
            {
                this._defaultExtendedSplashScreen = value;
            }
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
                VerifyDefaultMainPageType(value);
                this._defaultMainPage = value;
            }
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
                    break;

                case ActivationKind.Search:
                    break;

                case ActivationKind.ShareTarget:
                    break;

                case ActivationKind.File:
                    break;

                case ActivationKind.Protocol:
                    break;

                case ActivationKind.FileOpenPicker:
                    break;

                case ActivationKind.FileSavePicker:
                    break;

                case ActivationKind.CachedFileUpdater:
                    break;

                case ActivationKind.ContactPicker:
                    break;

                case ActivationKind.Device:
                    break;

                case ActivationKind.PrintTaskSettings:
                    break;

                case ActivationKind.CameraSettings:
                    break;

                case ActivationKind.RestrictedLaunch:
                    break;

                case ActivationKind.AppointmentsProvider:
                    break;

                case ActivationKind.Contact:
                    break;

                case ActivationKind.LockScreenCall:
                    break;

                case ActivationKind.VoiceCommand:
                    break;

                case ActivationKind.LockScreen:
                    break;

                case ActivationKind.PickerReturned:
                    break;

                case ActivationKind.WalletAction:
                    break;

                case ActivationKind.PickFileContinuation:
                    break;

                case ActivationKind.PickSaveFileContinuation:
                    break;

                case ActivationKind.PickFolderContinuation:
                    break;

                case ActivationKind.WebAuthenticationBrokerContinuation:
                    break;

                case ActivationKind.WebAccountProvider:
                    break;

                case ActivationKind.ComponentUI:
                    break;

                case ActivationKind.ProtocolForResults:
                    break;

                case ActivationKind.ToastNotification:
                    break;

                case ActivationKind.DialReceiver:
                    break;

                default:
                    break;
            }

            #endregion 所有激活类型
        }

        protected override sealed async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args)
        {
            this.IsInConstructing = false;
            await this.InvokeForConstructedActions();

            AppStartInfo info = AppStartInfo.Default;
            await this.OnPreStartAsync(args, info);
            await this.OnCacheFileUpdaterStartAsync(args, info);
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
        }

        protected override sealed async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            this.IsInConstructing = false;
            await this.InvokeForConstructedActions();
        }

        protected virtual Task OnFileOpenPickerStartAsync(FileOpenPickerActivatedEventArgs args, AppStartInfo info)
        {
            return Task.FromResult<object>(null);
        }

        protected override sealed async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args)
        {
            this.IsInConstructing = false;
            await this.InvokeForConstructedActions();
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

        protected override sealed void OnSearchActivated(SearchActivatedEventArgs args)
        {
        }

        protected override sealed void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
        }

        protected virtual Task OnSecondaryTileStartAsync(LaunchActivatedEventArgs args, AppStartInfo info)
        {
            // 二级磁贴启动。
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

        /// <summary>
        /// 验证 App 的默认页面类型是否正确。
        /// </summary>
        /// <param name="defaultMainPageType">App 的默认页面类型。</param>
        [Conditional("DEBUG")]
        private static void VerifyDefaultMainPageType(Type defaultMainPageType)
        {
            if (defaultMainPageType != null && typeof(Page).IsAssignableFrom(defaultMainPageType) == false)
            {
                throw new ArgumentException($"parameter {nameof(DefaultMainPage)} must sub type of {nameof(Page)}", nameof(DefaultMainPage));
            }
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