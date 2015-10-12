using SoftwareKobo.UniversalToolkit.Mvvm;
using System;
using Test.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Test.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainView : Page, IView
    {
        public MainView()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.DataContext = new MainViewModel();
        }

        public void ReceiveFromViewModel(ViewModelBase originSourceViewModel, object parameter)
        {
            if (parameter is DateTime)
            {
                Frame.Navigate(typeof(DetailPage), parameter);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Messenger.Unregister(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Register(this);
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            this.SendToViewModel("gotonext");
        }

        private void BtnRefreshTime_Click(object sender, RoutedEventArgs e)
        {
            this.SendToViewModel("refresh");
        }

        private void BtnNewWindow_Click(object sender, RoutedEventArgs e)
        {
            App.Current.ShowNewWindowAsync<NewWindowPage>(Guid.NewGuid());
        }
    }
}