using SoftwareKobo.UniversalToolkit.Controls;
using System;

namespace SoftwareKobo.UniversalToolkit
{
    public sealed class AppStartInfo
    {
        private Type _mainPage;

        private AppStartInfo()
        {
        }

        /// <summary>
        /// 该启动方式使用的扩展启动屏幕。
        /// </summary>
        /// <remarks>若设置为 null 或者方法返回 null，则不使用扩展启动屏幕。</remarks>
        public Func<ExtendedSplashScreenContent> ExtendedSplashScreen
        {
            get;
            set;
        }

        /// <summary>
        /// 是否使用一个新窗口展示内容（默认为 false）。
        /// </summary>
        public bool IsShowInNewWindow
        {
            get;
            set;
        }

        /// <summary>
        /// 该启动方式导航到哪个页面。
        /// </summary>
        /// <remarks>若设置为 null，则不对当前窗口进行导航。</remarks>
        public Type NavigatePage
        {
            get
            {
                return this._mainPage;
            }
            set
            {
                Bootstrapper.VerifyIsPageType(value);
                this._mainPage = value;
            }
        }

        /// <summary>
        /// 导航参数。
        /// </summary>
        public object Parameter
        {
            get;
            set;
        }

        internal static AppStartInfo Default
        {
            get
            {
                return new AppStartInfo()
                {
                    NavigatePage = Bootstrapper.Current?.DefaultNavigatePage,
                    ExtendedSplashScreen = Bootstrapper.Current?.DefaultExtendedSplashScreen
                };
            }
        }
    }
}