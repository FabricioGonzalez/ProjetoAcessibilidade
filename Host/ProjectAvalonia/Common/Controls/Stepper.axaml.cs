using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ProjectAvalonia.Common.Controls;

public partial class Stepper : UserControl
{
    public static readonly DirectProperty<Stepper, int> IndexProperty = AvaloniaProperty.RegisterDirect<Stepper, int>(
        name: nameof(Index), getter: numpicker => numpicker.Index,
        setter: (
            numpicker
            , value
        ) => numpicker.Index = value, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public static readonly DirectProperty<Stepper, ObservableCollection<string>> StepsProperty =
        AvaloniaProperty.RegisterDirect<Stepper, ObservableCollection<string>>(
            name: nameof(Steps), getter: numpicker => numpicker.Steps,
            setter: (
                numpicker
                , value
            ) => numpicker.Steps = value, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    private int _index;

    private ObservableCollection<string> _steps;

    public Stepper()
    {
        InitializeComponent();
        Update();
    }

    public int Index
    {
        get => _index;
        set
        {
            SetAndRaise(property: IndexProperty, field: ref _index, value: value);
            Update();
        }
    }

    public ObservableCollection<string> Steps
    {
        get => _steps;
        set
        {
            SetAndRaise(property: StepsProperty, field: ref _steps, value: value);
            Update();
        }
    }

    public void Update()
    {
        try
        {
            var grid = this.FindControl<Grid>(name: "gridStepper");

            grid.Children.Clear();

            SetColumnDefinitions(grid: grid);

            for (var i = 0; i < Steps.Count; i++)
            {
                AddStep(step: Steps[index: i], index: i, grid: grid);
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void SetColumnDefinitions(
        Grid grid
    )
    {
        var columns = new ColumnDefinitions();

        foreach (var step in Steps)
        {
            columns.Add(item: new ColumnDefinition());
            grid.ColumnDefinitions = columns;
        }
    }

    private void AddStep(
        string step
        , int index
        , Grid grid
    )
    {
        var PrimaryColor =
            new SolidColorBrush(color: (Color)Avalonia.Application.Current.FindResource(key: "SystemAccentColor"));
        var DisabledColor =
            new SolidColorBrush(
                color: (Color)Avalonia.Application.Current.FindResource(key: "SystemChromeDisabledLowColor"));

        var gridItem = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(), new ColumnDefinition() }
        };

        var line = new Border
        {
            CornerRadius = new CornerRadius(uniformRadius: 3)
            , Margin = new Thickness(left: -5, top: 0, right: 23, bottom: 0), Background = DisabledColor, Height = 2
            , HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center
        };
        var line1 = new Border
        {
            CornerRadius = new CornerRadius(uniformRadius: 3)
            , Margin = new Thickness(left: -5, top: 0, right: 23, bottom: 0), Background = DisabledColor, Height = 2
            , HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center
        };

        if (index == 0)
        {
            line.IsVisible = false;
        }

        if (index == Steps.Count - 1)
        {
            line1.IsVisible = false;
        }
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(obj: this);
}