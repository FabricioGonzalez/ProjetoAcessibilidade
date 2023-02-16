using ReactiveUI.Fody.Helpers;

namespace AppViewModels.Common;
public class ViewModelBase<TModel> : ViewModelBase
{
    [Reactive]
    public TModel Model
    {
        get; set;
    }

    public ViewModelBase(TModel model)
    {
        Model = model;
    }
}
