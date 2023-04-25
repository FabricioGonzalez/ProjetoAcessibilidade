using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Brush = System.Drawing.Brush;

namespace ProjectAvalonia.Common.Controls;

public partial class Stepper : UserControl
{
    public Stepper()
    {
        InitializeComponent();
        Update();
    }

    private int _index;

    public static readonly DirectProperty<Stepper, int> IndexProperty = AvaloniaProperty.RegisterDirect<Stepper, int>(
        nameof(Index), numpicker => numpicker.Index,
        (
            numpicker
            , value
        ) => numpicker.Index = value,defaultBindingMode: BindingMode.TwoWay,enableDataValidation:true);

    public int Index
    {
        get => _index;
        set
        {
            SetAndRaise(IndexProperty, ref _index, value);
            Update();
        }
    }

    private ObservableCollection<string> _steps;

    public static readonly DirectProperty<Stepper, ObservableCollection<string>> StepsProperty = AvaloniaProperty.RegisterDirect<Stepper, int>(
        nameof(Steps), numpicker => numpicker.Steps,
        (
            numpicker
            , value
        ) => numpicker.Index = value,defaultBindingMode: BindingMode.TwoWay,enableDataValidation:true);

    public ObservableCollection<string> Steps
    {
        get => _steps;
        set
        {
            SetAndRaise(StepsProperty, ref _steps, value);
            Update();
        }
    }

    public void Update()
    {
        try
        {
            Grid grid = this.FindControl<Grid>("gridStepper");

            grid.Children.Clear();

            SetColumnDefinitions(grid);

            for (int i = 0; i < Steps.Count; i++)
            {
                AddStep(Steps[i], i, grid);

            }
        }catch(Exception ex){}
    }

    private void SetColumnDefinitions(
        Grid grid
    )
    {
        var columns = new ColumnDefinitions();

        foreach (var step in Steps)
        {
            columns.Add(new ColumnDefinition());
            grid.ColumnDefinitions = columns;
        }
    }

    private void AddStep(
        string step
        , int index
        , Grid grid
    )
    {
        var PrimaryColor = new SolidColorBrush((Color)Application.Current.FindResource("SystemAccentColor"));
        var DisabledColor = new SolidColorBrush((Color)Application.Current.FindResource("SystemChromeDisabledLowColor"));

        var gridItem = new Grid()
        {
            ColumnDefinitions = { new ColumnDefinition(), new ColumnDefinition() }
        };

        var line = new Border() { CornerRadius = new CornerRadius(3),Margin = new Thickness(-5,0,23,0),Background = DisabledColor,Height = 2,HorizontalAlignment = HorizontalAlignment.Stretch,VerticalAlignment = VerticalAlignment.Center};
        var line1 = new Border() { CornerRadius = new CornerRadius(3),Margin = new Thickness(-5,0,23,0),Background = DisabledColor,Height = 2,HorizontalAlignment = HorizontalAlignment.Stretch,VerticalAlignment = VerticalAlignment.Center};

        if (index == 0)
        {
            line.IsVisible = false;
        }

        if (index == Steps.Count - 1)
        {
            line1.IsVisible = false;
        }

        if()

    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}