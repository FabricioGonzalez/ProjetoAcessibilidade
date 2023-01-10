using App.Core.Entities.Solution.Project.AppItem.DataItems;

using ReactiveUI;

namespace AppViewModels.Project.States.ProjectItemState;
public class AppModelState : ReactiveObject
{
    private string itemName;
    public string ItemName
    {
        get => itemName;
        set => this.RaiseAndSetIfChanged(ref itemName, value);
    }

    private IList<IAppFormDataItemContract> formData;

    /*public SourceList<IAppFormDataItemContract> FormData
    {
        get => new(formData.AsObservableChangeSet());
        set =>
    }*/
}
