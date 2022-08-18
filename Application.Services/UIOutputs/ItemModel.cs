using Core.Contracts;

namespace SystemApplication.Services.UIOutputs;

public class ItemModel : NotifierBaseClass
{
    public string ItemName
    {
        get; set;
    }

    private bool isEditing = false;
    public bool IsEditing
    {
        get => isEditing;
        set => SetAtributeValue(ref isEditing, value, nameof(IsEditing));
    }

    public List<IFormDataItemContract> FormData
    {
        get; set;
    }

    public List<LawModel> LawList
    {
        get; set;
    }
}
