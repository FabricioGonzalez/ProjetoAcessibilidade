using AppUsecases.Project.Entities.Project;

using AppViewModels.Common;

using DynamicData.Binding;

using ReactiveUI;

namespace AppViewModels.Dialogs.States;
public class SolutionStateViewModel : ViewModelBase
{
    private ReportDataModel reportData = new();
    public ReportDataModel ReportData
    {
        get => reportData;
        set => this.RaiseAndSetIfChanged(ref reportData, value, nameof(ReportData));
    }

    private string fileName = "";
    public string FileName
    {
        get => fileName;
        set => this.RaiseAndSetIfChanged(ref fileName, value, nameof(FileName));
    }

    private string filePath = "";
    public string FilePath
    {
        get => filePath;
        set => this.RaiseAndSetIfChanged(ref filePath, value, nameof(FilePath));
    }

    private string parentFolderName = "";
    public string ParentFolderName
    {
        get => parentFolderName;
        set => this.RaiseAndSetIfChanged(ref parentFolderName, value, nameof(ParentFolderName));
    }

    private string parentFolderPath = "";
    public string ParentFolderPath
    {
        get => parentFolderPath;
        set => this.RaiseAndSetIfChanged(ref parentFolderPath, value, nameof(ParentFolderPath));
    }

    private ObservableCollectionExtended<ItemGroupModel> itemGroups = new();
    public ObservableCollectionExtended<ItemGroupModel> ItemGroups
    {
        get => itemGroups;
        set => this.RaiseAndSetIfChanged(ref itemGroups, value, nameof(ItemGroups));
    }
}
