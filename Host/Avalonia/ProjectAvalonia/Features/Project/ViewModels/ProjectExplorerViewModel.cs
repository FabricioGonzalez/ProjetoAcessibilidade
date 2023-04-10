using System;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Logging;
using ProjectAvalonia.ViewModels;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class ProjectExplorerViewModel : ViewModelBase
{
    [AutoNotify] private bool _isDocumentSolutionEnabled;
    [AutoNotify] private string _phoneMask = "(00)0000-0000";

    [AutoNotify] private ItemState _selectedItem;
    [AutoNotify] private SolutionStateViewModel _solutionModel = new();

    public ProjectExplorerViewModel()
    {
        this.WhenValueChanged(propertyAccessor: vm =>
                vm
                    .SolutionModel
                    .ReportData
                    .Telefone)
            .Where(predicate: value => !string.IsNullOrWhiteSpace(value: value))
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
    }

    public ICommand CreateItemCommand
    {
        get;
        set;
    }

    public ICommand PrintProjectCommand
    {
        get;
        set;
    }

    public ICommand OpenSolutionCommand
    {
        get;
        set;
    }

    public ICommand SaveSolutionCommand
    {
        get;
        set;
    }
}