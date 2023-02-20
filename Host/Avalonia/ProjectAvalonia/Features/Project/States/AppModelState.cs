
using DynamicData.Binding;

using ProjectAvalonia.Features.Project.States.LawItemState;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States;
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
