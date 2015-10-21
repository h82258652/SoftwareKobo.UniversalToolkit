using System;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public class AdaptiveCollectionViewModeChangedEventArgs : EventArgs
    {
        public AdaptiveCollectionViewModeChangedEventArgs(AdaptiveCollectionViewMode oldMode, AdaptiveCollectionViewMode newMode)
        {
            this.OldMode = oldMode;
            this.NewMode = NewMode;
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