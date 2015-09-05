using SoftwareKobo.UniversalToolkit.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Uri uri;
            try
            {
                uri = new Uri(txt.Text);
            }
            catch 
            {
                return;
            }
            var b = new StorageCachedImage(uri);
            b.ImageFailed += B_ImageFailed;
            img.Source = b;
     //       img.Source = new SoftwareKobo.UniversalToolkit.Storage.StorageCachedImage(uri);            
        }

        private void B_ImageFailed(object sender, ImageLoadFailedEventArgs e)
        {
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            long size = SoftwareKobo.UniversalToolkit.Storage.StorageCachedImage.GetCachedImagesSize();
            await new MessageDialog(size.ToString()).ShowAsync();
        }

        private async void img_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            await new MessageDialog("failed").ShowAsync();
        }
    }
}
