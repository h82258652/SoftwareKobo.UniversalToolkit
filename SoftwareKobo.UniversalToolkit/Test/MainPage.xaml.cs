using Newtonsoft.Json;
using SoftwareKobo.UniversalToolkit.Extensions;
using SoftwareKobo.UniversalToolkit.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           var rr= e.Parameter;
            base.OnNavigatedTo(e);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker f = new FileSavePicker();
            f.FileTypeChoices.Add("Images", new string[] { ".jpg" });
           var r=  await f.PickSaveFileAsync();

           //var application= Package.Current.Manifest().Applications.First();
           // await new MessageDialog(application.Id).ShowAsync();
           //var vi= application.VisualElements;
           // await new MessageDialog(vi.ToString()).ShowAsync();
           // await new MessageDialog(vi.BackgroundColor.ToString()).ShowAsync();
           // var ffq = vi.SplashScreen;
           // await new MessageDialog(ffq.ToString()).ShowAsync();
        }
    }
}
