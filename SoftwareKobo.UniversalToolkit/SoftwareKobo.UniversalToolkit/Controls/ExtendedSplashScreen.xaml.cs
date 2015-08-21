using SoftwareKobo.UniversalToolkit.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace SoftwareKobo.UniversalToolkit.Controls
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    internal sealed partial class ExtendedSplashScreen
    {
        private SplashScreen _splashScreen;

        private ExtendedSplashScreen(SplashScreen splashScreen,ExtendedSplashScreenContent content)
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

        public ExtendedSplashScreenContent Content
        {
            get
            {
                return (ExtendedSplashScreenContent)extendedSplashContent.Content;
            }
            set
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

        internal static async Task<ExtendedSplashScreen> CreateAsync(SplashScreen splashScreen,ExtendedSplashScreenContent content)
        {
            ExtendedSplashScreen extendedSplashScreen = new ExtendedSplashScreen(splashScreen,content);

            await extendedSplashScreen.InitSplashScreenAsync();

            return extendedSplashScreen;
        }

        private async Task InitSplashScreenAsync()
        {
            StorageFile appxManifestFile = await Package.Current.InstalledLocation.GetFileAsync("AppxManifest.xml");
            string appxManifestXml = await FileIO.ReadTextAsync(appxManifestFile);

            XDocument document = XDocument.Parse(appxManifestXml);
            XElement root = document.Root;
            string uapNamespace = root.GetNamespaceOfPrefix("uap").NamespaceName;
            XElement splashScreenElement = document.Descendants(XName.Get("SplashScreen", uapNamespace)).SingleOrDefault();
            if (splashScreenElement != null)
            {
                XAttribute imageAttribute = splashScreenElement.Attribute("Image");
                if (imageAttribute != null)
                {
                    string image = imageAttribute.Value;
                    this.imgExtendedSplashBackground.Source = new BitmapImage(new Uri("ms-appx:///" + image, UriKind.Absolute));
                }

                XAttribute backgroundColorAttribute = splashScreenElement.Attribute("BackgroundColor");
                if (backgroundColorAttribute != null)
                {
                    string backgroundColor = backgroundColorAttribute.Value;
                    this.Background = new SolidColorBrush(ColorExtensions.FromHex(backgroundColor));
                }
            }
        }
    }
}