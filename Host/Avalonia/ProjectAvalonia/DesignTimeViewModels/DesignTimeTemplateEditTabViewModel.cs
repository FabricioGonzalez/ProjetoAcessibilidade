using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeTemplateEditTabViewModel : ReactiveObject, ITemplateEditTabViewModel
{
    public ReactiveCommand<IEditableItemViewModel, Unit> LoadEditingItem
    {
        get;
    }
}