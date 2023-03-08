using AppViewModels.Project.States.ProjectItemState.LawItemState;

using DynamicData.Binding;

using ReactiveUI;

namespace AppViewModels.Project.States.ProjectItemState;
public class AppModelState : ReactiveObject
{
    private string id;
    public string Id
    {
        get => id;
        set => this.RaiseAndSetIfChanged(ref id, value);
    }

    private string templateName;
    public string TemplateName
    {
        get => templateName;
        set => this.RaiseAndSetIfChanged(ref templateName, value);
    }

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
