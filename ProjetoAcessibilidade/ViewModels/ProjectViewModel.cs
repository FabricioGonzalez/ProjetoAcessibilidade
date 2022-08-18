using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Windows.Storage;
using Windows.ApplicationModel;

using SystemApplication.Services.UIOutputs;
using SystemApplication.Services.ProjectDataServices;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

using Projeto.Core.Models;

using ProjetoAcessibilidade.Contracts.ViewModels;
using ProjetoAcessibilidade.Services;
using ProjetoAcessibilidade.ViewModels.DialogViewModel;

using CustomControls;

namespace ProjetoAcessibilidade.ViewModels;

public class ProjectViewModel : ObservableRecipient, INavigationAware
{
    #region Bindings
    private ObservableCollection<ProjectEditingTabViewItem> tabViewItems = new();
    public ObservableCollection<ProjectEditingTabViewItem> TabViewItems
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
    readonly CreateProjectData createProjectData;
    #endregion

    #region Commands

    private ICommand _addItemCommand;
    public ICommand AddItemCommand => _addItemCommand ??= new RelayCommand<ExplorerItem>(OnAddItemCommand);

    private ICommand _addItemToProjectCommand;
    public ICommand AddItemToProjectCommand => _addItemToProjectCommand ??= new RelayCommand<ExplorerItem>(OnAddItemToProjectCommand);

    private ICommand _addFolderToProjectCommand;
    public ICommand AddFolderToProjectCommand => _addFolderToProjectCommand ??= new RelayCommand<ExplorerItem>(OnAddFolderToProjectCommand);

    private ICommand _textBoxLostFocusCommand;
    public ICommand TextBoxLostFocusCommand => _textBoxLostFocusCommand ??= new AsyncRelayCommand<string>(OnTextFieldLostFocus);

    #endregion

    #region CommandMethods
    private void OnAddItemCommand(ExplorerItem itemName)
    {
        var itemTemplate = new ProjectItemTemplate();

        itemTemplate.ProjectItem = getProjectData.GetItemProject(itemName.Path);

        var item = new ProjectEditingTabViewItem()
        {
            Header = $"{itemName.Name}",
            Content = itemTemplate,
            CornerRadius = new Microsoft.UI.Xaml.CornerRadius(4),
            Background = new SolidColorBrush() { Color = Colors.Aqua },
            VerticalContentAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch,
            itemPath = itemName.Path

        };

        if (!TabViewItems.Any(item => item.itemPath.Equals(itemName.Path)))
        {
            TabViewItems.Add(item);
        }
    }
    private async void OnAddItemToProjectCommand(ExplorerItem obj)
    {
        var result = await newItemDialogService.ShowDialog();

        var item = new ExplorerItem()
        {
            Name = result.Name,
            Path = Path.Combine(obj.Path, $"{result.Name}.prjd"),
            Type = ExplorerItem.ExplorerItemType.File
        };

        obj.Children.Add(item);
        createProjectData.CreateProjectItem(obj.Path, $"{item.Name}.prjd", result.Path);
    }
    private void OnAddFolderToProjectCommand(ExplorerItem obj)
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
    public ProjectViewModel(GetProjectData getProject, NewItemDialogService newItemDialog, CreateProjectData createProjectData)
    {
        getProjectData = getProject;
        this.createProjectData = createProjectData;
        newItemDialogService = newItemDialog;
    }
    #endregion



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

            ObservableCollection<ExplorerItem> result = new();
            Task.Run(async () =>
            {
                result = await getProjectData.GetProjectSolutionItens(solutionPath);
            }).Wait();

            Items = result;
        }
    }
    #endregion

}
