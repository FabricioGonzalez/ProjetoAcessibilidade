using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectAvalonia.Common.Bases;
using ProjectAvalonia.Logging;

namespace ProjectAvalonia;

[JsonObject(memberSerialization: MemberSerialization.OptIn)]
public class Config : ConfigBase
{
    /// <summary>
    ///     Constructor for config population using Newtonsoft.JSON.
    /// </summary>
    public Config()
    {
        /*  ServiceConfiguration = null!;*/
    }

    public Config(
        string filePath
    ) : base(filePath: filePath)
    {
        /*   ServiceConfiguration = new ServiceConfiguration(GetBitcoinP2pEndPoint(), DustThreshold);*/
    }

    [JsonProperty(PropertyName = "EnableGpu")]
    public bool EnableGpu
    {
        get;
        internal set;
    } = true;

    [DefaultValue(value: true)]
    [JsonProperty(PropertyName = "DownloadNewVersion", DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool DownloadNewVersion
    {
        get;
        internal set;
    } = true;

    [JsonProperty(PropertyName = "AppVersion", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string AppVersion
    {
        get;
        internal set;
    } = "1.0.0";

    [JsonProperty(PropertyName = "AppLanguage", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string AppLanguage
    {
        get;
        internal set;
    } = "en";

    /// <inheritdoc />
    public override void LoadFile() => base.LoadFile();

    /* ServiceConfiguration = new ServiceConfiguration(GetBitcoinP2pEndPoint(), DustThreshold);*/
    protected override bool TryEnsureBackwardsCompatibility(
        string jsonString
    )
    {
        try
        {
            var jsObject = JsonConvert.DeserializeObject<JObject>(value: jsonString);

            if (jsObject is null)
            {
                Logger.LogWarning(message: "Failed to parse config JSON.");
                return false;
            }

            var saveIt = false;

            return saveIt;
        }
        catch (Exception ex)
        {
            Logger.LogWarning(message: "Backwards compatibility couldn't be ensured.");
            Logger.LogInfo(exception: ex);
            return false;
        }
    }
}