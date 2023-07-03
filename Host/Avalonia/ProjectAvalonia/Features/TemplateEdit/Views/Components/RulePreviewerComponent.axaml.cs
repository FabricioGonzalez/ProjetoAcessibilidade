using System;
using System.Collections.ObjectModel;
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

    public ObservableCollection<IValidationRuleContainerState> SourceValidationRules
    {
        get => GetValue(SourceValidationRulesProperty);
        set => SetValue(SourceValidationRulesProperty, value);
    }


    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void AddRuleButtonClicked(object? sender, RoutedEventArgs e) => ValidationRules.Add(new ValidationRuleState
    {
        ValidationRuleName = "Inicial", Conditions = new ObservableCollection<IConditionState>(),
        Type = new ValueCheckOperation()
    });

    private async void EditRuleButtonClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Control control && control.DataContext is IValidationRuleState validationRuleState)
        {
            var vm = new EditRuleDialogViewModel(validationRuleState);

            var result = await RoutableViewModel.NavigateDialogAsync(vm, NavigationTarget.DialogScreen);
        }
    }
}