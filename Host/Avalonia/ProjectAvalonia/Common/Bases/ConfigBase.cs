using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Json;
using ProjectAvalonia.Logging;

namespace ProjectAvalonia.Common.Bases;

public abstract class ConfigBase
    : NotifyPropertyChangedBase
        , IConfig
{
    protected ConfigBase()
    {
    }

    protected ConfigBase(
        string filePath
    )
    {
        SetFilePath(path: filePath);
    }

    /// <remarks>
    ///     Guards both storing to <see cref="FilePath" /> and retrieving contents of <see cref="FilePath" />.
    ///     <para>Otherwise, we risk concurrent read and write operations on <see cref="FilePath" />.</para>
    /// </remarks>
    protected object FileLock
    {
        get;
    } = new();

    /// <inheritdoc />
    public string FilePath
    {
        get;
        private set;
    } = "";

    /// <inheritdoc />
    public void AssertFilePathSet()
    {
        if (string.IsNullOrWhiteSpace(value: FilePath))
        {
            throw new NotSupportedException(
                message: $"{nameof(FilePath)} is not set. Use {nameof(SetFilePath)} to set it.");
        }
    }

    /// <inheritdoc />
    public bool CheckFileChange()
    {
        AssertFilePathSet();

        if (!File.Exists(path: FilePath))
        {
            throw new FileNotFoundException(message: $"{GetType().Name} file did not exist at path: `{FilePath}`.");
        }

        lock (FileLock)
        {
            var jsonString = ReadFileNoLock();
            var newConfigObject = Activator.CreateInstance(type: GetType())!;
            JsonConvert.PopulateObject(value: jsonString, target: newConfigObject
                , settings: JsonSerializationOptions.Default.Settings);

            return !AreDeepEqual(otherConfig: newConfigObject);
        }
    }

    /// <inheritdoc />
    public virtual void LoadOrCreateDefaultFile()
    {
        AssertFilePathSet();

        lock (FileLock)
        {
            JsonConvert.PopulateObject(value: "{}", target: this);

            if (!File.Exists(path: FilePath))
            {
                Logger.LogInfo(message: $"{GetType().Name} file did not exist. Created at path: `{FilePath}`.");
            }
            else
            {
                try
                {
                    LoadFileNoLock();
                }
                catch (Exception ex)
                {
                    Logger.LogInfo(
                        message:
                        $"{GetType().Name} file has been deleted because it was corrupted. Recreated default version at path: `{FilePath}`.");
                    Logger.LogWarning(exception: ex);
                }
            }

            ToFileNoLock();
        }
    }

    /// <inheritdoc />
    public virtual void LoadFile()
    {
        lock (FileLock)
        {
            LoadFileNoLock();
        }
    }

    /// <inheritdoc />
    public void SetFilePath(
        string path
    ) => FilePath = Guard.NotNullOrEmptyOrWhitespace(parameterName: nameof(path), value: path, trim: true);

    /// <inheritdoc />
    public bool AreDeepEqual(
        object otherConfig
    )
    {
        var serializer = JsonSerializer.Create(settings: JsonSerializationOptions.Default.Settings);
        var currentConfig = JObject.FromObject(o: this, jsonSerializer: serializer);
        var otherConfigJson = JObject.FromObject(o: otherConfig, jsonSerializer: serializer);
        return JToken.DeepEquals(t1: otherConfigJson, t2: currentConfig);
    }

    /// <inheritdoc />
    public void ToFile()
    {
        lock (FileLock)
        {
            ToFileNoLock();
        }
    }

    protected virtual bool TryEnsureBackwardsCompatibility(
        string jsonString
    ) => true;

    protected void LoadFileNoLock()
    {
        var jsonString = ReadFileNoLock();

        JsonConvert.PopulateObject(value: jsonString, target: this
            , settings: JsonSerializationOptions.Default.Settings);

        if (TryEnsureBackwardsCompatibility(jsonString: jsonString))
        {
            ToFileNoLock();
        }
    }

    protected void ToFileNoLock()
    {
        AssertFilePathSet();

        var jsonString = JsonConvert.SerializeObject(value: this, formatting: Formatting.Indented
            , settings: JsonSerializationOptions.Default.Settings);
        WriteFileNoLock(contents: jsonString);
    }

    protected void WriteFileNoLock(
        string contents
    ) => File.WriteAllText(path: FilePath, contents: contents, encoding: Encoding.UTF8);

    protected string ReadFileNoLock() => File.ReadAllText(path: FilePath, encoding: Encoding.UTF8);
}