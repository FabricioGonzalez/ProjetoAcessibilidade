namespace Common;
public abstract class Resource<T>
{
    public sealed class Success : Resource<T>
    {
        public T? Data
        {
            get; set;
        } = default(T?);
        public Success(T? Data)
        {
            this.Data = Data;
        }

    }
    public sealed class Error : Resource<T>
    {
        public string? Message { get; set; } = null;
        public T? Data { get; set; } = default(T?);

        public Error(string? Message, T? Data)
        {
            this.Message = Message;
            this.Data = Data;
        }
    }
    public sealed class IsLoading : Resource<T>
    {
        public T? Data { get; set; } = default(T?);
        public bool isLoading
        {
            get; set;
        } = false;

        public IsLoading(T? Data, bool isLoading)
        {
            this.Data = Data;
            this.isLoading = isLoading;
        }
    }
}

public static class Extensions
{
    public static Resource<T> OnError<T>(this Resource<T> resource, out T? Data, out string? message)
    {
        if (resource is Resource<T>.Error error)
        {
            message = error.Message;

            if (error.Data is null)
            {
                Data = default;

            }
            else Data = error.Data;
        }
        else
        {
            message = "";
            Data = default;
        }

        return resource;
    }
    public static Resource<T> OnSuccess<T>(this Resource<T> resource, out T? Data)
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
    public static Resource<T> OnLoading<T>(this Resource<T> resource, out T? Data, out bool isLoading)
    {
        if (resource is Resource<T>.IsLoading IsLoading)
        {
            if (IsLoading.Data is null)
            {
                Data = default;
            }
            else
                Data = IsLoading.Data;

            isLoading = IsLoading.isLoading;
        }
        else
        {
            Data = default;
            isLoading = false;
        }
        return resource;
    }

    public static Resource<T> OnError<T>(this Resource<T> resource, Action<Resource<T>.Error> onErrorAction)
    {
        if (resource is Resource<T>.Error error)
        {
            onErrorAction.Invoke(error);
        }
        return resource;
    }
    public static Resource<T> OnSuccess<T>(this Resource<T> resource, Action<Resource<T>.Success> onSuccessAction)
    {
        if (resource is Resource<T>.Success success)
        {            
            onSuccessAction.Invoke(success);
        }
        return resource;
    }
    public static Resource<T> OnLoadingStarted<T>(this Resource<T> resource, Action<Resource<T>.IsLoading> onIsLoadingAction)
    {
        if (resource is Resource<T>.IsLoading IsLoading)
        {
            onIsLoadingAction.Invoke(IsLoading);
        }
        return resource;
    }
    public static Resource<T> OnLoadingEnded<T>(this Resource<T> resource, Action<Resource<T>> onIsLoadingAction)
    {
        if (resource is not Resource<T>.IsLoading)
        {
            onIsLoadingAction.Invoke(resource);
        }
        return resource;
    }

}