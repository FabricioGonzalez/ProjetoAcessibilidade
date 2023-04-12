using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ProjectAvalonia.Logging;

public class BenchmarkLogger : IDisposable
{
    private bool _disposedValue; // To detect redundant calls

    private BenchmarkLogger(
        LogLevel logLevel = LogLevel.Info
        , [CallerMemberName] string operationName = ""
        , [CallerFilePath] string callerFilePath = ""
        , [CallerLineNumber] int callerLineNumber = -1
    )
    {
        LogLevel = logLevel;
        OperationName = operationName;
        CallerFilePath = callerFilePath;
        CallerLineNumber = callerLineNumber;

        Stopwatch = Stopwatch.StartNew();
    }

    private LogLevel LogLevel
    {
        get;
    }

    public Stopwatch Stopwatch
    {
        get;
    }

    public string OperationName
    {
        get;
    }

    public string CallerFilePath
    {
        get;
    }

    public int CallerLineNumber
    {
        get;
    }

    /// <summary>
    ///     Logs the time between the creation of the class and the disposing of the class.
    ///     Example usage: using (BenchmarkLogger.Measure()) { /* Your code here */ }
    /// </summary>
    /// <param name="operationName">Which operation to measure. Default is the caller function name.</param>
    public static IDisposable Measure(
        LogLevel logLevel = LogLevel.Info
        , [CallerMemberName] string operationName = ""
        , [CallerFilePath] string callerFilePath = ""
        , [CallerLineNumber] int callerLineNumber = -1
    ) => new BenchmarkLogger(logLevel: logLevel, operationName: operationName, callerFilePath: callerFilePath
        , callerLineNumber: callerLineNumber);

    #region IDisposable Support

    protected virtual void Dispose(
        bool disposing
    )
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Stopwatch.Stop();

                var min = Stopwatch.Elapsed.TotalMinutes;
                var sec = Stopwatch.Elapsed.TotalSeconds;
                string message;
                if (min > 1)
                {
                    message = $"{OperationName} finished in {min:#.##} minutes.";
                }
                else if (sec > 1)
                {
                    message = $"{OperationName} finished in {sec:#.##} seconds.";
                }
                else
                {
                    message = $"{OperationName} finished in {Stopwatch.ElapsedMilliseconds} milliseconds.";
                }

                Logger.Log(level: LogLevel, message: message, callerFilePath: CallerFilePath
                    , callerLineNumber: CallerLineNumber);
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