using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

#if NETFRAMEWORK || NETSTANDARD2_0
using System.Runtime.Serialization;
#else
#endif

namespace AppDI;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/// <summary>
///     Helper code for the various activator services.
/// </summary>
public static class ActivatorUtilities
{
    private static readonly MethodInfo GetServiceInfo =
        GetMethodInfo<Func<IServiceProvider, Type, Type, bool, object?>>(
            (
                sp,
                t,
                r,
                c
            ) => GetService(
                sp,
                t,
                r,
                c));

    /// <summary>
    ///     Instantiate a type with constructor arguments provided directly and/or from an <see cref="IServiceProvider" />.
    /// </summary>
    /// <param name="provider">The service provider used to resolve dependencies</param>
    /// <param name="instanceType">The type to activate</param>
    /// <param name="parameters">Constructor arguments not provided by the <paramref name="provider" />.</param>
    /// <returns>An activated object of type instanceType</returns>
    public static object CreateInstance(
        IServiceProvider provider,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type instanceType,
        params object[] parameters
    )
    {
        if (provider == null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        if (instanceType.IsAbstract)
        {
            throw new InvalidOperationException( /*SR.CannotCreateAbstractClasses*/);
        }

        var serviceProviderIsService = provider.GetService<IServiceProviderIsService>();
        // if container supports using IServiceProviderIsService, we try to find the longest ctor that
        // (a) matches all parameters given to CreateInstance
        // (b) matches the rest of ctor arguments as either a parameter with a default value or as a service registered
        // if no such match is found we fallback to the same logic used by CreateFactory which would only allow creating an
        // instance if all parameters given to CreateInstance only match with a single ctor
        if (serviceProviderIsService != null)
        {
            var bestLength = -1;
            var seenPreferred = false;

            ConstructorMatcher bestMatcher = default;
            var multipleBestLengthFound = false;

            foreach (var constructor in instanceType.GetConstructors())
            {
                var matcher = new ConstructorMatcher(constructor);
                var isPreferred = constructor.IsDefined(
                    attributeType: typeof(ActivatorUtilitiesConstructorAttribute),
                    inherit: false);
                var length = matcher.Match(
                    givenParameters: parameters,
                    serviceProviderIsService: serviceProviderIsService);

                if (isPreferred)
                {
                    if (seenPreferred)
                    {
                        ThrowMultipleCtorsMarkedWithAttributeException();
                    }

                    if (length == -1)
                    {
                        ThrowMarkedCtorDoesNotTakeAllProvidedArguments();
                    }
                }

                if (isPreferred || bestLength < length)
                {
                    bestLength = length;
                    bestMatcher = matcher;
                    multipleBestLengthFound = false;
                }
                else if (bestLength == length)
                {
                    multipleBestLengthFound = true;
                }

                seenPreferred |= isPreferred;
            }

            if (bestLength != -1)
            {
                if (multipleBestLengthFound)
                {
                }

                return bestMatcher.CreateInstance(provider);
            }
        }

        Type?[] argumentTypes = new Type[parameters.Length];
        for (var i = 0;
             i < argumentTypes.Length;
             i++)
        {
            argumentTypes[i] = parameters[i]
                ?.GetType();
        }

        FindApplicableConstructor(
            instanceType: instanceType,
            argumentTypes: argumentTypes,
            matchingConstructor: out var constructorInfo,
            matchingParameterMap: out var parameterMap);
        var constructorMatcher = new ConstructorMatcher(constructorInfo);
        constructorMatcher.MapParameters(
            parameterMap: parameterMap,
            givenParameters: parameters);
        return constructorMatcher.CreateInstance(provider);
    }

    /// <summary>
    ///     Create a delegate that will instantiate a type with constructor arguments provided directly
    ///     and/or from an <see cref="IServiceProvider" />.
    /// </summary>
    /// <param name="instanceType">The type to activate</param>
    /// <param name="argumentTypes">
    ///     The types of objects, in order, that will be passed to the returned function as its second parameter
    /// </param>
    /// <returns>
    ///     A factory that will instantiate instanceType using an <see cref="IServiceProvider" />
    ///     and an argument array containing objects matching the types defined in argumentTypes
    /// </returns>
    public static ObjectFactory CreateFactory(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type instanceType,
        Type[] argumentTypes
    )
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP
        if (!RuntimeFeature.IsDynamicCodeCompiled)
        {
            // Create a reflection-based factory when dynamic code is not compiled\jitted as would be the case with
            // NativeAOT, iOS or WASM.
            // For NativeAOT and iOS, using the reflection-based factory is faster than reflection-fallback interpreted
            // expressions and also doesn't pull in the large System.Linq.Expressions dependency.
            // For WASM, although it has the ability to use expressions (with dynamic code) and interpet the dynamic code
            // efficiently, the size savings of not using System.Linq.Expressions is more important than CPU perf.
            return CreateFactoryReflection(
                instanceType: instanceType,
                argumentTypes: argumentTypes);
        }
#endif

        CreateFactoryInternal(
            instanceType: instanceType,
            argumentTypes: argumentTypes,
            provider: out var provider,
            argumentArray: out var argumentArray,
            factoryExpressionBody: out var factoryExpressionBody);

        var factoryLambda = Expression.Lambda<Func<IServiceProvider, object?[]?, object>>(
            body: factoryExpressionBody,
            provider,
            argumentArray);

        var result = factoryLambda.Compile();
        return result.Invoke;
    }

