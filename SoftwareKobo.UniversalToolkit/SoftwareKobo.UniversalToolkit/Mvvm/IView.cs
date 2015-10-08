namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public interface IView
    {
        void ReceiveFromViewModel(ViewModelBase originSourceViewModel, object parameter);
    }
}