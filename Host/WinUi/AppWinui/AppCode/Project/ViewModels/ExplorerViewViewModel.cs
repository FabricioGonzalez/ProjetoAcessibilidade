﻿using System.Collections.ObjectModel;
using System.Reactive;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Entities.FileTemplate;

using AppWinui.AppCode.Project.States;

using Common;

using DynamicData;
using DynamicData.Binding;

using ReactiveUI;

using static Common.Resource<System.Collections.Generic.List<AppUsecases.Entities.FileTemplate.ExplorerItem>>;

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

    private readonly IUsecaseContract<string, List<ExplorerItem>> GetProjectItemsUsecase;

    public ExplorerItemState ExplorerState
    {
        get; private set;
    }

    public ExplorerViewViewModel(IUsecaseContract<string, List<ExplorerItem>> usecase)
    {
        Source = new ObservableCollectionExtended<Resource<ExplorerItem>>();

        ExplorerState = new();

        GetProjectItemsUsecase = usecase;

        this.WhenAnyValue(x => x.ProjectRootPath)
            .WhereNotNull()
            .Subscribe(async x =>
            {
                ExplorerState.ErrorMessage = null;
                ExplorerState.IsNotLoading = false;
                ExplorerState.Items = null;

                var result = await GetProjectItemsUsecase.executeAsync(x);

                if (result is Error err)
                {
                    ExplorerState.ErrorMessage = err?.Message;
                    ExplorerState.IsNotLoading = false;
                    ExplorerState.Items = null;
                }
                if (result is Success success)
                {
                    ExplorerState.ErrorMessage = null;
                    ExplorerState.IsNotLoading = true;
                    ExplorerState.Items = success.Data;
                }
            }
            ).Dispose();

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

        /* items.Subscribe((observer) =>
         {

         });*/
    }



}