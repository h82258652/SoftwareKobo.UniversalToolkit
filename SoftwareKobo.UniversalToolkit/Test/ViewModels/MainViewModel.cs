using SoftwareKobo.UniversalToolkit.Mvvm;
using System;
using Windows.UI.Xaml;

namespace Test.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Time = DateTime.Now;
        }

        private DateTime _time;

        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                Set(ref _time, value);
            }
        }

        protected override void ReceiveFromView(FrameworkElement originSourceView, object parameter)
        {
            if ((parameter as string) == "refresh")
            {
                Time = DateTime.Now;
            }
            else if ((parameter as string) == "gotonext")
            {
                this.SendToView(Time);
            }
        }
    }
}