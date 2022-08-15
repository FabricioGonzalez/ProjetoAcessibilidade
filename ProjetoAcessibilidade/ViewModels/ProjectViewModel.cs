using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;

using Windows.Storage;
using Windows.ApplicationModel;

using SystemApplication.Services.UIOutputs;
using SystemApplication.Services.ProjectDataServices;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using Projeto.Core.Models;

using ProjetoAcessibilidade.Contracts.ViewModels;
using ProjetoAcessibilidade.Services;
using ProjetoAcessibilidade.ViewModels.DialogViewModel;

using CustomControls.TemplateSelectors;
using CustomControls;

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
        set => SetProperty(ref items, value);
    }
    #endregion

    #region Dependencies
    readonly GetProjectData getProjectData;
    readonly NewItemDialogService newItemDialogService;
    #endregion

    #region Commands

    private ICommand _addItemCommand;
    public ICommand AddItemCommand => _addItemCommand ??= new RelayCommand<ExplorerItem>(OnAddItemCommand);

    private ICommand _addItemToProjectCommand;
    public ICommand AddItemToProjectCommand => _addItemToProjectCommand ??= new RelayCommand<object>(OnAddItemToProjectCommand);

    private ICommand _addFolderToProjectCommand;
    public ICommand AddFolderToProjectCommand => _addFolderToProjectCommand ??= new RelayCommand<object>(OnAddFolderToProjectCommand);

    private ICommand _textBoxLostFocusCommand;
    public ICommand TextBoxLostFocusCommand => _textBoxLostFocusCommand ??= new AsyncRelayCommand<string>(OnTextFieldLostFocus);

    #endregion

    #region CommandMethods
    private void OnAddItemCommand(ExplorerItem itemName)
    {

        var itemTemplate = new ProjectItemTemplate();


        itemTemplate.ProjectItem = getProjectData.GetItemProject(itemName.Path);


        var item = new TabViewItem()
        {
            Header = $"{itemName.Name}",
            Content = itemTemplate,
            CornerRadius = new Microsoft.UI.Xaml.CornerRadius(4),
            Background = new SolidColorBrush() { Color = Colors.Aqua },
            VerticalContentAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch,
        };
        TabViewItems.Add(item);
    }
    private void OnAddItemToProjectCommand(object obj)
    {
        newItemDialogService.ShowDialog<NewItemViewModel>("Escolha um item");
    }
    private void OnAddFolderToProjectCommand(object obj)
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
    public ProjectViewModel(GetProjectData getProject, NewItemDialogService newItemDialog)
    {
        getProjectData = getProject;
        newItemDialogService = newItemDialog;
    }
    #endregion

    private async Task GetDataFromPath(StorageFolder folder, IList<ExplorerItem> list)
    {
        var itens = await folder.GetItemsAsync();

        var folderItem = new ExplorerItem
        {
            Name = folder.Name,
            Path = folder.Path,
            Type = ExplorerItem.ExplorerItemType.Folder,
            Children = new()
        };

        foreach (var item in itens)
        {
            if (item.IsOfType(StorageItemTypes.Folder))
            {
                var newfolder = await StorageFolder.GetFolderFromPathAsync(item.Path);

                await GetDataFromPath(newfolder, folderItem.Children);
            }
            if (item.IsOfType(StorageItemTypes.File))
            {
                var i = new ExplorerItem()
                {
                    Name = item.Name.Split(".")[0],
                    Path = item.Path,
                    Type = ExplorerItem.ExplorerItemType.File
                };
                folderItem.Children.Add(i);
            }
        }
        list.Add(folderItem);
    }

    private async Task<ObservableCollection<ExplorerItem>> GetData()
    {
        var list = new ObservableCollection<ExplorerItem>();

        var directory = await StorageFolder.GetFolderFromPathAsync(Path.Combine(Package.Current.InstalledPath, "Specifications"));

        //var directory = await StorageFolder.GetFolderFromPathAsync(Path.Combine(SolutionPath, "Itens"));

        await GetDataFromPath(directory, list);

        return list;
    }

    #region InterfaceImplementedMethods
    public void OnNavigatedFrom()
    {

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

            Items = Task.Run(async () => await GetData()).Result;
        }
    }
    #endregion

}
