using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectAvalonia.Presentation.Interfaces;
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
}