    /// <summary>
    ///     Create a delegate that will instantiate a type with constructor arguments provided directly
    ///     and/or from an <see cref="IServiceProvider" />.
    /// </summary>
    /// <typeparam name="T">The type to activate</typeparam>
    /// <param name="argumentTypes">
    ///     The types of objects, in order, that will be passed to the returned function as its second parameter
    /// </param>
    /// <returns>
    ///     A factory that will instantiate type T using an <see cref="IServiceProvider" />
    ///     and an argument array containing objects matching the types defined in argumentTypes
    /// </returns>
    /*public static ObjectFactory<T>
        CreateFactory<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
            Type[] argumentTypes
        )
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP
        if (!RuntimeFeature.IsDynamicCodeCompiled)
        {
            // See the comment above in the non-generic CreateFactory() for why we use 'IsDynamicCodeCompiled' here.
            var factory = CreateFactoryReflection(
                instanceType: typeof(T),
                argumentTypes: argumentTypes);
            return (
                serviceProvider,
                arguments
            ) => (T)factory(
                serviceProvider: serviceProvider,
                arguments: arguments);
        }
#endif

        CreateFactoryInternal(
            instanceType: typeof(T),
            argumentTypes: argumentTypes,
            provider: out var provider,
            argumentArray: out var argumentArray,
            factoryExpressionBody: out var factoryExpressionBody);

        var factoryLambda = Expression.Lambda<Func<IServiceProvider, object?[]?, T>>(
            body: factoryExpressionBody,
            provider,
            argumentArray);

        var result = factoryLambda.Compile();
        return result.Invoke;
    }*/
    private static void CreateFactoryInternal(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type instanceType,
        Type[] argumentTypes,
        out ParameterExpression provider,
        out ParameterExpression argumentArray,
        out Expression factoryExpressionBody
    )
    {
        FindApplicableConstructor(
            instanceType: instanceType,
            argumentTypes: argumentTypes,
            matchingConstructor: out var constructor,
            matchingParameterMap: out var parameterMap);

        provider = Expression.Parameter(
            type: typeof(IServiceProvider),
            name: "provider");
        argumentArray = Expression.Parameter(
            type: typeof(object[]),
            name: "argumentArray");
        factoryExpressionBody = BuildFactoryExpression(
            constructor: constructor,
            parameterMap: parameterMap,
            serviceProvider: provider,
            factoryArgumentArray: argumentArray);
    }

    /// <summary>
    ///     Instantiate a type with constructor arguments provided directly and/or from an <see cref="IServiceProvider" />.
    /// </summary>
    /// <typeparam name="T">The type to activate</typeparam>
    /// <param name="provider">The service provider used to resolve dependencies</param>
    /// <param name="parameters">Constructor arguments not provided by the <paramref name="provider" />.</param>
    /// <returns>An activated object of type T</returns>
    public static T CreateInstance<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        T>(
        IServiceProvider provider,
        params object[] parameters
    ) => (T)CreateInstance(
        provider: provider,
        instanceType: typeof(T),
        parameters: parameters);

