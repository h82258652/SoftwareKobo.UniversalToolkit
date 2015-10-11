using SoftwareKobo.UniversalToolkit.Controls;
using SoftwareKobo.UniversalToolkit.Helpers;
using SoftwareKobo.UniversalToolkit.Mvvm;
using SoftwareKobo.UniversalToolkit.Storage;
using System;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page, IView
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            bool b = true;
            TestWriteRead(b);

            sbyte sb = 1;
            TestWriteRead(sb);

            byte by = 1;
            TestWriteRead(by);

            short sh = 1;
            TestWriteRead(sh);

            ushort ush = 1;
            TestWriteRead(ush);

            int i = 1;
            TestWriteRead(i);

            uint ui = 1;
            TestWriteRead(ui);

            long l = 1;
            TestWriteRead(l);

            ulong ul = 1;
            TestWriteRead(ul);

            float f = 1;
            TestWriteRead(f);

            double d = 1;
            TestWriteRead(d);

            decimal dd = 1;
            TestWriteRead(dd);

            char c = 'A';
            TestWriteRead(c);

            string sq = "sgq";
            TestWriteRead(sq);

            Dock df = Dock.Right;
            TestWriteRead(df);
        }

        public T TestWriteRead<T>(T value)
        {
            ApplicationLocalSettings.Write<T>("Temp", value);
            var temp = ApplicationLocalSettings.Read<T>("Temp");
            if (object.Equals(temp,value)==false)
            {
                throw new ArgumentException();
            }
            return temp;
        }

        private MainPageModel vm = new MainPageModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Register(this);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Messenger.Unregister(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //F.IsOpen = !F.IsOpen;
        }

        public async void ReceiveFromViewModel(ViewModelBase originSourceViewModel, object parameter)
        {
            Debug.WriteLine(parameter);
            await new MessageDialog((parameter as string) ?? "").ShowAsync();
        }

    }

    public enum TT : sbyte
    {

    }
}