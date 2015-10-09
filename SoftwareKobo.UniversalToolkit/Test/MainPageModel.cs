using SoftwareKobo.UniversalToolkit.Mvvm;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Test
{
    public class MainPageModel : ViewModelBase
    {
        private bool _isOpen;

        public bool IsOpen
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

        protected override async void ReceiveFromView(FrameworkElement originSourceView, object parameter)
        {
            await new MessageDialog((parameter as string) ?? "").ShowAsync();

            SendToView("hi, view.");
        }
    }
}