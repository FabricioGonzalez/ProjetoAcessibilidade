using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

using App.Core.Entities.App;
using App.Core.Entities.Solution.Explorer;

using AppViewModels.Dialogs;
using AppViewModels.Interactions.Main;
using AppViewModels.Interactions.Project;
using AppViewModels.Main;
using AppViewModels.Project;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using ProjectAvalonia.Project.Components.ProjectExplorer.Components;
using ProjectAvalonia.Project.Components.ProjectExplorer.Dialogs;
using ProjectAvalonia.Views;

using QuestPDF.Previewer;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Project.Components.ProjectExplorer;
public partial class ExplorerComponent : ReactiveUserControl<ProjectExplorerViewModel>
{
    public TreeView ExplorerTree => this.FindControl<TreeView>("explorerTreeView");
    public Button PrintButton => this.FindControl<Button>("AppPrintButton");
    public Button SaveSolution => this.FindControl<Button>("SaveSolutionButton");
    public ToggleButton SolutionButton => this.FindControl<ToggleButton>("SolutionFlyoutButton");

    public ExplorerComponent()
    {
        ViewModel ??= Locator.Current.GetService<ProjectExplorerViewModel>();
        DataContext = ViewModel;

        this.WhenActivated(disposables =>
        {
            ViewModel.PrintDocument = ReactiveCommand.Create<string>(
                execute: (path) =>
            {
                var route = Locator.Current.GetService<PreviewerViewModel>();
                var main = Locator.Current.GetService<MainViewModel>();

                route.HostScreen = main;

                main.Router.Navigate
                .Execute(route.SetSolutionPath(ViewModel.CurrentOpenProject))
                .Subscribe();
            },
            canExecute: ViewModel.IsProjectOpened);

            this.OneWayBind(
                viewModel: ViewModel,
                vmProperty: vm => vm.projectExplorerState.ExplorerItems,
                viewProperty: v => v.ExplorerTree.Items)
            .DisposeWith(compositeDisposable: disposables);

            SolutionButton.Bind(
                property: IsEnabledProperty,
                source: ViewModel.IsProjectOpened)
            .DisposeWith(compositeDisposable: disposables);

            this.Bind(viewModel: ViewModel,
                vmProperty: vm => vm.IsDocumentSolutionEnabled,
                viewProperty: v => v.SolutionButton.IsChecked);

            SaveSolution.Bind(property: IsVisibleProperty,
                source: ViewModel.IsProjectOpened)
            .DisposeWith(compositeDisposable: disposables);

            BindCommands(disposables);
            BindToInteractions(disposables);
            RegisterInteractionHandlers(disposables);
            AddHandlers(disposables);
        });

        AvaloniaXamlLoader.Load(this);
    }
    private void AddHandlers(CompositeDisposable disposables)
    {
        disposables.Add(
            item: Disposable.Create(
            dispose: () =>
        {
            (SolutionButton.Flyout as SolutionFlyout).Closed -= ExplorerComponent_Closed;
        }));

        (SolutionButton.Flyout as SolutionFlyout).Closed += ExplorerComponent_Closed;
    }

    private void ExplorerComponent_Closed(object? sender, EventArgs e) => ViewModel.IsDocumentSolutionEnabled = !ViewModel.IsDocumentSolutionEnabled;

    private void BindCommands(CompositeDisposable disposables)
    {
        this.BindCommand(
            viewModel: ViewModel,
            propertyName: vm => vm.PrintDocument,
            controlName: v => v.PrintButton)
            .DisposeWith(compositeDisposable: disposables);

    }
    private void BindToInteractions(CompositeDisposable disposables)
    {
        this.BindInteraction(
            viewModel: ViewModel,
            propertyName: vm => vm.ShowDialog,
            handler: context => DoShowDialogAsync(context))
            .DisposeWith(compositeDisposable: disposables);
    }
    private void RegisterInteractionHandlers(CompositeDisposable disposables)
    {
        ProjectInteractions
              .SelectedProjectPath
              .RegisterHandler(handler: async interaction =>
              {
                  ViewModel.CurrentOpenProject = interaction.Input;

                  if (!string.IsNullOrEmpty(ViewModel.CurrentOpenProject))
                  {
                      var result = await ViewModel.ReadSolution(ViewModel.CurrentOpenProject);

                      if (result is not null)
                      {
                          ViewModel.SolutionModel.FileName = result.FileName;
                          ViewModel.SolutionModel.FilePath = result.FilePath;
                          ViewModel.SolutionModel.ReportData = result.SolutionReportInfo;
                          ViewModel.SolutionModel.ItemGroups = new(result.ItemGroups is not null ? result.ItemGroups : new());
                          ViewModel.SolutionModel.ParentFolderName = result.ParentFolderName;
                          ViewModel.SolutionModel.ParentFolderPath = result.ParentFolderPath;
                      }
                  }
                  interaction.SetOutput(ViewModel.CurrentOpenProject);

              })
              .DisposeWith(compositeDisposable: disposables);

        ProjectInteractions
        .RenameFileInteraction
        .RegisterHandler(handler: async interaction =>
        {
            var item = await ViewModel
            .explorerOperations
            .RenameFile(interaction.Input,
                ViewModel.projectExplorerState.ExplorerItems
            .ToList());

            interaction.SetOutput(item);

        })
        .DisposeWith(compositeDisposable: disposables);

        ProjectInteractions
       .RenameFolderInteraction
       .RegisterHandler(handler: async interaction =>
       {
           var item = await ViewModel
            .explorerOperations
            .RenameFolder(interaction.Input,
                ViewModel.projectExplorerState.ExplorerItems
            .ToList());

           AppInterations
                 .MessageQueue
                 .Handle(new()
                 {
                     Type = MessageType.Info,
                     Message = $"{item.Title} Foi Renomeado"
                 }).Subscribe();

           interaction.SetOutput(item);

       })
       .DisposeWith(compositeDisposable: disposables);

        ProjectInteractions
       .DeleteFileInteraction
       .RegisterHandler(handler: async interaction =>
       {
           var item = await ViewModel
           .explorerOperations
           .DeleteFile(interaction.Input,
               ViewModel.projectExplorerState.ExplorerItems
           .ToList());

           interaction.SetOutput(item);

       })
       .DisposeWith(compositeDisposable: disposables);

        ProjectInteractions
       .DeleteFolderInteraction
       .RegisterHandler(handler: async interaction =>
       {
           var item = await ViewModel
             .explorerOperations
             .DeleteFolder(interaction.Input,
                 ViewModel.projectExplorerState.ExplorerItems
             .ToList());

           interaction.SetOutput(item);

       })
       .DisposeWith(compositeDisposable: disposables);
    }

    private async Task DoShowDialogAsync(InteractionContext<AddItemViewModel, ExplorerItem?> interaction)
    {
        var dialog = Locator.Current.GetService<AddItemWindow>();

        if (dialog is AddItemWindow dialogVm)
        {
            dialogVm.DataContext = interaction.Input;

            dialogVm.Activate();

            var result = await dialogVm.ShowDialog<ExplorerItem?>(Locator.Current.GetService<MainWindow>());
            interaction.SetOutput(result);
        }
    }


}
