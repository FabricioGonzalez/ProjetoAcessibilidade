using ReactiveUI.Fody.Helpers;

namespace Project.Core.ViewModels.Extensions;
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
