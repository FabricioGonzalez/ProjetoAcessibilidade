﻿using System.Collections.ObjectModel;
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

using CustomControls;
using Microsoft.UI.Xaml.Controls;
using CustomControls.TabViews;

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
    public ICommand AddItemToProjectCommand => _addItemToProjectCommand ??= new AsyncRelayCommand<ExplorerItem>(OnAddItemToProjectCommand);

    private ICommand _addFolderToProjectCommand;
    public ICommand AddFolderToProjectCommand => _addFolderToProjectCommand ??= new RelayCommand<ExplorerItem>(OnAddFolderToProjectCommand);

    private ICommand _renameProjectItemCommand;
    public ICommand RenameProjectItemCommand => _renameProjectItemCommand ??= new RelayCommand<ExplorerItem>(OnRenameProjectItemCommand);

    private ICommand _textBoxLostFocusCommand;
    public ICommand TextBoxLostFocusCommand => _textBoxLostFocusCommand ??= new AsyncRelayCommand<string>(OnTextFieldLostFocus);

    #endregion

    #region CommandMethods
    private void OnAddItemCommand(ExplorerItem itemName)
    {
        var item = new ProjectEditingTabViewItem()
        {
            itemPath = itemName.Path,
            ProjectItem = getProjectData.GetItemProject(itemName.Path)
        };

        if (!TabViewItems.Any(item => item.itemPath.Equals(itemName.Path)))
        {
            TabViewItems.Add(item);
        }
    }
    private async Task OnAddItemToProjectCommand(ExplorerItem obj)
    {
        try
        {
            var result = await newItemDialogService.ShowDialog();

            App.MainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                if (result is not null)
                {
                    _infoBarService.SetMessageData(result.Path, result.Name, InfoBarSeverity.Warning);
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
    private void OnAddFolderToProjectCommand(ExplorerItem obj)
    {
        if (obj is not null)
        {
            createProjectData.RenameProjectFolder(obj.Path, obj.Name);
        }
    }
    private void OnRenameProjectItemCommand(ExplorerItem obj)
    {
        if (obj is not null)
        {
            if (obj.Type == ExplorerItem.ExplorerItemType.File)
                createProjectData.RenameProjectItem(obj.Path, obj.Name);

            if (obj.Type == ExplorerItem.ExplorerItemType.Folder)
                createProjectData.RenameProjectFolder(obj.Path, obj.Name);
        }
    }
    private async Task OnTextFieldLostFocus(string data)
    {
        await Task.Run(() =>
        {
            Debug.WriteLine(data);
        });
    }
    #endregion
    InfoBarService _infoBarService;
    #region Constructor
    public ProjectViewModel(GetProjectData getProject, NewItemDialogService newItemDialog, InfoBarService infoBarService, CreateProjectData createProjectData)
    {
        getProjectData = getProject;
        this.createProjectData = createProjectData;
        newItemDialogService = newItemDialog;
        _infoBarService = infoBarService;
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
