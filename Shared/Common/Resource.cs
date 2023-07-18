namespace Common;

public abstract class Resource<T>
{
    public sealed class Success : Resource<T>
    {
        public Success(
            T? Data
        )
        {
            this.Data = Data;
        }

        public T? Data
        {
            get;
            set;
        }
    }

    public sealed class Error : Resource<T>
    {
        public Error(
            string? Message
            , T? Data = default
        )
        {
            this.Message = Message;
            this.Data = Data;
        }

        public string? Message
        {
            get;
            set;
        }

        public T? Data
        {
            get;
            set;
        }
    }
}

public static class Extensions
{
    public static Resource<T> OnError<T>(
        this Resource<T> resource
        , out T? Data
        , out string? message
    )
    {
        if (resource is Resource<T>.Error error)
        {
            message = error.Message;

            if (error.Data is null)
            {
                Data = default;
            }
            else
            {
                Data = error.Data;
            }
        }
        else
        {
            message = "";
            Data = default;
        }

        return resource;
    }

    public static Resource<T> OnSuccess<T>(
        this Resource<T> resource
        , out T? Data
    ) where T : class
    {
        if (resource is Resource<T>.Success success)
        {
            Data = success.Data;
        }
        else
        {
            Data = default;
        }

        return resource;
    }


    public static Resource<T> OnError<T>(
        this Resource<T> resource
        , Action<Resource<T>.Error> onErrorAction
    )
    {
        if (resource is Resource<T>.Error error)
        {
            onErrorAction.Invoke(obj: error);
        }

        return resource;
    }

    public static Resource<T> OnSuccess<T>(
        this Resource<T> resource
        , Action<Resource<T>.Success> onSuccessAction
    )
    {
        if (resource is Resource<T>.Success success)
        {
            onSuccessAction.Invoke(obj: success);
        }

        return resource;
    }

    public static (Resource<T> resource, T? value) OnSuccessToTuple<T>(
        this Resource<T> resource
        , Func<Resource<T>.Success, T> onSuccessAction
    )
    {
        if (resource is Resource<T>.Success success)
        {
            return (resource, value: onSuccessAction.Invoke(arg: success));
        }

        return (resource, default);
    }

    public static (Resource<T>.Error resource, T? value) OnErrorToTuple<T>(
        this (Resource<T> resource, T? value) resource
        , Func<Resource<T>.Error, T> onErrorAction
    )
    {
        if (resource.resource is Resource<T>.Error error)
        {
            return (error, value: onErrorAction.Invoke(arg: error));
        }

        return ((Resource<T>.Error resource, T? value))resource;
    }

    public static (Resource<T> resource, T? value) OnError<T>(
        this (Resource<T> resource, T? value) resource
        , Func<Resource<T>.Error, T> onErrorAction
    )
    {
        if (resource.resource is Resource<T>.Error error)
        {
            return (resource.resource, value: onErrorAction.Invoke(arg: error));
        }

        return resource;
    }


    public static (Resource<T> resource, T? value) OnSuccess<T>(
        this (Resource<T> resource, T? value) resource
        , Func<Resource<T>.Success, T> onSuccessAction
    )
    {
        if (resource.resource is Resource<T>.Success success)
        {
            return (resource.resource,
                value: onSuccessAction.Invoke(arg: success));
        }

        return resource with { value = default };
    }
}