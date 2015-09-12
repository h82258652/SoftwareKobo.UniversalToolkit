using SoftwareKobo.UniversalToolkit.Controls;
using System;

namespace SoftwareKobo.UniversalToolkit
{
    public sealed class AppStartInfo
    {
        private AppStartInfo()
        {
        }

        public Func<ExtendedSplashScreenContent> ExtendedSplashScreen
        {
            get;
            set;
        }

        public Type MainPage
        {
            get;
            set;
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
                    MainPage = Bootstrapper4.Current?.DefaultMainPage,
                    ExtendedSplashScreen = Bootstrapper4.Current?.DefaultExtendedSplashScreen
                };
            }
        }
    }
}