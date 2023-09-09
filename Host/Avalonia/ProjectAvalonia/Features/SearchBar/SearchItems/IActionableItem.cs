using System;
using System.Threading.Tasks;

namespace ProjectAvalonia.Features.SearchBar.SearchItems;

public interface IActionableItem : ISearchItem
{
    Func<Task> OnExecution
    {
        get;
    }
}