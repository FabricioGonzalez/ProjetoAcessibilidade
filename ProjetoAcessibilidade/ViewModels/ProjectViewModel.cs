using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

using SystemApplication.Services.UIOutputs;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using Projeto.Core.Models;

using ProjetoAcessibilidade.Contracts.ViewModels;
using System.Threading.Tasks;
using ProjetoAcessibilidade.TemplateSelector;

namespace ProjetoAcessibilidade.ViewModels;

public class ProjectViewModel : ObservableRecipient, INavigationAware
{
    #region Bindings
    private ObservableCollection<TabViewItem> tabViewItems = new();
    public ObservableCollection<TabViewItem> TabViewItems
    {
        get => tabViewItems;
        set => tabViewItems = value;
    }

    private ReportDataOutput reportData;
    public ReportDataOutput ReportData
    {
        get => reportData;
        set
        {
            reportData = value;
            OnPropertyChanged(nameof(ReportData));
        }
    }
    
    private string solutionPath;
    public string SolutionPath
    {
        get => solutionPath;
        set
        {
            solutionPath = value;
            OnPropertyChanged(nameof(SolutionPath));
        }
    }

    private ObservableCollection<ExplorerItem> items;
    public ObservableCollection<ExplorerItem> Items
    {
        get => items;
        set => SetProperty(ref items, value, nameof(Items));
    } 
    #endregion

    #region Commands

    private ICommand _addItemCommand;
    public ICommand AddItemCommand => _addItemCommand ??= new RelayCommand<string>(OnAddItemCommand);

    private ICommand _addItemToProjectCommand;
    public ICommand AddItemToProjectCommand => _addItemToProjectCommand ??= new RelayCommand<string>(OnAddItemToProjectCommand);
    
    private ICommand _addFolderToProjectCommand;
    public ICommand AddFolderToProjectCommand => _addFolderToProjectCommand ??= new RelayCommand<string>(OnAddFolderToProjectCommand);

    private ICommand _textBoxLostFocusCommand;
    public ICommand TextBoxLostFocusCommand => _textBoxLostFocusCommand ??= new AsyncRelayCommand<string>(OnTextFieldLostFocus);

    #endregion

    #region CommandMethods
    private void OnAddItemCommand(string itemName)
    {
        var grid = new Grid()
        {
            Background = new SolidColorBrush() { Color = Colors.Aqua },
            VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch,
            CornerRadius = new Microsoft.UI.Xaml.CornerRadius(4),
        };
        grid.Children.Add(new TextBlock() { Text = $"{itemName}" });

        var item = new TabViewItem()
        {
            Header = $"{itemName}",
            Content = grid,
            CornerRadius = new Microsoft.UI.Xaml.CornerRadius(4),
            Background = new SolidColorBrush() { Color = Colors.Aqua },
            VerticalContentAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch,
        };
        TabViewItems.Add(item);
    }
    private void OnAddItemToProjectCommand(string obj)
    {
    }
    private void OnAddFolderToProjectCommand(string obj)
    {
    }
    private async Task OnTextFieldLostFocus(string data)
    {
        await Task.Run(() =>
        {
            Debug.WriteLine(data);
        });
    }
    #endregion
   
    #region Constructor
    public ProjectViewModel()
    {
        Items = GetData();
    } 
    #endregion

    private ObservableCollection<ExplorerItem> GetData()
    {
        var list = new ObservableCollection<ExplorerItem>();
        ExplorerItem folder1 = new ExplorerItem()
        {
            Name = "Work Documents",
            Type = ExplorerItem.ExplorerItemType.Folder,
            Children =
                {
                    new ExplorerItem()
                    {
                        Name = "Functional Specifications",
                        Type = ExplorerItem.ExplorerItemType.Folder,
                        Children =
                        {
                            new ExplorerItem()
                            {
                                Name = "TreeView spec",
                                Type = ExplorerItem.ExplorerItemType.File,
                              }
                        }
                    },
                    new ExplorerItem()
                    {
                        Name = "Feature Schedule",
                        Type = ExplorerItem.ExplorerItemType.File,
                    },
                    new ExplorerItem()
                    {
                        Name = "Overall Project Plan",
                        Type = ExplorerItem.ExplorerItemType.File,
                    },
                    new ExplorerItem()
                    {
                        Name = "Feature Resources Allocation",
                        Type = ExplorerItem.ExplorerItemType.File,
                    }
                }
        };
        ExplorerItem folder2 = new ExplorerItem()
        {
            Name = "Personal Folder",
            Type = ExplorerItem.ExplorerItemType.Folder,
            Children =
                        {
                            new ExplorerItem()
                            {
                                Name = "Home Remodel Folder",
                                Type = ExplorerItem.ExplorerItemType.Folder,
                                Children =
                                {
                                    new ExplorerItem()
                                    {
                                        Name = "Contractor Contact Info",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Paint Color Scheme",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Flooring Woodgrain type",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Kitchen Cabinet Style",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    }
                                }
                            }
                        }
        };

        list.Add(folder1);
        list.Add(folder2);
        return list;
    }
    
    #region InterfaceImplementedMethods
    public void OnNavigatedFrom()
    {
        return;
    }
    public void OnNavigatedTo(object parameter)
    {
        if (parameter is ProjectSolutionModel data)
        {
            ReportData = new ReportDataOutput();
            ReportData.Data = data.reportData.Data;
            ReportData.Telefone = data.reportData.Telefone;
            ReportData.Responsavel = data.reportData.Responsavel;
            ReportData.UF = data.reportData.UF;
            ReportData.LogoPath = data.reportData.LogoPath;
            ReportData.Email = data.reportData.Email;
            ReportData.Endereco = data.reportData.Endereco;
            ReportData.NomeEmpresa = data.reportData.NomeEmpresa;
            ReportData.SolutionName = data.reportData.SolutionName;

            solutionPath = data.ParentFolderPath;
        }
    } 
    #endregion

}
