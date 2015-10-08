using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 视图模型基类。
    /// </summary>
    public abstract class ViewModelBase : BindableBase
    {
        private string _name;

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

        internal string GetName()
        {
            this._name = this._name ?? this.GetType().Name;
            return this._name;
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