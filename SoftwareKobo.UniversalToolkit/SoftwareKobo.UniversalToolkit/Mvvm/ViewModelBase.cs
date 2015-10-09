using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 视图模型基类。
    /// </summary>
    public abstract class ViewModelBase : BindableBase
    {
        protected ViewModelBase()
        {
            Messenger.Register(this);
        }

        /// <summary>
        /// 指示当前是否处于设计模式。
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                return DesignMode.DesignModeEnabled;
            }
        }

        protected override void DisposeManagedObjects()
        {
            Messenger.Unregister(this);
        }

        protected internal virtual void ReceiveFromView(FrameworkElement originSourceView, object parameter)
        {
        }

        protected void SendToView(object parameter)
        {
            Messenger.Process(this, parameter);
        }
    }
}