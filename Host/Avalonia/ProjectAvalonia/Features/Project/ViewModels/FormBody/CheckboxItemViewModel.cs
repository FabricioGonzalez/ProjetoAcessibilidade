using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Common.Linq;

using DynamicData;
using DynamicData.Binding;

using ProjectAvalonia.Models.ValidationTypes;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.FormItemState;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class CheckboxItemViewModel
    : ReactiveObject
        , ICheckboxItemViewModel
{
    [AutoNotify]
    private bool _isInvalid;

    public CheckboxItemViewModel(
        string topic
        , IOptionsContainerViewModel options
        , ObservableCollection<ITextFormItemViewModel> textItems
        , string id
        , SourceList<ObservationState> observations
        , IEnumerable<IValidationRuleContainerState> rules
    )
    {
        Topic = topic;
        Options = options;
        TextItems = textItems;
        Id = id;
        Rules = rules;

        this.WhenAnyValue(vm => vm.Options)
            .Select(op => op.Options)
            .Select(op => op
                .ToObservableChangeSet()
                .AutoRefreshOnObservable(x => x.WhenAnyValue(it => it.IsChecked))
                .DisposeMany())
            .Switch()
            .WhenPropertyChanged(propertyAccessor: prop => prop.IsChecked, notifyOnInitialValue: false)
            .Do(prop =>
            {
                if (prop.Value)
                {
                    return;
                }

                var rulesToEvaluate = Rules
                    .SelectMany(x =>
                        x.ValidaitonRules
                            .SelectMany(rule => rule.Conditions
                                .Where(y => y.TargetId == prop.Sender.Id)
                            ))
                    .Select(it => new Func<string, (bool ResultValue, IEnumerable<string> Results)>(
                        value =>
                        {
                            return value is "checked" or "unchecked"
                                ? (it.CheckingValue.Value == value, it.Result.Select(result => result.ResultValue))
                                : (false, new List<string>());
                        }));

                rulesToEvaluate.IterateOn(x =>
                {
                    var evaluationResult = x.Invoke(prop.Sender.IsChecked ? "checked" : "unchecked");

                    var exsits = (
                        string it
                    ) => observations.Items.Any(observation => observation.Observation == it);

                    if (!evaluationResult.ResultValue)
                    {
                        var item = evaluationResult.Results
                            .Where(it => !exsits(it));

                        item
                            .Select(it => new ObservationState { Observation = it })
                            .IterateOn(obs => observations.Remove(obs));
                    }

                    if (Rules.SelectMany(ruleSet => ruleSet.ValidaitonRules.Where(rule =>
                            rule.Type == AppValidation.GetOperationByValue("Obrigatority"))).Any())
                    {
                        IsInvalid = false;
                        Options
                         .Options.First(it => it.Id == prop.Sender.Id).IsInvalid = false;
                    }
                });
            })
            .Where(it => it.Value)
            .Subscribe(prop =>
            {
                var rulesToEvaluate = Rules
                    .SelectMany(x =>
                        x.ValidaitonRules
                            .SelectMany(rule => rule.Conditions
                                .Where(y => y.TargetId == prop.Sender.Id)
                            )).Select(it => new Func<string, (bool ResultValue, IEnumerable<string> Results)>(value =>
                    {
                        return value is "checked" or "unchecked"
                            ? (it.CheckingValue.Value == value, it.Result.Select(result => result.ResultValue))
                            : (false, new List<string>());
                    }));

                rulesToEvaluate.IterateOn(x =>
                {
                    var evaluationResult = x.Invoke(prop.Sender.IsChecked ? "checked" : "unchecked");

                    var exsits = (
                        string it
                    ) => observations.Items.Any(observation => observation.Observation == it);

                    if (evaluationResult.ResultValue)
                    {
                        var item = evaluationResult.Results
                            .Where(it => !exsits(it));

                        observations.AddRange(item
                            .Select(it => new ObservationState { Observation = it }));
                    }

                    if (Rules.SelectMany(ruleSet =>
                            ruleSet.ValidaitonRules.Where(rule =>
                                rule.Type == AppValidation.GetOperationByValue("Obrigatority"))).Any())
                    {
                        IsInvalid = true;
                        Options
                        .Options.First(it => it.Id == prop.Sender.Id).IsInvalid = true;
                    }
                });

                Options
                    .Options
                    .Chunk(2)
                    .SelectMany(item => item.Where(i => item.Any(x => x.Id == prop.Sender.Id)))
                    .IterateOn(item =>
                    {
                        if (item.Id != prop.Sender.Id)
                        {
                            item.IsChecked = false;
                        }
                    });
            });
    }

    public IEnumerable<IValidationRuleContainerState> Rules
    {
        get;
        set;
    }

    public string Id
    {
        get;
        set;
    }

    public string Topic
    {
        get;
    }

    public IOptionsContainerViewModel Options
    {
        get;
    }

    public ObservableCollection<ITextFormItemViewModel> TextItems
    {
        get;
    }
}