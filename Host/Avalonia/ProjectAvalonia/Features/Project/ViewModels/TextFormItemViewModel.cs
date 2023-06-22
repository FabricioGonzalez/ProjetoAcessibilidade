using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using Common.Linq;

using Core.Entities.Solution.Project.AppItem;
using Core.Entities.ValidationRules;

using DynamicData;
using DynamicData.Binding;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class TextFormItemViewModel : ReactiveObject, ITextFormItemViewModel
{
    public TextFormItemViewModel(string topic, string textData, string measurementUnit, string id, SourceList<ObservationModel> observations, IEnumerable<ValidationRule> rules)
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
             x.Rules
             .SelectMany(rule => rule.Conditions
             .Where(y => y.TargetId == prop.Sender.Id)
             .Select(z => z.ConditionsFunctions)));

                var ok = rulesToEvaluate.Select(x => x.Invoke(prop.Value ?? ""));

                ok.IterateOn(x =>
                {
                    var exsits = (string it) => observations.Items.Any(observation => observation.ObservationText == it);

                    if (x.evaluationResult)
                    {
                        var item = x.results
                           .Where(it => !exsits(it));

                        observations.AddRange(item
                            .Select(it => new ObservationModel() { Id = Guid.NewGuid().ToString(), ObservationText = it }));

                        this.RaisePropertyChanged();
                    }
                    else
                    {
                        var itemsToRemove = x.results.Where(it => exsits(it));
                        observations.RemoveMany(observations.Items.IntersectBy(itemsToRemove, it => it.ObservationText));
                        this.RaisePropertyChanged();
                    }

                });

            });
    }
    public string Id
    {
        get; set;
    }
    public IEnumerable<ValidationRule> Rules
    {
        get; set;
    }
    public string Topic
    {
        get;
    }
    [AutoNotify] private string _textData;

    public string MeasurementUnit
    {
        get; set;
    }
}