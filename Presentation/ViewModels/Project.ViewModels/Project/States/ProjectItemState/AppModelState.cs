using AppViewModels.Project.States.ProjectItemState.LawItemState;

using DynamicData.Binding;

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

    private ObservableCollectionExtended<ReactiveObject> formData;
    public ObservableCollectionExtended<ReactiveObject> FormData
    {
        get => formData;
        set => this.RaiseAndSetIfChanged(ref formData, value);
    }

    private ObservableCollectionExtended<LawStateItem> lawItems;
    public ObservableCollectionExtended<LawStateItem> LawItems
    {
        get => lawItems;
        set => this.RaiseAndSetIfChanged(ref lawItems, value);
    }

}
