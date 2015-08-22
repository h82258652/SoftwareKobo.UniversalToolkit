using SoftwareKobo.UniversalToolkit.Controls;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SoftwareKobo.UniversalToolkit
{
    public abstract class Bootstrapper : Application
    {
        private bool _isEnabledDebugSettings;
        private Type _mainPageType;
        private Type _splashScreenType;

        public Bootstrapper(Type mainPageType) : this(mainPageType, null)
        {
        }

        public Bootstrapper(Type mainPageType, Type splashScreenType)
        {
            this.VerifyConstructorParameters(mainPageType, splashScreenType);

            this._mainPageType = mainPageType;
            this._splashScreenType = splashScreenType;

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
            this.UnhandledException += OnUnhandledException;
        }

        public Frame RootFrame
        {
            get;
            protected set;
        }

        protected bool IsEnabledDebugSettings
        {
            get
            {
                return this._isEnabledDebugSettings;
            }
            set
            {
                this._isEnabledDebugSettings = value;
                this.EnabledDebugSettings(value);
            }
        }

        [Conditional("DEBUG")]
        protected virtual void EnabledDebugSettings(bool isEnabled)
        {
            if (Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = isEnabled;
            }
        }

        protected virtual Task InitAppParametersAsync()
        {
            return Task.FromResult<object>(null);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            this.IsEnabledDebugSettings = true;

            this.InitRootFrame();

            await this.InitAppParametersAsync();

            if (this._splashScreenType != null)
            {
                if (args.PreviousExecutionState != ApplicationExecutionState.Running)
                {
                    ExtendedSplashScreenContent extendedSplashScreenContent = Activator.CreateInstance(this._splashScreenType) as ExtendedSplashScreenContent;
                    extendedSplashScreenContent.Finished += (sender, e) =>
                    {
                        Window.Current.Content = this.RootFrame;
                        this.RootFrame.Navigate(this._mainPageType, args.Arguments);
                    };

                    ExtendedSplashScreen extendedSplashScreen = await ExtendedSplashScreen.CreateAsync(args.SplashScreen, extendedSplashScreenContent);
                    Window.Current.Content = extendedSplashScreen;
                }
            }
            else
            {
                Window.Current.Content = this.RootFrame;
                this.RootFrame.Navigate(this._mainPageType, args.Arguments);
            }

            Window.Current.Activate();
        }

        /// <summary>
        /// 在应用程序从挂起状态转换为运行状态时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnResuming(object sender, object e)
        {
        }

        protected virtual Task OnSuspendingAsync(object sender, SuspendingEventArgs e)
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 当异常可由应用程序代码处理，如从本机级别的 Windows 运行时错误转发时发生。应用程序可标记事件数据中处理的匹配项。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.Fail(e.Message, e.Exception.StackTrace);
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        protected virtual void RootFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.Fail("Failed to load Page" + e.SourcePageType.FullName, e.Exception.StackTrace);
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        private void InitRootFrame()
        {
            RootFrame = Window.Current.Content as Frame;
            if (RootFrame == null)
            {
                RootFrame = new Frame()
                {
                    Language = ApplicationLanguages.Languages[0]
                };
                RootFrame.NavigationFailed += RootFrameNavigationFailed;
            }
        }

        [Conditional("DEBUG")]
        private void VerifyConstructorParameters(Type mainPageType, Type splashScreenType)
        {
            if (typeof(Page).IsAssignableFrom(mainPageType) == false)
            {
                throw new ArgumentException($"parameter {nameof(mainPageType)} is not sub type of {nameof(Page)}", nameof(mainPageType));
            }
            if (splashScreenType != null && typeof(ExtendedSplashScreenContent).IsAssignableFrom(splashScreenType) == false)
            {
                throw new ArgumentException($"parameter {nameof(splashScreenType)} is not sub type of {nameof(ExtendedSplashScreenContent)}", nameof(splashScreenType));
            }
        }
    }
}