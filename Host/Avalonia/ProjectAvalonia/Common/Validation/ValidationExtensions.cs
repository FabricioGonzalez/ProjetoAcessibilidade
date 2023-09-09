using System;
using System.Linq.Expressions;
using ProjectAvalonia.Common.Models;

namespace ProjectAvalonia.Common.Validation;

public delegate void ValidateMethod(
    IValidationErrors errors
);

public static class ValidationExtensions
{
    public static void ValidateProperty<TSender, TRet>(
        this TSender viewModel
        , Expression<Func<TSender, TRet>> property
        , ValidateMethod validateMethod
    )
        where TSender : IRegisterValidationMethod
    {
        var propertyName = ((MemberExpression)property.Body).Member.Name;

        viewModel.RegisterValidationMethod(propertyName: propertyName, validateMethod: validateMethod);
    }
}