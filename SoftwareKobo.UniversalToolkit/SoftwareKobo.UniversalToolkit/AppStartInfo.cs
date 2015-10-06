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

        public Func<ExtendedSplashScreenContent> ExtendedSplashScreen
        {
            get;
            set;
        }

        public bool IsShowInNewWindow
        {
            get;
            set;
        }

        public Type MainPage
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
                    MainPage = Bootstrapper.Current?.DefaultMainPage,
                    ExtendedSplashScreen = Bootstrapper.Current?.DefaultExtendedSplashScreen
                };
            }
        }
    }
}