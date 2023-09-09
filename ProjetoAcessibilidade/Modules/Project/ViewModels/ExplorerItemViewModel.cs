using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using ProjetoAcessibilidade.Services;

using ReactiveUI;

using SystemApplication.Services.ProjectDataServices;
using SystemApplication.Services.UIOutputs;

namespace ProjetoAcessibilidade.Modules.Project.ViewModels;
public class ExplorerItemViewModel : ReactiveObject
{
    private ExplorerItem explorerItem;
    public ExplorerItem ExplorerItem
    {
        get => explorerItem; set => this.RaiseAndSetIfChanged(ref explorerItem, value);
    }
  
    readonly GetProjectData getProjectData;
    readonly NewItemDialogService newItemDialogService;
    readonly CreateProjectData createProjectData;
    public ICommand AddItemCommand
    {
        get; private set;
    }

    public ICommand AddItemToProjectCommand
    {
        get; private set;
    }
    public ICommand AddFolderToProjectCommand
    {
        get; private set;
    }

    public ICommand RenameProjectItemCommand
    {
        get; private set;
    } 
    public ICommand DeleteProjectItemCommand
    {
        get; private set;
    }

    public ICommand TextBoxLostFocusCommand
    {
        get; private set;
    }

    public ExplorerItemViewModel()
    {

    }
    public ExplorerItemViewModel(ExplorerItem item)
    {
        getProjectData = App.GetService<GetProjectData>();
        createProjectData = App.GetService<CreateProjectData>();
        newItemDialogService = App.GetService<NewItemDialogService>();
        //_infoBarService = App.GetService<InfoBarService>();

        ExplorerItem = item;

        //AddItemCommand = ReactiveCommand.CreateFromTask<ExplorerItem>(param => OnAddItemToProjectCommand(param));
        //AddFolderToProjectCommand = ReactiveCommand.Create<ExplorerItem>(param => OnAddFolderToProjectCommand(param));
        //RenameProjectItemCommand = ReactiveCommand.Create<ExplorerItem>(param => OnRenameProjectItemCommand(param));  
        
        AddItemToProjectCommand = new AsyncRelayCommand<ExplorerItem>(param => OnAddItemToProjectCommand(param));
        AddFolderToProjectCommand =new RelayCommand<ExplorerItem>(param => OnAddFolderToProjectCommand(param));
        RenameProjectItemCommand = new RelayCommand<ExplorerItem>(param => OnRenameProjectItemCommand(param));
        DeleteProjectItemCommand = new RelayCommand<ExplorerItem>(param => OnDeleteProjectItemCommand(param));
    }

    //private async Task OnAddItemCommand(ExplorerItem itemName)
    //{
    //    AddItemCommand = new AsyncRelayCommand<ExplorerItem>(OnAddItemCommand);
    //    AddItemToProjectCommand = new AsyncRelayCommand<ExplorerItem>(OnAddItemToProjectCommand);
    //    AddFolderToProjectCommand = new RelayCommand<ExplorerItem>(OnAddFolderToProjectCommand);
    //    RenameProjectItemCommand = new RelayCommand<ExplorerItem>(OnRenameProjectItemCommand);
    //    //TextBoxLostFocusCommand = new AsyncRelayCommand<string>(OnTextFieldLostFocus);

    //    var ProjectItem = await getProjectData.GetItemProject(itemName.Path);
    //    var item = new ProjectEditingTabViewItem()
    //    {
    //        itemPath = itemName.Path,
    //        ProjectItem = ProjectItem
    //    };

    //    if (!TabViewItems.Any(item => item.itemPath.Equals(itemName.Path)))
    //    {
    //        App.MainWindow.DispatcherQueue.TryEnqueue(() => TabViewItems.Add(item));
    //    }
    //}
    private async Task OnAddItemToProjectCommand(ExplorerItem obj)
    {
        try
        {
            var result = await newItemDialogService.ShowDialog();

            App.MainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                if (result is not null)
                {
                    //_infoBarService.SetMessageData("Item Adicionado", result.Name, InfoBarSeverity.Informational);
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
    private void OnDeleteProjectItemCommand(ExplorerItem obj)
    {
        if (obj is not null)
        {
            if (obj.Type == ExplorerItem.ExplorerItemType.File)
                createProjectData.RenameProjectItem(obj.Path, obj.Name);

            if (obj.Type == ExplorerItem.ExplorerItemType.Folder)
                createProjectData.RenameProjectFolder(obj.Path, obj.Name);
        }
    }
}
