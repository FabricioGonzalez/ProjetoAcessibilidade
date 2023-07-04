using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DynamicData;
using DynamicData.Binding;
using ProjectAvalonia.Features.TemplateEdit.ViewModels;
using ProjectAvalonia.Models.ValidationTypes;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.Views.Components;

public partial class RulePreviewerComponent : UserControl
{
    public static readonly StyledProperty<string>
        ContainerIdProperty =
            AvaloniaProperty.Register<RulePreviewerComponent, string>(
                nameof(ContainerId)
                , "");


    public static readonly StyledProperty<ObservableCollection<IValidationRuleContainerState>>
        SourceValidationRulesProperty =
            AvaloniaProperty
                .Register<RulePreviewerComponent, ObservableCollection<IValidationRuleContainerState>>(
                    nameof(SourceValidationRules));

    public static readonly StyledProperty<ObservableCollection<OptionsItemState>>
        OptionsProperty =
            AvaloniaProperty
                .Register<RulePreviewerComponent, ObservableCollection<OptionsItemState>>(
                    nameof(Options),
                    new ObservableCollection<OptionsItemState>());

    public RulePreviewerComponent()
    {
        InitializeComponent();

        this.WhenAnyValue(view => view.SourceValidationRules)
            .WhereNotNull()
            .Subscribe(it =>
            {
                if (it.FirstOrDefault(rule => rule.TargetContainerId == ContainerId) is { } rules)
                {
                    ValidationRules = rules?.ValidaitonRules;
                }
            });

        ValidationRules.ToObservableChangeSet().OnItemAdded(item =>
        {
            if (SourceValidationRules.FirstOrDefault(it => it.TargetContainerId == ContainerId) is { } col)
            {
                col.ValidaitonRules.Add(item);
            }
        });
    }

    public ObservableCollection<IValidationRuleState> ValidationRules
    {
        get;
        set;
    } = new();

    public string ContainerId
    {
        get => GetValue(ContainerIdProperty);
        set => SetValue(ContainerIdProperty, value);
    }

    public ObservableCollection<OptionsItemState> Options
    {
        get => GetValue(OptionsProperty);
        set => SetValue(OptionsProperty, value);
    }

    public ObservableCollection<IValidationRuleContainerState> SourceValidationRules
    {
        get => GetValue(SourceValidationRulesProperty);
        set => SetValue(SourceValidationRulesProperty, value);
    }


    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void AddRuleButtonClicked(
        object? sender
        , RoutedEventArgs e
    ) => ValidationRules.Add(new ValidationRuleState
    {
        ValidationRuleName = "Inicial", Conditions = new ObservableCollection<IConditionState>()
        , Type = new ValueCheckOperation()
    });

    private async void EditRuleButtonClicked(
        object? sender
        , RoutedEventArgs e
    )
    {
        if (sender is Control control && control.DataContext is IValidationRuleState validationRuleState)
        {
            var vm = new EditRuleDialogViewModel(validationRuleState);

            if (Options.Any())
            {
                vm.SetCheckboxItems(Options);
            }

            var result = await RoutableViewModel.NavigateDialogAsync(vm, NavigationTarget.DialogScreen);

            Debug.WriteLine(result.Kind);

            if (result is { Result: ValidationRuleState, Kind: DialogResultKind.Normal } ok)
            {
                if (SourceValidationRules.FirstOrDefault(rule => rule.TargetContainerId == ContainerId) is { } rules)
                {
                    rules.ValidaitonRules = ValidationRules;
                }
            }
        }
    }
}