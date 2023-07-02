using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectAvalonia.Presentation.Interfaces;

namespace ProjectAvalonia.Features.TemplateEdit.Views.Components;

public partial class RulePreviewerComponent : UserControl
{
    public static readonly DirectProperty<RulePreviewerComponent, string>
        ContainerIdProperty =
            AvaloniaProperty.RegisterDirect<RulePreviewerComponent, string>(
                nameof(SourceValidationRules)
                , it => it.ContainerId
                , (
                    it
                    , newValue
                ) => it.ContainerId = newValue);


    public static readonly DirectProperty<RulePreviewerComponent, ObservableCollection<IValidationRuleState>>
        SourceValidationRulesProperty =
            AvaloniaProperty
                .RegisterDirect<RulePreviewerComponent, ObservableCollection<IValidationRuleState>>(
                    nameof(SourceValidationRules)
                    , it => it.SourceValidationRules
                    , (
                        it
                        , newValue
                    ) => it.SourceValidationRules = newValue);

    private string _containerId;
    private ObservableCollection<IValidationRuleState> _sourceValidationRules = new();

    public RulePreviewerComponent()
    {
        InitializeComponent();
    }

    public string ContainerId
    {
        get => _containerId;
        set => SetAndRaise(ContainerIdProperty, ref _containerId, value);
    }

    public ObservableCollection<IValidationRuleState> SourceValidationRules
    {
        get => _sourceValidationRules;
        set => SetAndRaise(SourceValidationRulesProperty, ref _sourceValidationRules, value);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}