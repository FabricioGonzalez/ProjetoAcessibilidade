using AppUsecases.Contracts.Entity;
using AppUsecases.Project.Entities.Project;
using ReactiveUI;

namespace AppWinui.AppCode.Project.States;
public class ProjectItemState: ReactiveObject
{
    private string _itemName;
    public string ItemName
    {
        get => _itemName; 
        set => this.RaiseAndSetIfChanged(ref _itemName,value,nameof(ItemName));
    }
    public IList<IAppFormDataItemContract> FormData
    {
        get; set;
    }
    public IList<AppLawModel> LawList
    {
        get; set;
    }
}
