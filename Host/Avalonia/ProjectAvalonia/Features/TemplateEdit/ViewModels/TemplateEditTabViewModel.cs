using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Common.Optional;
using DynamicData;
using DynamicData.Binding;
using ProjectAvalonia.Models.ValidationTypes;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.Presentation.States.LawItemState;
using ProjectAvalonia.ViewModels;
using ProjetoAcessibilidade.Core.Enuns;
using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

[NavigationMetaData(
    Title = "Template Editing",
    Caption = "Manage general settings",
    Order = 0,
    Searchable = false,
    Category = "Templates",
    Keywords = new[]
    {
        "Settings", "General", "Bitcoin", "Dark", "Mode", "Run", "Computer", "System", "Start", "Background", "Close"
        , "Auto", "Copy", "Paste", "Addresses", "Custom", "Change", "Address", "Fee", "Display", "Format", "BTC", "sats"
    },
    IconName = "settings_general_regular")]
public partial class TemplateEditTabViewModel
    : TemplateEditTabViewModelBase
        , ITemplateEditTabViewModel
{
    private AppModelState _editingItem;

    [AutoNotify]
    private ObservableCollection<IValidationRuleContainerState> _editingItemRules;

    public TemplateEditTabViewModel()
    {
        var observable = this.WhenAnyValue(vm => vm.EditingItem)
            .WhereNotNull()
            .Select(x => x.FormData
                .ToObservableChangeSet()
                .AutoRefresh()
                .DisposeMany())
            .Switch()
            .WhenPropertyChanged(prop => prop.Type, false)
            .Subscribe(prop =>
                {
                    ChangeBody(prop.Sender);
                }, error =>
                {
                    Debug.WriteLine(error);
                },
                () =>
                {
                    Debug.WriteLine("Completado");
                });
    }

    public override MenuViewModel? ToolBar => null;

    public ReactiveCommand<FormItemContainer, Unit> RemoveItemCommand => ReactiveCommand.Create<FormItemContainer>(
        item =>
        {
            EditingItem.RemoveItem(item);
        });

    public ReactiveCommand<FormItemContainer, Unit> AddRuleToItemCommand => ReactiveCommand.Create<FormItemContainer>(
        item =>
        {
            EditingItem.RemoveItem(item);
        });

    public ReactiveCommand<FormItemContainer, Unit> EditRuleCommand => ReactiveCommand.Create<FormItemContainer>(
        item =>
        {
            EditingItem.RemoveItem(item);
        });

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;

    public ReactiveCommand<string, Unit> AddRuleCommand => ReactiveCommand.Create<string>(itemId =>
    {
        EditingItemRules
            .FirstOrDefault(it => it.TargetContainerId == itemId)
            ?.ValidaitonRules
            .Add(new ValidationRuleState { ValidationRuleName = itemId });
    });


    ReactiveCommand<IValidationRuleState, Unit> ITemplateEditTabViewModel.EditRuleCommand
    {
        get;
    }

    public ReactiveCommand<LawStateItem, Unit> RemoveLawCommand => ReactiveCommand.Create<LawStateItem>(item =>
    {
        EditingItem.RemoveLawItem(item);
    });

    public ReactiveCommand<Unit, Unit> AddItemCommand => ReactiveCommand.Create(() =>
    {
        EditingItem.AddFormItem(new FormItemContainer
        {
            Id = Guid.NewGuid().ToString(), Topic = "", Type = AppFormDataType.Texto
            , Body = new TextItemState("", "", id: Guid.NewGuid().ToString())
        });
    });

    public ReactiveCommand<Unit, Unit> AddLawCommand => ReactiveCommand.Create(() =>
    {
        EditingItem.AddLawItems(new LawStateItem());
    });

    public AppModelState EditingItem
    {
        get => _editingItem;
        set => this.RaiseAndSetIfChanged(ref _editingItem, value);
    }

    private void ChangeBody(
        FormItemContainer container
    )
    {
        var result = container.Body.ToOption()
            .Map(e => container.Type switch
            {
                AppFormDataType.Texto => container.ChangeItem(AppFormDataType.Texto)
                    .Reduce(() => new TextItemState(container.Body.Topic, "", id: container.Body.Id))
                , AppFormDataType.Checkbox => container.ChangeItem(AppFormDataType.Checkbox)
                    .Reduce(() => new CheckboxContainerItemState(container.Body.Topic, id: container.Body.Id))
            })
            .Reduce(() => container.Type switch
            {
                AppFormDataType.Texto => container.ChangeItem(AppFormDataType.Texto)
                    .Reduce(() => new TextItemState("", "", id: Guid.NewGuid().ToString()))
                , AppFormDataType.Checkbox => container.ChangeItem(AppFormDataType.Checkbox)
                    .Reduce(() => new CheckboxContainerItemState("", id: Guid.NewGuid().ToString()))
            });
        container.Body = result;

        container.AddToHistory(result);
    }
}