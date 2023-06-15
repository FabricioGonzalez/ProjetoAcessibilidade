using System.Collections.ObjectModel;
using System.Reactive;

using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.Presentation.States.LawItemState;

using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeTemplateEditTabViewModel
    : ReactiveObject,
        ITemplateEditTabViewModel
{
    public AppModelState EditingItem
    {
        get;
        set;
    } = new()
    {
        Id = "",
        FormData = new ObservableCollection<FormItemContainer>
        {
            new()
            {
                Topic = "Teste",Type=ProjetoAcessibilidade.Core.Enuns.AppFormDataType.Texto, Body = new TextItemState(
                    topic: "teste item",
                    id: "",
                    textData: "teste",
                    measurementUnit: "m")
            },
            new()
            {
                Topic = "Teste 2",Type=ProjetoAcessibilidade.Core.Enuns.AppFormDataType.Checkbox, Body = new CheckboxContainerItemState("Teste")
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
                                },
                                new()
                                {
                                    Value = "Não", IsChecked = false
                                },
                                new()
                                {
                                    Value = "Talvez", IsChecked = false
                                }
                            },
                            TextItems = new ObservableCollection<TextItemState>
                            {
                                new(
                                    topic: "inner text",
                                    id: "",
                                    textData: "teste",
                                    measurementUnit: "m"),
                                new(
                                    topic: "inner text 2",
                                    id: "",
                                    textData: "teste",
                                    measurementUnit: "m")
                            }
                        }
                    }
                }
            },
            new()
            {
                Topic = "Teste",Type=ProjetoAcessibilidade.Core.Enuns.AppFormDataType.Texto, Body = new TextItemState(
                    topic: "teste item",
                    id: "",
                    textData: "teste2",
                    measurementUnit: "m")
            }
        },
        ItemName = "Teste",
        ItemTemplate = "Teste Template",
        LawItems = new ObservableCollection<LawStateItem>()
    };

    public ReactiveCommand<Unit, Unit> AddItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> AddLawCommand
    {
        get;
    }
}