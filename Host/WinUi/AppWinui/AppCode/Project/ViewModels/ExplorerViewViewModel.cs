using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Entities.FileTemplate;

using AppWinui.AppCode.Project.States;

using Common;

using DynamicData;
using DynamicData.Binding;

using ReactiveUI;

namespace AppWinui.AppCode.Project.ViewModels;

public class ExplorerViewViewModel : ReactiveObject
{
    private string _projectRootPath = "";
    public string ProjectRootPath
    {
        get => _projectRootPath;
        set => this.RaiseAndSetIfChanged(
            ref _projectRootPath,
            value,
            nameof(ProjectRootPath));
    }

    private ReadOnlyObservableCollection<Resource<ExplorerItem>> _items;
    public ReadOnlyObservableCollection<Resource<ExplorerItem>> Items => _items;

    private ObservableCollectionExtended<Resource<ExplorerItem>> Source;

    private readonly IQueryUsecase<string, List<ExplorerItem>> GetProjectItemsUsecase;

    public ExplorerItemState ExplorerState
    {
        get; set;
    }

   


    public ExplorerViewViewModel()
    {
        Source = new ObservableCollectionExtended<Resource<ExplorerItem>>();

        ExplorerState = new();

        GetProjectItemsUsecase = App.GetService<IQueryUsecase<string, List<ExplorerItem>>>();


        this.WhenAnyValue(x => x.ProjectRootPath)
       .WhereNotNull()
       .Subscribe(async x =>
       {
           ExplorerState.ErrorMessage = null;
           ExplorerState.IsNotLoading = false;
           ExplorerState.Items = null;

           RenameItemCommand = ReactiveCommand.Create(() =>
           {
               Debug.WriteLine("Rename");
           });

           ExcludeItemCommand = ReactiveCommand.Create(() =>
           {
               Debug.WriteLine("Exclude");
           });

           AddItemCommand = ReactiveCommand.Create(() =>
           {

               Debug.WriteLine("Add Item");
           });

           AddFolderCommand = ReactiveCommand.Create(() =>
           {
               Debug.WriteLine("Add Folder");
           });

           var result = await GetProjectItemsUsecase.executeAsync(x);

           if (result is Resource<List<ExplorerItem>>.Error err)
           {
               ExplorerState.ErrorMessage = err?.Message;
               ExplorerState.IsNotLoading = false;
               ExplorerState.Items = null;
           }
           if (result is Resource<List<ExplorerItem>>.Success success)
           {
               ExplorerState.ErrorMessage = null;
               ExplorerState.IsNotLoading = true;
               ExplorerState.Items = success.Data;
           }
       }
       );

        // Use the ToObservableChangeSet operator to convert
        // the observable collection to IObservable<IChangeSet<T>>
        // which describes the changes. Then, use any DD operators
        // to transform the collection. 
        Source.ToObservableChangeSet()
            /*.Transform(value => !)*/
            // No need to use the .ObserveOn() operator here, as
            // ObservableCollectionExtended is single-threaded.
            .Bind(out _items)
            .Subscribe(x =>
            {

            });
    }
    public ICommand RenameItemCommand
    {
        get; set;
    }

    public ICommand ExcludeItemCommand
    {
        get; set;
    }

    public ICommand AddItemCommand
    {
        get; set;
    }

    public ICommand AddFolderCommand
    {
        get; set;
    }
}