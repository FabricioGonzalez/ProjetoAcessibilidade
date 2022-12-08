using ReactiveUI.Fody.Helpers;

namespace AppViewModels.Common;
internal class ViewModelBase<TModel> : ViewModelBase
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
