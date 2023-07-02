using System.Collections.ObjectModel;
using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.Presentation.States.LawItemState;
using ProjetoAcessibilidade.Core.Enuns;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeTemplateEditTabViewModel
    : ReactiveObject
        , ITemplateEditTabViewModel
{
    public ReactiveCommand<LawStateItem, Unit> RemoveLawCommand
    {
        get;
    }

    public AppModelState EditingItem
    {
        get;
        set;
    } = new()
    {
        Id = "", FormData = new ObservableCollection<FormItemContainer>
        {
            new()
            {
                Topic = "Teste", Type = AppFormDataType.Texto, Body = new TextItemState(
                    "teste item",
                    id: "",
                    textData: "teste",
                    measurementUnit: "m")
            }
            , new()
            {
                Topic = "Teste 2", Type = AppFormDataType.Checkbox, Body = new CheckboxContainerItemState("Teste")
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
                                new(
                                    "inner text",
                                    id: "",
                                    textData: "teste",
                                    measurementUnit: "m")
                                , new(
                                    "inner text 2",
                                    id: "",
                                    textData: "teste",
                                    measurementUnit: "m")
                            }
                        }
                    }
                }
            }
            , new()
            {
                Topic = "Teste", Type = AppFormDataType.Texto, Body = new TextItemState(
                    "teste item",
                    id: "",
                    textData: "teste2",
                    measurementUnit: "m")
            }
        }
        , ItemName = "Teste", ItemTemplate = "Teste Template", LawItems = new ObservableCollection<LawStateItem>()
    };

    public ObservableCollection<IValidationRuleContainerState> EditingItemRules
    {
        get;
        set;
    }

    public ReactiveCommand<Unit, Unit> AddItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> AddLawCommand
    {
        get;
    }
}