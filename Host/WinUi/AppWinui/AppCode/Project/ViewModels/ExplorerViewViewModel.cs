using System.Collections.ObjectModel;

using AppUsecases.Entities.FileTemplate;

using Common;

using DynamicData;
using DynamicData.Binding;

using ReactiveUI;

using static Common.Resource<System.Collections.Generic.List<AppUsecases.Entities.FileTemplate.ExplorerItem>>;

namespace AppWinui.AppCode.Project.ViewModels;

public class ExplorerViewViewModel : ReactiveObject
{
    private ReadOnlyObservableCollection<Resource<ExplorerItem>> _items;
    public ReadOnlyObservableCollection<Resource<ExplorerItem>> Items => _items;

    private ObservableCollectionExtended<Resource<ExplorerItem>> Source;

    public bool GetResult()
    {
        var res = new IsLoading(null, true); 
        if (res is IsLoading loading)
        {
            return !loading.isLoading;
        }
        return true;
    }

    public ExplorerViewViewModel()
    {
        Source = new ObservableCollectionExtended<Resource<ExplorerItem>>();

        // Use the ToObservableChangeSet operator to convert
        // the observable collection to IObservable<IChangeSet<T>>
        // which describes the changes. Then, use any DD operators
        // to transform the collection. 
        Source.ToObservableChangeSet()
            /*.Transform(value => !)*/
            // No need to use the .ObserveOn() operator here, as
            // ObservableCollectionExtended is single-threaded.
            .Bind(out _items)
            .Subscribe();
    }



}