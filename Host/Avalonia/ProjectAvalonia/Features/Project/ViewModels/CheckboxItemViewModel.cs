using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Core.Entities.ValidationRules;

using DynamicData;
using DynamicData.Binding;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class CheckboxItemViewModel : ReactiveObject, ICheckboxItemViewModel
{
    public CheckboxItemViewModel(string topic, IOptionsContainerViewModel options,
        ObservableCollection<ITextFormItemViewModel> textItems, string id, IEnumerable<ValidationRule> rules)
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
          .AutoRefreshOnObservable(x => x.WhenAnyValue(it => it.IsChecked)))
          .Switch()
          .WhenPropertyChanged(propertyAccessor: prop => prop.IsChecked, notifyOnInitialValue: false)
          .Subscribe(prop =>
          {
              var rulesToEvaluate = Rules
              .SelectMany(x =>
              x.Rules
              .SelectMany(rule => rule.Conditions
              .Where(y => y.TargetId == prop.Sender.Id).Select(z => z.ConditionsFunctions)));
              var ok = rulesToEvaluate.Select(x => x.Invoke(prop.Value ? "checked" : "unchecked"));


          });


    }

    public string Id
    {
        get;
        set;
    }
    public IEnumerable<ValidationRule> Rules
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