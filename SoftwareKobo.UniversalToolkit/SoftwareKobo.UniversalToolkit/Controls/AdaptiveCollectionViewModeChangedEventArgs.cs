using System;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public class AdaptiveCollectionViewModeChangedEventArgs : EventArgs
    {
        public AdaptiveCollectionViewModeChangedEventArgs(AdaptiveCollectionViewMode oldMode, AdaptiveCollectionViewMode newMode)
        {
            OldMode = oldMode;
            NewMode = NewMode;
        }

        public AdaptiveCollectionViewMode OldMode
        {
            get;
            private set;
        }

        public AdaptiveCollectionViewMode NewMode
        {
            get;
            private set;
        }
    }
}