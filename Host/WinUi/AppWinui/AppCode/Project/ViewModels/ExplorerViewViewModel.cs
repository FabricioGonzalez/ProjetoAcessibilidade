using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.FileTemplate;
using AppUsecases.Project.Enums;

using AppWinui.AppCode.AppUtils.Services;
using AppWinui.AppCode.Project.States;

using Common;

using DynamicData;
using DynamicData.Binding;

using Microsoft.UI.Xaml.Controls;

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
    private readonly NewItemDialogService newItemDialogService;
    private readonly InfoBarService infoBarService;

    public ExplorerItemState ExplorerState
    {
        get; set;
    }

    public ExplorerViewViewModel()
    {
        Source = new ObservableCollectionExtended<Resource<ExplorerItem>>();

        ExplorerState = new();

        GetProjectItemsUsecase = App.GetService<IQueryUsecase<string, List<ExplorerItem>>>();
        newItemDialogService = App.GetService<NewItemDialogService>();
        infoBarService = App.GetService<InfoBarService>();

        AddItemCommand = ReactiveCommand.CreateFromTask<ExplorerItem>(async (obj) =>
        {
            try
            {
                var result = await newItemDialogService.ShowDialog();

                App.MainWindow.DispatcherQueue.TryEnqueue(() =>
                {
                    if (result is not null)
                    {
                        infoBarService.SetMessageData("Item Adicionado", result.Name, InfoBarSeverity.Informational);
                        var item = new ExplorerItem()
                        {
                            Name = result.Name,
                            Path = Path.Combine(obj.Path, $"{result.Name}.prjd"),
                        };

                        (obj as FolderItem).Children.Add(item);
                       /* createProjectData.CreateProjectItem(obj.Path, $"{item.Name}.prjd", result.Path);*/
                    }
                });

            }
            catch (Exception)
            {
                throw;
            }
        });

        this.WhenAnyValue(x => x.ProjectRootPath)
       .WhereNotNull()
       .Subscribe(async x =>
       {
           ExplorerState.ErrorMessage = null;
           ExplorerState.IsNotLoading = false;
           ExplorerState.Items = null;

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
    } = ReactiveCommand.Create<ExplorerItem>((obj) =>
    {
        Debug.WriteLine("Exclude");
    });

    public ICommand ExcludeItemCommand
    {
        get; set;
    } = ReactiveCommand.Create<ExplorerItem>((obj) =>
    {
        Debug.WriteLine("Exclude");
    });

    public ICommand AddItemCommand
    {
        get; set;
    } = ReactiveCommand.Create<ExplorerItem>((obj) =>
    {
        Debug.WriteLine("Exclude");
    });

    public ICommand AddFolderCommand
    {
        get; set;
    } = ReactiveCommand.Create<ExplorerItem>((obj) =>
    {
        Debug.WriteLine("Exclude");
    });
}