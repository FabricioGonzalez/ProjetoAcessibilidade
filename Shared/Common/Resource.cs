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
    public static Resource<T> OnError<T>(this Resource<T> resource, ref T? Data, ref string? message)
    {
        if (resource is Resource<T>.Error error)
        {
            message = error.Message;

            if (error.Data is not null)
            {
                Data = error.Data;
            }
        }
        return resource;
    }
    public static Resource<T> OnSuccess<T>(this Resource<T> resource, ref T? Data)
    {
        if (resource is Resource<T>.Success success)
        {
            Data = success.Data;
        }
        return resource;
    }
    public static Resource<T> OnLoading<T>(this Resource<T> resource,ref T? Data, ref bool isLoading)
    {
        if (resource is Resource<T>.IsLoading IsLoading)
        {
            if (Data is not null)
            {
                Data = IsLoading.Data;
            }

            isLoading = IsLoading.isLoading;
        }
        return resource;
    }
}