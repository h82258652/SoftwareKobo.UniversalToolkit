using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public interface IIncrementalItemSource<TItem>
    {
        [SuppressMessage("Microsoft.Design", "CA1009")]
        event EventHandler<bool> RaiseHasMoreItemsChanged;

        Task<uint> LoadMoreItemsAsync(ICollection<TItem> collection, uint suggestLoadCount);
    }
}