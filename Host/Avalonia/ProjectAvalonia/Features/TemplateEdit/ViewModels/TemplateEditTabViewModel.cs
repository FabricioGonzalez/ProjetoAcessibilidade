using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Common.Optional;
using DynamicData;
using DynamicData.Binding;
using ProjectAvalonia.Models.ValidationTypes;
using ProjectAvalonia.Presentation.Enums;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.Presentation.States.LawItemState;
using ProjectAvalonia.ViewModels;
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

    [AutoNotify] private ObservableCollection<IValidationRuleContainerState> _editingItemRules;

    public TemplateEditTabViewModel()
    {
        var observable = this.WhenAnyValue(vm => vm.EditingItem)
            .WhereNotNull()
            .Select(x => x.FormData
                .ToObservableChangeSet()
                .AutoRefresh()
                .DisposeMany())
            .Switch()
            .WhenPropertyChanged(propertyAccessor: prop => prop.Type, notifyOnInitialValue: false)
            .Subscribe(onNext: prop =>
                {
                    ChangeBody(prop.Sender);
                }, onError: error =>
                {
                    Debug.WriteLine(error);
                },
                onCompleted: () =>
                {
                    Debug.WriteLine("Completado");
                });
    }

    public override MenuViewModel? ToolBar => null;

    public ReactiveCommand<FormItemContainer, Unit> AddRuleToItemCommand => ReactiveCommand.Create<FormItemContainer>(
        item =>
        {
            EditingItem.RemoveItem(item);
        });

    public ReactiveCommand<(string, IValidationRuleState), Unit> EditRuleCommand =>
        ReactiveCommand.Create<(string, IValidationRuleState)>(
            item =>
            {
                Debug.WriteLine(item.Item1);
            });

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;

    public ReactiveCommand<FormItemContainer, Unit> RemoveItemCommand => ReactiveCommand.Create<FormItemContainer>(
        item =>
        {
            EditingItem.RemoveItem(item);
        });

    public ReactiveCommand<string, Unit> AddRuleCommand => ReactiveCommand.Create<string>(itemId =>
    {
        if (EditingItemRules
                .FirstOrDefault(it => it.TargetContainerId == itemId) is { } res)
        {
            res?.ValidaitonRules
                .Add(new ValidationRuleState { ValidationRuleName = itemId });
            return;
        }

        EditingItemRules.Add(new ValidationRuleContainerState
        {
            TargetContainerId = itemId, ValidaitonRules = new ObservableCollection<IValidationRuleState>(
                new List<IValidationRuleState>
                    { new ValidationRuleState { ValidationRuleName = itemId } })
        });
    });


    ReactiveCommand<(string, IValidationRuleState), Unit> ITemplateEditTabViewModel.EditRuleCommand
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
            Id = Guid.NewGuid().ToString(), Topic = "", Type = AppFormDataType.Text
            , Body = new TextItemState(topic: "", textData: "", id: Guid.NewGuid().ToString())
        });
    });

    public ReactiveCommand<Unit, Unit> AddLawCommand => ReactiveCommand.Create(() =>
    {
        EditingItem.AddLawItems(new LawStateItem());
    });

    public AppModelState EditingItem
    {
        get => _editingItem;
        set => this.RaiseAndSetIfChanged(backingField: ref _editingItem, newValue: value);
    }

    private void ChangeBody(
        FormItemContainer container
    )
    {
        var result = container.Body.ToOption()
            .Map(e => container.Type switch
            {
                AppFormDataType.Text => container.ChangeItem(AppFormDataType.Text)
                    .Reduce(() => new TextItemState(topic: container.Body.Topic, textData: "", id: container.Body.Id))
                , AppFormDataType.Checkbox => container.ChangeItem(AppFormDataType.Checkbox)
                    .Reduce(() => new CheckboxContainerItemState(topic: container.Body.Topic, id: container.Body.Id))
            })
            .Reduce(() => container.Type switch
            {
                AppFormDataType.Text => container.ChangeItem(AppFormDataType.Text)
                    .Reduce(() => new TextItemState(topic: "", textData: "", id: Guid.NewGuid().ToString()))
                , AppFormDataType.Checkbox => container.ChangeItem(AppFormDataType.Checkbox)
                    .Reduce(() => new CheckboxContainerItemState(topic: "", id: Guid.NewGuid().ToString()))
            });
        container.Body = result;

        container.AddToHistory(result);
    }
}