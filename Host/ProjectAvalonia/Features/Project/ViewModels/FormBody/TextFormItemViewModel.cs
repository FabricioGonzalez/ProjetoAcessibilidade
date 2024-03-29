﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Common.Linq;
using DynamicData;
using DynamicData.Binding;
using ProjectAvalonia.Models.ValidationTypes;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.FormItemState;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class TextFormItemViewModel
    : ReactiveObject
        , ITextFormItemViewModel
{
    [AutoNotify] private string _textData;

    public TextFormItemViewModel(
        string topic
        , string textData
        , string measurementUnit
        , string id
        , SourceList<ObservationState> observations
        , IEnumerable<IValidationRuleContainerState> rules
    )
    {
        Topic = topic;
        TextData = textData;
        MeasurementUnit = measurementUnit;
        Id = id;
        Rules = rules;

        this.WhenPropertyChanged(vm => vm.TextData)
            .Subscribe(prop =>
            {
                var rulesToEvaluate = Rules
                    .SelectMany(x =>
                        x.ValidaitonRules
                            .SelectMany(rule => rule.Conditions
                                .Where(y => y.TargetId == prop.Sender.Id)
                            )).Select(it => new Func<string?, (bool ResultValue, IEnumerable<string>Results)>(value =>
                    {
                        double.TryParse(s: it.CheckingValue.Value, provider: CultureInfo.CurrentUICulture
                            , result: out var data);
                        double.TryParse(s: value ?? "0", provider: CultureInfo.CurrentUICulture
                            , result: out var dataToEval);

                        return (it.CheckingOperationType switch
                        {
                            HasOperation => it.CheckingValue.Value.Contains(value ?? "")
                            , LessOperation => dataToEval < data
                            , LessThanOperation =>
                                dataToEval <= data
                            , GreaterOperation =>
                                dataToEval > data
                            , GreaterThanOperation =>
                                dataToEval >= data
                            , _ => false
                        }, it.Result.Select(result => result.ResultValue));
                    }));

                rulesToEvaluate.IterateOn(x =>
                {
                    var evaluationResult = x.Invoke(prop.Sender.TextData);

                    var exsits = (
                        string it
                    ) => observations.Items.Any(observation => observation.Observation == it);

                    if (evaluationResult.ResultValue)
                    {
                        var item = evaluationResult.Results
                            .Where(it => !exsits(it));

                        observations.AddRange(item
                            .Select(it => new ObservationState { Observation = it }));

                        this.RaisePropertyChanged("Observation");
                    }
                    else
                    {
                        var itemsToRemove = evaluationResult.Results.Where(it => exsits(it));
                        observations.RemoveMany(observations.Items.IntersectBy(second: itemsToRemove
                            , keySelector: it => it.Observation));
                        this.RaisePropertyChanged("Observation");
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

    public string MeasurementUnit
    {
        get;
        set;
    }
}