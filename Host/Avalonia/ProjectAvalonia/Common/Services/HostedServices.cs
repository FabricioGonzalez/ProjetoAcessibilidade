using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ProjectAvalonia.Logging;

namespace ProjectAvalonia.Common.Services;

public class HostedServices : IDisposable
{
    private volatile bool _disposedValue; // To detect redundant calls

    private List<HostedService> Services
    {
        get;
    } = new();

    private object ServicesLock
    {
        get;
    } = new();

    private bool IsStartAllAsyncStarted
    {
        get;
        set;
    }

    public void Register<T>(
        Func<IHostedService> serviceFactory
        , string friendlyName
    )
        where T : class, IHostedService => Register<T>(service: serviceFactory(), friendlyName: friendlyName);

    private void Register<T>(
        IHostedService service
        , string friendlyName
    )
        where T : class, IHostedService
    {
        if (typeof(T) != service.GetType())
        {
            throw new ArgumentException(
                message:
                $"Type mismatch: {nameof(T)} is {typeof(T).Name}, but {nameof(service)} is {service.GetType()}.");
        }

        if (IsStartAllAsyncStarted)
        {
            throw new InvalidOperationException(message: "Services are already started.");
        }

        lock (ServicesLock)
        {
            if (AnyNoLock<T>())
            {
                throw new InvalidOperationException(message: $"{typeof(T).Name} is already registered.");
            }

            Services.Add(item: new HostedService(service: service, friendlyName: friendlyName));
        }
    }

    public async Task StartAllAsync(
        CancellationToken token = default
    )
    {
        if (IsStartAllAsyncStarted)
        {
            throw new InvalidOperationException(message: "Operation is already started.");
        }

        IsStartAllAsyncStarted = true;

        var exceptions = new List<Exception>();
        var exceptionsLock = new object();

        var tasks = CloneServices().Select(selector: x => x.Service.StartAsync(cancellationToken: token).ContinueWith(
            continuationAction: y =>
            {
                if (y.Exception is null)
                {
                    Logger.LogInfo(message: $"Started {x.FriendlyName}.");
                }
                else
                {
                    lock (exceptionsLock)
                    {
                        exceptions.Add(item: y.Exception);
                    }

                    Logger.LogError(message: $"Error starting {x.FriendlyName}.");
                    Logger.LogError(exception: y.Exception);
                }
            }));

        await Task.WhenAll(tasks: tasks).ConfigureAwait(continueOnCapturedContext: false);

        if (exceptions.Any())
        {
            throw new AggregateException(innerExceptions: exceptions);
        }
    }

    public async Task StopAllAsync(
        CancellationToken token = default
    )
    {
        var tasks = CloneServices().Select(selector: x => x.Service.StopAsync(cancellationToken: token).ContinueWith(
            continuationAction: y =>
            {
                if (y.Exception is null)
                {
                    Logger.LogInfo(message: $"Stopped {x.FriendlyName}.");
                }
                else
                {
                    Logger.LogError(message: $"Error stopping {x.FriendlyName}.");
                    Logger.LogError(exception: y.Exception);
                }
            }));

        await Task.WhenAll(tasks: tasks).ConfigureAwait(continueOnCapturedContext: false);
    }

    private IEnumerable<HostedService> CloneServices()
    {
        lock (ServicesLock)
        {
            return Services.ToArray();
        }
    }

    public T? GetOrDefault<T>()
        where T : class, IHostedService
    {
        lock (ServicesLock)
        {
            return Services.SingleOrDefault(predicate: x => x.Service is T)?.Service as T;
        }
    }

    public T Get<T>()
        where T : class, IHostedService
    {
        lock (ServicesLock)
        {
            return (T)Services.Single(predicate: x => x.Service is T).Service;
        }
    }

    public bool Any<T>()
        where T : class, IHostedService
    {
        lock (ServicesLock)
        {
            return AnyNoLock<T>();
        }
    }

    private bool AnyNoLock<T>()
        where T : class, IHostedService => Services.Any(predicate: x => x.Service is T);

    #region IDisposable Support

    protected virtual void Dispose(
        bool disposing
    )
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                foreach (var service in CloneServices())
                {
                    if (service.Service is IDisposable disposable)
                    {
                        disposable?.Dispose();
                        Logger.LogInfo(message: $"Disposed {service.FriendlyName}.");
                    }
                }
            }

            _disposedValue = true;
        }
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose() =>
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(disposing: true);

    #endregion IDisposable Support
}