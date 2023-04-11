using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Logging;
using ProjectAvalonia.Stores;
using ProjectAvalonia.ViewModels;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class ProjectExplorerViewModel : ViewModelBase
{
    private readonly EditingItemsStore _editingItemsStore;
    private readonly SolutionStore _solutionStore;
    [AutoNotify] private bool _isDocumentSolutionEnabled;
    [AutoNotify] private ReadOnlyObservableCollection<ItemGroupState> _items;
    [AutoNotify] private string _phoneMask = "(00)0000-0000";

    [AutoNotify] private ItemState _selectedItem;

    public ProjectExplorerViewModel()
    {
        _solutionStore ??= Locator.Current.GetService<SolutionStore>();
        _editingItemsStore ??= Locator.Current.GetService<EditingItemsStore>();

        this.WhenValueChanged(propertyAccessor: vm =>
                vm
                    .CurrentOpenSolution
                    .ReportData
                    .Telefone).Where(predicate: value => !string.IsNullOrWhiteSpace(value: value))
            .Subscribe(onNext: value =>
            {
                var val = value
                    .Replace(oldValue: "(", newValue: "")
                    .Replace(oldValue: ")", newValue: "")
                    .Replace(oldValue: "-", newValue: "")
                    .Replace(oldValue: "_", newValue: "");

                if (val
                        .Length <= 10)
                {
                    PhoneMask = "(00)0000-0000";
                }
                else
                {
                    PhoneMask = "(00)00000-0000";
                }

                Logger.LogDebug(message: $"{PhoneMask} - {val}");
            });

        this.WhenAnyValue(property1: vm => vm._solutionStore.CurrentOpenSolution.ItemGroups)
            .Subscribe(onNext: prop =>
            {
                Items = prop;
            });

        CreateItemCommand = ReactiveCommand.Create<ItemState?>(execute: item =>
        {
            if (item is not null)
            {
                _editingItemsStore.EditItem(item: item);
            }
        });
    }

    public SolutionState CurrentOpenSolution => _solutionStore?.CurrentOpenSolution;

    public ICommand CreateItemCommand
    {
        get;
    }


    public ICommand OpenSolutionCommand
    {
        get;
        set;
    }
}