using SoftwareKobo.UniversalToolkit.Mvvm;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Test
{
    public class MainPageModel : ViewModelBase
    {
        private bool? _isOpen = false;

        public bool? IsOpen
        {
            get
            {
                return this._isOpen;
            }
            set
            {
                Set(ref _isOpen, value);
            }
        }

        public string Name
        {
            get
            {
                return "Hello world";
            }
        }

        protected override async void ReceiveFromView(FrameworkElement originSourceView, object parameter)
        {
            await new MessageDialog((parameter as string) ?? "").ShowAsync();

            SendToView("hi, view.");
        }
    }
}