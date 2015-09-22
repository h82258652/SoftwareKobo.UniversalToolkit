using SoftwareKobo.UniversalToolkit.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Test
{
    public sealed partial class CustomSplashScreen : ExtendedSplashScreenContent
    {
        public CustomSplashScreen()
        {
            this.InitializeComponent();
            //this.Loaded += async (sender,e) =>
            //{
            //    await Task.Delay(3000);
            //    this.Finish();
            //};
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Finish();
        }

        private void FontIcon_Loaded(object sender, RoutedEventArgs e)
        {
           var icon = (FontIcon)sender;
            icon.Glyph = "&#xE700;";
        }
    }
}