    /// <summary>
    ///     Retrieve an instance of the given type from the service provider. If one is not found then instantiate it directly.
    /// </summary>
    /// <typeparam name="T">The type of the service</typeparam>
    /// <param name="provider">The service provider used to resolve dependencies</param>
    /// <returns>The resolved service or created instance</returns>
    public static T GetServiceOrCreateInstance<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        T>(
        IServiceProvider provider
    ) => (T)GetServiceOrCreateInstance(
        provider: provider,
        type: typeof(T));

    /// <summary>
    ///     Retrieve an instance of the given type from the service provider. If one is not found then instantiate it directly.
    /// </summary>
    /// <param name="provider">The service provider</param>
    /// <param name="type">The type of the service</param>
    /// <returns>The resolved service or created instance</returns>
    public static object GetServiceOrCreateInstance(
        IServiceProvider provider,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type type
    ) =>
        provider.GetService(type) ??
        CreateInstance(
            provider: provider,
            instanceType: type);

    private static MethodInfo GetMethodInfo<T>(
        Expression<T> expr
    )
    {
        var mc = (MethodCallExpression)expr.Body;
        return mc.Method;
    }

    private static object? GetService(
        IServiceProvider sp,
        Type type,
        Type requiredBy,
        bool isDefaultParameterRequired
    )
    {
        var service = sp.GetService(type);
        if (service == null &&
            !isDefaultParameterRequired)
        {
            /*throw new InvalidOperationException(
                SR.Format(
                    SR.UnableToResolveService,
                    type,
                    requiredBy));*/
        }

        return service;
    }

    private static BlockExpression BuildFactoryExpression(
        ConstructorInfo constructor,
        int?[] parameterMap,
        Expression serviceProvider,
        Expression factoryArgumentArray
    )
    {
        var constructorParameters = constructor.GetParameters();
        var constructorArguments = new Expression[constructorParameters.Length];

        for (var i = 0;
             i < constructorParameters.Length;
             i++)
        {
            var constructorParameter = constructorParameters[i];
            var parameterType = constructorParameter.ParameterType;
            var hasDefaultValue = ParameterDefaultValue.TryGetDefaultValue(
                parameter: constructorParameter,
                defaultValue: out var defaultValue);

            if (parameterMap[i] != null)
            {
                constructorArguments[i] = Expression.ArrayAccess(
                    array: factoryArgumentArray,
                    Expression.Constant(parameterMap[i]));
            }
            else
            {
                var parameterTypeExpression = new[]
                {
                    serviceProvider,
                    Expression.Constant(
                        value: parameterType,
                        type: typeof(Type)),
                    Expression.Constant(
                        value: constructor.DeclaringType,
                        type: typeof(Type)),
                    Expression.Constant(hasDefaultValue)
                };
                constructorArguments[i] = Expression.Call(
                    method: GetServiceInfo,
                    arguments: parameterTypeExpression);
            }

            // Support optional constructor arguments by passing in the default value
            // when the argument would otherwise be null.
            if (hasDefaultValue)
            {
                var defaultValueExpression = Expression.Constant(defaultValue);
                constructorArguments[i] = Expression.Coalesce(
                    left: constructorArguments[i],
                    right: defaultValueExpression);
            }

            constructorArguments[i] = Expression.Convert(
                expression: constructorArguments[i],
                type: parameterType);
        }

        return Expression.Block(
            arg0: Expression.IfThen(
                test: Expression.Equal(
                    left: serviceProvider,
                    right: Expression.Constant(null)),
                ifTrue: Expression.Throw(Expression.Constant(new ArgumentNullException(nameof(serviceProvider))))),
            arg1: Expression.New(
                constructor: constructor,
                arguments: constructorArguments));
    }

    private static void FindApplicableConstructor(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type instanceType,
        Type?[] argumentTypes,
        out ConstructorInfo matchingConstructor,
        out int?[] matchingParameterMap
    )
    {
        ConstructorInfo? constructorInfo = null;
        int?[]? parameterMap = null;

        if (!TryFindPreferredConstructor(
                instanceType: instanceType,
                argumentTypes: argumentTypes,
                matchingConstructor: ref constructorInfo,
                parameterMap: ref parameterMap) &&
            !TryFindMatchingConstructor(
                instanceType: instanceType,
                argumentTypes: argumentTypes,
                matchingConstructor: ref constructorInfo,
                parameterMap: ref parameterMap))
        {
            /*throw new InvalidOperationException(
                SR.Format(
                    SR.CtorNotLocated,
                    instanceType));*/
        }

        matchingConstructor = constructorInfo;
        matchingParameterMap = parameterMap;
    }

