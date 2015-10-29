using System;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public static class ViewExtensions
    {
        /// <summary>
        /// 发送消息到 ViewModel。
        /// </summary>
        /// <param name="view">需要发送消息的 View。</param>
        /// <param name="parameter">消息内容。</param>
        public static void SendToViewModel(this IView view, object parameter)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            Messenger.Process(view, parameter);
        }
    }
}