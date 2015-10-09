namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 继承该接口以接受来自 ViewModel 的消息。
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 实现该方法以处理来自 ViewModel 的消息。
        /// </summary>
        /// <param name="originSourceViewModel">发送该消息的 ViewModel。</param>
        /// <param name="parameter">消息内容。</param>
        void ReceiveFromViewModel(ViewModelBase originSourceViewModel, object parameter);
    }
}