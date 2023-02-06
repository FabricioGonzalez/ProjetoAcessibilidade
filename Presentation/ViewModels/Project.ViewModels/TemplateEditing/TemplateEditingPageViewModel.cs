using System.Reactive.Disposables;

using AppViewModels.Common;

using Common;

using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using Core.Entities.Solution.Project.AppItem.DataItems.Text;
using Core.Enuns;

using DynamicData.Binding;

using Project.Application.Contracts;
using Project.Application.Project.Queries.GetProjectItemContent;

using ReactiveUI;

using Splat;

namespace AppViewModels.TemplateEditing;
public class TemplateEditingPageViewModel : ViewModelBase
{
    private AppItemModel editingItem;

    public AppItemModel EditingItem
    {
        get => editingItem;
        set => this.RaiseAndSetIfChanged(ref editingItem, value);
    }

    public ObservableCollectionExtended<AppFormDataType> Types => new(
        Enum
        .GetValues<AppFormDataType>());

    private readonly IQueryDispatcher queryDispatcher;

    public TemplateEditingPageViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();


        this.WhenActivated((CompositeDisposable disposables) =>
        {

        });
    }

    public async Task SetEditingItem(string path)
    {
        var result = await queryDispatcher.Dispatch<GetProjectItemContentQuery, Resource<AppItemModel>>(new(path), CancellationToken.None);
        if (result is Resource<AppItemModel>.Error err) { }
        if (result is Resource<AppItemModel>.Success success)
        {
            success.Data.FormData = success
                .Data
                .FormData
                .Where(item =>
            {
                if (item is AppFormDataItemCheckboxModel)
                    return true;

                if (item is AppFormDataItemTextModel)
                    return true;

                return false;
            })
                .ToList();

            EditingItem = success.Data;
        }

    }
}
