using SoftwareKobo.UniversalToolkit.Utils.AppxManifest;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public sealed partial class ExtendedSplashScreen : UserControl
    {
        private SplashScreen _splashScreen;

        private ExtendedSplashScreen(SplashScreen splashScreen, ExtendedSplashScreenContent content)
        {
            this.InitializeComponent();

            this._splashScreen = splashScreen;
            this.Content = content;

            Window.Current.SizeChanged += (sender, e) =>
            {
                this.SetExtendedSplashBackgroundLocation();
            };
            this.SetExtendedSplashBackgroundLocation();
        }

        public new ExtendedSplashScreenContent Content
        {
            get
            {
                return (ExtendedSplashScreenContent)extendedSplashContent.Content;
            }
            private set
            {
                extendedSplashContent.Content = value;
            }
        }

        private void SetExtendedSplashBackgroundLocation()
        {
            if (this._splashScreen != null)
            {
                Rect extendedSplashBackgroundLocation = _splashScreen.ImageLocation;
                Canvas.SetLeft(imgExtendedSplashBackground, extendedSplashBackgroundLocation.Left);
                Canvas.SetTop(imgExtendedSplashBackground, extendedSplashBackgroundLocation.Top);

                double scaleFactor = (double)DisplayInformation.GetForCurrentView().ResolutionScale / 100.0d;
                imgExtendedSplashBackground.Width = extendedSplashBackgroundLocation.Width / scaleFactor;
                imgExtendedSplashBackground.Height = extendedSplashBackgroundLocation.Height / scaleFactor;
            }
        }

        internal static async Task<ExtendedSplashScreen> CreateAsync(SplashScreen splashScreen, ExtendedSplashScreenContent content)
        {
            ExtendedSplashScreen extendedSplashScreen = new ExtendedSplashScreen(splashScreen, content);

            await extendedSplashScreen.InitSplashScreenAsync();

            return extendedSplashScreen;
        }

        private async Task InitSplashScreenAsync()
        {
            var visualElements = PackageManifest.Current.Applications.First().VisualElements;
            var splashScreen = visualElements.SplashScreen;
            if (splashScreen != null)
            {
                if (string.IsNullOrEmpty(splashScreen.Image) == false)
                {
                    string imagePath = splashScreen.Image;
                    StorageFile imageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + imagePath, UriKind.Absolute));
                    var buffer = (await FileIO.ReadBufferAsync(imageFile)).ToArray();
                    using (MemoryStream stream = new MemoryStream(buffer))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
                        this.imgExtendedSplashBackground.Source = bitmap;
                    }
                }

                Color backgroundColor = splashScreen.BackgroundColor.HasValue ? splashScreen.BackgroundColor.Value : visualElements.BackgroundColor;
                this.RootLayout.Background = new SolidColorBrush(backgroundColor);
            }
        }
    }
}