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
            InitializeComponent();

            _splashScreen = splashScreen;
            Content = content;

            Window.Current.SizeChanged += (sender, e) =>
            {
                SetExtendedSplashBackgroundLocation();
            };
            SetExtendedSplashBackgroundLocation();
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
            if (_splashScreen != null)
            {
                var extendedSplashBackgroundLocation = _splashScreen.ImageLocation;
                Canvas.SetLeft(imgExtendedSplashBackground, extendedSplashBackgroundLocation.Left);
                Canvas.SetTop(imgExtendedSplashBackground, extendedSplashBackgroundLocation.Top);

                var scaleFactor = (double)DisplayInformation.GetForCurrentView().ResolutionScale / 100.0d;
                imgExtendedSplashBackground.Width = extendedSplashBackgroundLocation.Width / scaleFactor;
                imgExtendedSplashBackground.Height = extendedSplashBackgroundLocation.Height / scaleFactor;
            }
        }

        internal static async Task<ExtendedSplashScreen> CreateAsync(SplashScreen splashScreen, ExtendedSplashScreenContent content)
        {
            var extendedSplashScreen = new ExtendedSplashScreen(splashScreen, content);

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
                    var imagePath = splashScreen.Image;
                    var imageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + imagePath, UriKind.Absolute));
                    var buffer = (await FileIO.ReadBufferAsync(imageFile)).ToArray();
                    using (var stream = new MemoryStream(buffer))
                    {
                        var bitmap = new BitmapImage();
                        await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
                        imgExtendedSplashBackground.Source = bitmap;
                    }
                }

                var backgroundColor = splashScreen.BackgroundColor.HasValue ? splashScreen.BackgroundColor.Value : visualElements.BackgroundColor;
                RootLayout.Background = new SolidColorBrush(backgroundColor);
            }
        }
    }
}