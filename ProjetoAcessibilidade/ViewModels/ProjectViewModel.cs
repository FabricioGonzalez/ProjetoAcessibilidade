using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq;

using SystemApplication.Services.UIOutputs;
using SystemApplication.Services.ProjectDataServices;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Projeto.Core.Models;

using ProjetoAcessibilidade.Contracts.ViewModels;
using ProjetoAcessibilidade.Services;

using Microsoft.UI.Xaml.Controls;
using ProjetoAcessibilidade.Controls.TabViews;
using ProjetoAcessibilidade.Stores;

namespace ProjetoAcessibilidade.ViewModels; 

[ObservableObject]
public partial class ProjectViewModel : INavigationAware
{
    #region Bindings
    [ObservableProperty]
    private ObservableCollection<ProjectEditingTabViewItem> tabViewItems = new();

    [ObservableProperty]
    private ReportDataOutput reportData = new();
  
    [ObservableProperty]
    private string solutionPath;

    [ObservableProperty]
    private ObservableCollection<ExplorerItem> items = new();
    
    #endregion
    private ProjectStore projectStore = new();
    #region Dependencies
    readonly GetProjectData getProjectData;
    readonly NewItemDialogService newItemDialogService;
    readonly CreateProjectData createProjectData;
    #endregion

    #region CommandMethods
    [RelayCommand]
    private async Task AddItem(ExplorerItem itemName)
    {
        var ProjectItem = await getProjectData.GetItemProject(itemName.Path);
        var item = new ProjectEditingTabViewItem()
        {
            itemPath = itemName.Path,
            ProjectItem = ProjectItem
        };

        if (!TabViewItems.Any(item => item.itemPath.Equals(itemName.Path)))
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(() => TabViewItems.Add(item));
        }
    }
    [RelayCommand]
    private async Task AddItemToProject(ExplorerItem obj)
    {
        try
        {
            var result = await newItemDialogService.ShowDialog();

            App.MainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                if (result is not null)
                {
                    _infoBarService.SetMessageData("Item Adicionado", result.Name, InfoBarSeverity.Informational);
                    var item = new ExplorerItem()
                    {
                        Name = result.Name,
                        Path = Path.Combine(obj.Path, $"{result.Name}.prjd"),
                        Type = ExplorerItem.ExplorerItemType.File
                    };

                    obj.Children.Add(item);
                    createProjectData.CreateProjectItem(obj.Path, $"{item.Name}.prjd", result.Path);
                }
            });

        }
        catch (Exception)
        {
            throw;
        }
    }
    [RelayCommand]
    private void AddFolderToProject(ExplorerItem obj)
    {
        if (obj is not null)
        {
            createProjectData.RenameProjectFolder(obj.Path, obj.Name);
        }
    }
    [RelayCommand]
    private void RenameProjectItem(ExplorerItem obj)
    {
        if (obj is not null)
        {
            obj.IsEditing = true;
            if (obj.Type == ExplorerItem.ExplorerItemType.File)
                createProjectData.RenameProjectItem(obj.Path, obj.Name);

            if (obj.Type == ExplorerItem.ExplorerItemType.Folder)
                createProjectData.RenameProjectFolder(obj.Path, obj.Name);
        }
    }
    [RelayCommand]
    private void DeleteProjectItem(ExplorerItem obj)
    {
        if (obj is not null)
        {
           
        }
    }
    #endregion
    readonly InfoBarService _infoBarService;

    #region Constructor
    public ProjectViewModel(GetProjectData getProject, NewItemDialogService newItemDialog, InfoBarService infoBarService, CreateProjectData createProjectData)
    {
        getProjectData = getProject;
        this.createProjectData = createProjectData;
        newItemDialogService = newItemDialog;
        _infoBarService = infoBarService;
    }
    public ProjectViewModel()
    {

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
