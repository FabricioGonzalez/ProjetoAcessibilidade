using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
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
        , SourceList<ObservationState> observations
        , ObservableCollection<ITextFormItemViewModel> textItems
        , string id
        , IEnumerable<ValidationRuleContainerState> rules
    )
    {
        Topic = topic;
        Options = options;
        TextItems = textItems;
        Id = id;
        Rules = rules;

        /*this.WhenAnyValue(vm => vm.Options)
          .Select(op => op.Options)
          .Select(op => op
          .ToObservableChangeSet()
          .AutoRefreshOnObservable(x => x.WhenAnyValue(it => it.IsChecked))
          .DisposeMany())
          .Switch()
          .WhenPropertyChanged(propertyAccessor: prop => prop.IsChecked/*, notifyOnInitialValue: false#1#)
          .Do(prop =>
          {
              if (prop.Value == false)
              {
                  var rulesToEvaluate = Rules
             .SelectMany(x =>
             x.Rules
             .SelectMany(rule => rule.Conditions
             .Where(y => y.TargetId == prop.Sender.Id)
            ));


                  var ok = rulesToEvaluate
                  .Select(z => z.ConditionsFunctions)
                  .Select(x => x.Invoke(prop.Value ? "checked" : "unchecked"));

                  ok.IterateOn(x =>
                  {
                      var exsits = (string it) => observations.Items.Any(observation => observation.ObservationText == it);

                      var itemsToRemove = x.results.Where(it => exsits(it));
                      observations.RemoveMany(observations.Items.IntersectBy(itemsToRemove, it => it.ObservationText));
                      this.RaisePropertyChanged();

                      if (Rules.SelectMany(x => x.Rules.Where(x => x.Operation == "Obrigatority")).Count() > 0)
                      {
                          IsInvalid = false;
                      }
                  });
              }
          })
          .Where(it => it.Value)
          .Subscribe(prop =>
          {
              var rulesToEvaluate = Rules
              .SelectMany(x =>
              x.Rules
              .SelectMany(rule => rule.Conditions
              .Where(y => y.TargetId == prop.Sender.Id)
             ));

              var ok = rulesToEvaluate
              .Select(z => z.ConditionsFunctions)
              .Select(x => x.Invoke(prop.Value ? "checked" : "unchecked"));

              ok.IterateOn(x =>
              {
                  var exsits = (string it) => observations.Items.Any(observation => observation.ObservationText == it);

                  if (x.evaluationResult)
                  {
                      var item = x.results
                         .Where(it => !exsits(it));

                      observations.AddRange(item
                          .Select(it => new ObservationModel() { Id = Guid.NewGuid().ToString(), ObservationText = it }));
                  }
                  if (Rules.SelectMany(x => x.Rules.Where(x => x.Operation == "Obrigatority")).Count() > 0)
                  {
                      IsInvalid = true;
                  }

              });

              Options
              .Options
              .Chunk(2)
              .SelectMany(item => item.Where((i) => item.Any(x => x.Id == prop.Sender.Id)))
              .IterateOn(item =>
              {
                  if (item.Id != prop.Sender.Id)
                  {
                      item.IsChecked = false;
                  }
              });


          });*/
    }

    public IEnumerable<ValidationRuleContainerState> Rules
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