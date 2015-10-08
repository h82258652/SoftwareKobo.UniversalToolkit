using SoftwareKobo.UniversalToolkit.Mvvm;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            this.DataContext = new MainPageModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Register(this);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Messenger.UnRegister(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.SendToViewModel("hello, view model.");
        }

        public async void ReceiveFromViewModel(ViewModelBase originSourceViewModel, object parameter)
        {
            await new MessageDialog((parameter as string) ?? "").ShowAsync();
        }
    }
}