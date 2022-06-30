using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace ProjetoAcessibilidade.ViewModels;

public class ProjectViewModel : ObservableRecipient
{
    private ObservableCollection<TabViewItem> tabViewItems = new();
    public ObservableCollection<TabViewItem> TabViewItems
    {
        get => tabViewItems;
        set => tabViewItems = value;
    }

    private ICommand _addItemCommand;
    public ICommand AddItemCommand => _addItemCommand ??= new RelayCommand(OnAddItemCommand);

    private int count = 0;

    private void OnAddItemCommand()
    {
        count++;

        var grid = new Grid()
        {
            Background = new SolidColorBrush() { Color = Colors.Aqua },
            VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch,
            CornerRadius = new Microsoft.UI.Xaml.CornerRadius(4),
        };
        grid.Children.Add(new TextBlock() { Text = $"ok {count}" });

        var item = new TabViewItem()
        {
            Header = $"project {count}",
            Content = grid,
            CornerRadius = new Microsoft.UI.Xaml.CornerRadius(4),
            Background = new SolidColorBrush() { Color = Colors.Aqua },
            VerticalContentAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch,
        };
        TabViewItems.Add(item);
    }

    public ProjectViewModel()
    {
    }

    public void OnNavigatedFrom()
    {
        return;
    }
    public void OnNavigatedTo(object parameter)
    {
        if (parameter is not null)
        {
            var param = parameter;
            Debug.WriteLine(param);
        }
    }
}
