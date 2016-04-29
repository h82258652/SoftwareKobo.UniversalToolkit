using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public abstract class IncrementalItemSourceBase<TItem>
    {
        [SuppressMessage("Microsoft.Design", "CA1009")]
        public event EventHandler<bool> HasMoreItemsChanged;

        public void RaiseHasMoreItemsChanged(bool value)
        {
            HasMoreItemsChanged?.Invoke(this, value);
        }

        internal void InternalRefresh(ICollection<TItem> collection)
        {
            RaiseHasMoreItemsChanged(true);
            OnRefresh(collection);
        }

        protected internal abstract Task LoadMoreItemsAsync(ICollection<TItem> collection, uint suggestLoadCount);

        protected virtual void OnRefresh(ICollection<TItem> collection)
        {
        }
    }
}