    // Tries to find constructor based on provided argument types
    private static bool TryFindMatchingConstructor(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type instanceType,
        Type?[] argumentTypes,
        [NotNullWhen(true)] ref ConstructorInfo? matchingConstructor,
        [NotNullWhen(true)] ref int?[]? parameterMap
    )
    {
        foreach (var constructor in instanceType.GetConstructors())
        {
            if (TryCreateParameterMap(
                    constructorParameters: constructor.GetParameters(),
                    argumentTypes: argumentTypes,
                    parameterMap: out var tempParameterMap))
            {
                if (matchingConstructor != null)
                {
                    /*throw new InvalidOperationException(
                        SR.Format(
                            SR.MultipleCtorsFound,
                            instanceType));*/
                }

                matchingConstructor = constructor;
                parameterMap = tempParameterMap;
            }
        }

        if (matchingConstructor != null)
        {
            Debug.Assert(parameterMap != null);
            return true;
        }

        return false;
    }

    // Tries to find constructor marked with ActivatorUtilitiesConstructorAttribute
    private static bool TryFindPreferredConstructor(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type instanceType,
        Type?[] argumentTypes,
        [NotNullWhen(true)] ref ConstructorInfo? matchingConstructor,
        [NotNullWhen(true)] ref int?[]? parameterMap
    )
    {
        var seenPreferred = false;
        foreach (var constructor in instanceType.GetConstructors())
        {
            if (constructor.IsDefined(
                    attributeType: typeof(ActivatorUtilitiesConstructorAttribute),
                    inherit: false))
            {
                if (seenPreferred)
                {
                    ThrowMultipleCtorsMarkedWithAttributeException();
                }

                if (!TryCreateParameterMap(
                        constructorParameters: constructor.GetParameters(),
                        argumentTypes: argumentTypes,
                        parameterMap: out var tempParameterMap))
                {
                    ThrowMarkedCtorDoesNotTakeAllProvidedArguments();
                }

                matchingConstructor = constructor;
                parameterMap = tempParameterMap;
                seenPreferred = true;
            }
        }

        if (matchingConstructor != null)
        {
            Debug.Assert(parameterMap != null);
            return true;
        }

        return false;
    }

    // Creates an injective parameterMap from givenParameterTypes to assignable constructorParameters.
    // Returns true if each given parameter type is assignable to a unique; otherwise, false.
    private static bool TryCreateParameterMap(
        ParameterInfo[] constructorParameters,
        Type?[] argumentTypes,
        out int?[] parameterMap
    )
    {
        parameterMap = new int?[constructorParameters.Length];

        for (var i = 0;
             i < argumentTypes.Length;
             i++)
        {
            var foundMatch = false;
            var givenParameter = argumentTypes[i];

            for (var j = 0;
                 j < constructorParameters.Length;
                 j++)
            {
                if (parameterMap[j] != null)
                {
                    // This ctor parameter has already been matched
                    continue;
                }

                if (constructorParameters[j]
                    .ParameterType.IsAssignableFrom(givenParameter))
                {
                    foundMatch = true;
                    parameterMap[j] = i;
                    break;
                }
            }

            if (!foundMatch)
            {
                return false;
            }
        }

        return true;
    }

    private static void ThrowMultipleCtorsMarkedWithAttributeException() => throw new InvalidOperationException(

        /*SR.Format(
            SR.MultipleCtorsMarkedWithAttribute,
            nameof(ActivatorUtilitiesConstructorAttribute))*/);

    private static void ThrowMarkedCtorDoesNotTakeAllProvidedArguments() => throw new InvalidOperationException(

        /*SR.Format(
            SR.MarkedCtorMissingArgumentTypes,
            nameof(ActivatorUtilitiesConstructorAttribute))*/);

