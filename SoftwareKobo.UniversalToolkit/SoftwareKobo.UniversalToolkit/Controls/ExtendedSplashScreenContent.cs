using System;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public abstract class ExtendedSplashScreenContent : UserControl
    {
        internal event EventHandler Finished;

        protected void Finish()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        public ExtendedSplashScreen ExtendedSplashScreen
        {
            get
            {
                return this.Parent as ExtendedSplashScreen;
            }
        }
    }
}