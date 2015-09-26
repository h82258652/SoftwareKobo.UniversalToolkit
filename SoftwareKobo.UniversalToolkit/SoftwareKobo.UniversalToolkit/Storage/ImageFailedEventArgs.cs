using System;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    /// <summary>
    /// 图片加载失败事件数据。
    /// </summary>
    public class ImageFailedEventArgs : EventArgs
    {
        public ImageFailedEventArgs(Exception failReason)
        {
            this.FailReason = failReason;
        }

        /// <summary>
        /// 导致加载失败的异常。
        /// </summary>
        public Exception FailReason
        {
            get;
            private set;
        }
    }
}