    private readonly struct ConstructorMatcher
    {
        private readonly ConstructorInfo _constructor;
        private readonly ParameterInfo[] _parameters;
        private readonly object?[] _parameterValues;

        public ConstructorMatcher(
            ConstructorInfo constructor
        )
        {
            _constructor = constructor;
            _parameters = _constructor.GetParameters();
            _parameterValues = new object?[_parameters.Length];
        }

        public int Match(
            object[] givenParameters,
            IServiceProviderIsService serviceProviderIsService
        )
        {
            for (var givenIndex = 0;
                 givenIndex < givenParameters.Length;
                 givenIndex++)
            {
                var givenType = givenParameters[givenIndex]
                    ?.GetType();
                var givenMatched = false;

                for (var applyIndex = 0;
                     applyIndex < _parameters.Length;
                     applyIndex++)
                {
                    if (_parameterValues[applyIndex] == null &&
                        _parameters[applyIndex]
                            .ParameterType.IsAssignableFrom(givenType))
                    {
                        givenMatched = true;
                        _parameterValues[applyIndex] = givenParameters[givenIndex];
                        break;
                    }
                }

                if (!givenMatched)
                {
                    return -1;
                }
            }

            // confirms the rest of ctor arguments match either as a parameter with a default value or as a service registered
            for (var i = 0;
                 i < _parameters.Length;
                 i++)
            {
                if (_parameterValues[i] == null &&
                    !serviceProviderIsService.IsService(
                        _parameters[i]
                            .ParameterType))
                {
                    if (ParameterDefaultValue.TryGetDefaultValue(
                            parameter: _parameters[i],
                            defaultValue: out var defaultValue))
                    {
                        _parameterValues[i] = defaultValue;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }

            return _parameters.Length;
        }

        public object CreateInstance(
            IServiceProvider provider
        )
        {
            for (var index = 0;
                 index != _parameters.Length;
                 index++)
            {
                if (_parameterValues[index] == null)
                {
                    var value = provider.GetService(
                        _parameters[index]
                            .ParameterType);
                    if (value == null)
                    {
                        if (!ParameterDefaultValue.TryGetDefaultValue(
                                parameter: _parameters[index],
                                defaultValue: out var defaultValue))
                        {
                            throw new InvalidOperationException(

                                /*SR.Format(
                                    SR.UnableToResolveService,
                                    _parameters[index]
                                        .ParameterType,
                                    _constructor.DeclaringType)*/);
                        }

                        _parameterValues[index] = defaultValue;
                    }
                    else
                    {
                        _parameterValues[index] = value;
                    }
                }
            }

#if NETFRAMEWORK || NETSTANDARD2_0
                try
                {
                    return _constructor.Invoke(_parameterValues);
                }
                catch (TargetInvocationException ex) when (ex.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    // The above line will always throw, but the compiler requires we throw explicitly.
                    throw;
                }
#else
            return _constructor.Invoke(
                invokeAttr: BindingFlags.DoNotWrapExceptions,
                binder: null,
                parameters: _parameterValues,
                culture: null);
#endif
        }

        public void MapParameters(
            int?[] parameterMap,
            object[] givenParameters
        )
        {
            for (var i = 0;
                 i < _parameters.Length;
                 i++)
            {
                if (parameterMap[i] != null)
                {
                    _parameterValues[i] = givenParameters[(int)parameterMap[i]!];
                }
            }
        }
    }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP
    private static ObjectFactory CreateFactoryReflection(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type instanceType,
        Type?[] argumentTypes
    )
    {
        FindApplicableConstructor(
            instanceType: instanceType,
            argumentTypes: argumentTypes,
            matchingConstructor: out var constructor,
            matchingParameterMap: out var parameterMap);

        var constructorParameters = constructor.GetParameters();
        if (constructorParameters.Length == 0)
        {
            return (
                    serviceProvider,
                    arguments
                ) =>
                constructor.Invoke(
                    invokeAttr: BindingFlags.DoNotWrapExceptions,
                    binder: null,
                    parameters: null,
                    culture: null);
        }

        var parameters = new FactoryParameterContext[constructorParameters.Length];
        for (var i = 0;
             i < constructorParameters.Length;
             i++)
        {
            var constructorParameter = constructorParameters[i];
            var hasDefaultValue = ParameterDefaultValue.TryGetDefaultValue(
                parameter: constructorParameter,
                defaultValue: out var defaultValue);

            parameters[i] = new FactoryParameterContext(
                parameterType: constructorParameter.ParameterType,
                hasDefaultValue: hasDefaultValue,
                defaultValue: defaultValue,
                argumentIndex: parameterMap[i] ?? -1);
        }

        var declaringType = constructor.DeclaringType!;

        return (
            serviceProvider,
            arguments
        ) =>
        {
            if (serviceProvider is null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var constructorArguments = new object?[parameters.Length];
            for (var i = 0;
                 i < parameters.Length;
                 i++)
            {
                ref var parameter = ref parameters[i];
                constructorArguments[i] = (parameter.ArgumentIndex != -1
                                              // Throws an NullReferenceException if arguments is null. Consistent with expression-based factory.
                                              ? arguments![parameter.ArgumentIndex]
                                              : GetService(
                                                  sp: serviceProvider,
                                                  type: parameter.ParameterType,
                                                  requiredBy: declaringType,
                                                  isDefaultParameterRequired: parameter.HasDefaultValue)) ??
                                          parameter.DefaultValue;
            }

            return constructor.Invoke(
                invokeAttr: BindingFlags.DoNotWrapExceptions,
                binder: null,
                parameters: constructorArguments,
                culture: null);
        };
    }

    private readonly struct FactoryParameterContext
    {
        public FactoryParameterContext(
            Type parameterType,
            bool hasDefaultValue,
            object? defaultValue,
            int argumentIndex
        )
        {
            ParameterType = parameterType;
            HasDefaultValue = hasDefaultValue;
            DefaultValue = defaultValue;
            ArgumentIndex = argumentIndex;
        }

        public Type ParameterType
        {
            get;
        }

        public bool HasDefaultValue
        {
            get;
        }

        public object? DefaultValue
        {
            get;
        }

        public int ArgumentIndex
        {
            get;
        }
    }
#endif
}

internal static class ParameterDefaultValue
{
    public static bool TryGetDefaultValue(
        ParameterInfo parameter,
        out object? defaultValue
    )
    {
        var hasDefaultValue = CheckHasDefaultValue(
            parameter: parameter,
            tryToGetDefaultValue: out var tryToGetDefaultValue);
        defaultValue = null;

        if (hasDefaultValue)
        {
            if (tryToGetDefaultValue)
            {
                defaultValue = parameter.DefaultValue;
            }

            var isNullableParameterType = parameter.ParameterType.IsGenericType &&
                                          parameter.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>);

            // Workaround for https://github.com/dotnet/runtime/issues/18599
            if (defaultValue == null &&
                parameter.ParameterType.IsValueType &&
                !isNullableParameterType) // Nullable types should be left null
            {
                defaultValue = CreateValueType(parameter.ParameterType);
            }

            [UnconditionalSuppressMessage(
                category: "ReflectionAnalysis",
                checkId: "IL2067:UnrecognizedReflectionPattern",
                Justification =
                    "CreateValueType is only called on a ValueType. You can always create an instance of a ValueType.")]
            static object? CreateValueType(
                Type t
            ) =>
#if NETFRAMEWORK || NETSTANDARD2_0
                    FormatterServices.GetUninitializedObject(t);
#else
                RuntimeHelpers.GetUninitializedObject(t);
#endif

            // Handle nullable enums
            if (defaultValue != null && isNullableParameterType)
            {
                var underlyingType = Nullable.GetUnderlyingType(parameter.ParameterType);
                if (underlyingType != null &&
                    underlyingType.IsEnum)
                {
                    defaultValue = Enum.ToObject(
                        enumType: underlyingType,
                        value: defaultValue);
                }
            }
        }

        return hasDefaultValue;
    }

    public static bool CheckHasDefaultValue(
        ParameterInfo parameter,
        out bool tryToGetDefaultValue
    )
    {
        tryToGetDefaultValue = true;
        try
        {
            return parameter.HasDefaultValue;
        }
        catch (FormatException) when (parameter.ParameterType == typeof(DateTime))
        {
            // Workaround for https://github.com/dotnet/runtime/issues/18844
            // If HasDefaultValue throws FormatException for DateTime
            // we expect it to have default value
            tryToGetDefaultValue = false;
            return true;
        }
    }
}