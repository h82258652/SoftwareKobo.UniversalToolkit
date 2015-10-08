using SoftwareKobo.UniversalToolkit.Mvvm;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Test
{
    public class MainPageModel : ViewModelBase
    {
        protected override async void ReceiveFromView(FrameworkElement originSourceView, object parameter)
        {
            await new MessageDialog((parameter as string) ?? "").ShowAsync();

            SendToView("hi, view.");
        }
    }
}