using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public interface IIncrementalItemSource<TItem>
    {
        event EventHandler<bool> RaiseHasMoreItemsChanged;

        Task<uint> LoadMoreItemsAsync(ICollection<TItem> collection, uint suggestLoadCount);
    }
}