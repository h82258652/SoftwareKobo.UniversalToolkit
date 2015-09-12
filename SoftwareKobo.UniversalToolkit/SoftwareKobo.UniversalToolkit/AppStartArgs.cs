using SoftwareKobo.UniversalToolkit.Controls;
using System;

namespace SoftwareKobo.UniversalToolkit
{
    public sealed class AppStartArgs
    {
        private AppStartArgs()
        {
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

        public Func<ExtendedSplashScreenContent> SplashScreen
        {
            get;
            set;
        }

        internal static AppStartArgs LoadDefaultSetting()
        {
            return new AppStartArgs()
            {
                MainPage = Bootstrapper3.Current?.DefaultMainPage,
                SplashScreen = Bootstrapper3.Current?.DefaultSplashScreen
            };
        }
    }
}