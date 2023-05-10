using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeTemplateEditTabViewModel
    : ReactiveObject
        , ITemplateEditTabViewModel
{
    public AppModelState EditingItem
    {
        get;
        set;
    } = new()
    {
        Id = "", FormData = new ReadOnlyObservableCollection<FormItemStateBase>(
            new ObservableCollection<FormItemStateBase>
            {
                new TextItemState("teste item", id: "", textData: "teste", measurementUnit: "m")
                , new CheckboxContainerItemState("Teste")
                {
                    Children = new ObservableCollection<CheckboxItemState>
                    {
                        new()
                        {
                            Options = new ObservableCollection<OptionsItemState>
                            {
                                new()
                                {
                                    Value = "Sim", IsChecked = false
                                }
                                , new()
                                {
                                    Value = "Não", IsChecked = false
                                }
                                , new()
                                {
                                    Value = "Talvez", IsChecked = false
                                }
                            }
                            , TextItems = new ObservableCollection<TextItemState>
                            {
                                new("inner text", id: "", textData: "teste", measurementUnit: "m")
                                , new("inner text 2", id: "", textData: "teste", measurementUnit: "m")
                            }
                        }
                    }
                }
                , new TextItemState("teste item", id: "", textData: "teste2", measurementUnit: "m")
            })
        , ItemName = "Teste", ItemTemplate = "Teste Template"
        , LawItems = new ReadOnlyObservableCollection<LawStateItem>(new ObservableCollection<LawStateItem>())
    };
}