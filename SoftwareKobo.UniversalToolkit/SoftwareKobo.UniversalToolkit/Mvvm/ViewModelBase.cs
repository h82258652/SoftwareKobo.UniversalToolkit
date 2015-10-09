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

        /// <summary>
        /// 接收来自 View 的消息。
        /// </summary>
        /// <param name="originSourceView">发送消息的 View。</param>
        /// <param name="parameter">消息内容。</param>
        protected internal virtual void ReceiveFromView(FrameworkElement originSourceView, object parameter)
        {
        }

        /// <summary>
        /// 发送消息到对应的 View。
        /// </summary>
        /// <param name="parameter">消息内容。</param>
        protected void SendToView(object parameter)
        {
            Messenger.Process(this, parameter);
        }
    }
}