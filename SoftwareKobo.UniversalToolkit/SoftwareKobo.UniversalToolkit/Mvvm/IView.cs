namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 实现该接口以进行与 ViewModel 的通信。
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 获取或设置 View 参与数据绑定时的数据上下文。
        /// </summary>
        object DataContext
        {
            get;
            set;
        }

        /// <summary>
        /// 实现该方法以处理来自 ViewModel 的消息。
        /// </summary>
        /// <param name="parameter">消息内容。</param>
        void ReceiveFromViewModel(dynamic parameter);
    }
}