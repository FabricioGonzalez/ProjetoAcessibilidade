using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ProjectAvalonia.Common.Models;
using ReactiveUI;

namespace ProjectAvalonia.Common.Validation;

public class Validations
    : ReactiveObject
        , IRegisterValidationMethod
        , IValidations
{
    public Validations()
    {
        ErrorsByPropertyName = new Dictionary<string, ErrorDescriptors>();
        ValidationMethods = new Dictionary<string, ValidateMethod>();
    }

    private Dictionary<string, ErrorDescriptors> ErrorsByPropertyName
    {
        get;
    }

    private Dictionary<string, ValidateMethod> ValidationMethods
    {
        get;
    }

    void IRegisterValidationMethod.RegisterValidationMethod(
        string propertyName
        , ValidateMethod validateMethod
    )
    {
        if (string.IsNullOrWhiteSpace(value: propertyName))
        {
            throw new ArgumentException(message: "PropertyName must be valid.", paramName: nameof(propertyName));
        }

        ValidationMethods[key: propertyName] = validateMethod;
        ErrorsByPropertyName[key: propertyName] = ErrorDescriptors.Create();
    }

    public bool Any => ErrorsByPropertyName.Any(predicate: x => x.Value.Any());

    public bool AnyErrors => ErrorsByPropertyName.Any(predicate: x =>
        x.Value.Any(predicate: error => error.Severity == ErrorSeverity.Error));

    public bool AnyWarnings => ErrorsByPropertyName.Any(predicate: x =>
        x.Value.Any(predicate: error => error.Severity == ErrorSeverity.Warning));

    public bool AnyInfos => ErrorsByPropertyName.Any(predicate: x =>
        x.Value.Any(predicate: error => error.Severity == ErrorSeverity.Info));

    IEnumerable<string> IValidations.Infos => ErrorsByPropertyName.Values
        .SelectMany(selector: x => x.Where(predicate: error => error.Severity == ErrorSeverity.Info)
            .Select(selector: error => error.Message));

    IEnumerable<string> IValidations.Warnings => ErrorsByPropertyName.Values
        .SelectMany(selector: x => x.Where(predicate: error => error.Severity == ErrorSeverity.Warning)
            .Select(selector: error => error.Message));

    IEnumerable<string> IValidations.Errors => ErrorsByPropertyName.Values
        .SelectMany(selector: x => x.Where(predicate: error => error.Severity == ErrorSeverity.Error)
            .Select(selector: error => error.Message));

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public void Clear()
    {
        foreach (var propertyName in ValidationMethods.Keys)
        {
            ValidateProperty(propertyName: propertyName, clear: true);
            ErrorsByPropertyName[key: propertyName].Clear();
        }
    }

    public void Validate()
    {
        foreach (var propertyName in ValidationMethods.Keys)
        {
            ValidateProperty(propertyName: propertyName);
        }
    }

    public void ValidateProperty(
        string propertyName
        , bool clear = false
    )
    {
        if (ValidationMethods.TryGetValue(key: propertyName, value: out var validationMethod))
        {
            var currentErrors = ErrorsByPropertyName[key: propertyName];

            // Copy the current errors.
            var previousErrors = currentErrors.ToList();

            if (!clear)
            {
                // Validate.
                validationMethod(errors: currentErrors);
            }

            // Clear obsoleted errors and notify properties that changed.
            UpdateAndNotify(currentErrors: currentErrors, previousErrors: previousErrors, propertyName: propertyName);
        }
    }

    public IEnumerable GetErrors(
        string? propertyName
    )
    {
        if (!string.IsNullOrWhiteSpace(value: propertyName))
        {
            return ErrorsByPropertyName.TryGetValue(key: propertyName, value: out var value) && value.Any()
                ? value
                : ErrorDescriptors.Empty;
        }

        return ErrorDescriptors.Empty;
    }

    private void UpdateAndNotify(
        List<ErrorDescriptor> currentErrors
        , List<ErrorDescriptor> previousErrors
        , string propertyName
    )
    {
        // Severities of the new errors.
        var categoriesToNotify = currentErrors.Except(second: previousErrors).Select(selector: x => x.Severity)
            .Distinct().ToList();

        // Remove the old errors.
        previousErrors.ForEach(action: x => currentErrors.Remove(item: x));

        // Severities of the obsoleted errors.
        categoriesToNotify.AddRange(collection: previousErrors.Except(second: currentErrors)
            .Select(selector: x => x.Severity).Distinct().ToList());

        OnErrorsChanged(propertyName: propertyName, categoriesToNotify: categoriesToNotify);
    }

    private void OnErrorsChanged(
        string propertyName
        , List<ErrorSeverity> categoriesToNotify
    )
    {
        Func<ErrorSeverity, string> selector = x => x switch
        {
            ErrorSeverity.Info => nameof(AnyInfos), ErrorSeverity.Warning => nameof(AnyWarnings)
            , ErrorSeverity.Error => nameof(AnyErrors), _ => throw new NotImplementedException()
        };

        var propertiesToNotify = categoriesToNotify.Select(selector: selector).ToList();

        if (propertiesToNotify.Any())
        {
            ErrorsChanged?.Invoke(sender: this, e: new DataErrorsChangedEventArgs(propertyName: propertyName));
            this.RaisePropertyChanged(propertyName: nameof(Any));
        }

        propertiesToNotify.ForEach(action: this.RaisePropertyChanged);
    }
}