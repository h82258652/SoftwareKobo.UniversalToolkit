using Windows.ApplicationModel;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 视图模型基类。
    /// </summary>
    public abstract class ViewModelBase : BindableBase
    {
        /// <summary>
        /// 指示当前是否处于设计模式。
        /// </summary>
        public static bool IsInDesignMode => DesignMode.DesignModeEnabled;

        /// <summary>
        /// 接收来自 View 的消息。
        /// </summary>
        /// <param name="parameter">消息内容。</param>
        protected internal virtual void ReceiveFromView(dynamic parameter)
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