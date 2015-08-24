using System;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    public class ImageLoadFailedEventArgs : EventArgs
    {
        public ImageLoadFailedEventArgs(Exception failReason)
        {
            this.FailReason = failReason;
        }

        public Exception FailReason
        {
            get;
            private set;
        }
    }
}