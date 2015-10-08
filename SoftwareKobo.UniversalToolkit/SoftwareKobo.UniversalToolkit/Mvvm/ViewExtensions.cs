using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public static class ViewExtensions
    {
        public static void SendToViewModel(this FrameworkElement view, object parameter)
        {
            Messenger.Process(view, parameter);
        }
    }
}