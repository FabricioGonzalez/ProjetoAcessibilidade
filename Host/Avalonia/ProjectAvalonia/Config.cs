using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using ProjectAvalonia.Common.Bases;
using ProjectAvalonia.Logging;

namespace ProjectAvalonia;

[JsonObject(MemberSerialization.OptIn)]
public class Config : ConfigBase
{
    /// <summary>
    /// Constructor for config population using Newtonsoft.JSON.
    /// </summary>
    public Config() : base()
    {
        /*  ServiceConfiguration = null!;*/
    }

    public Config(string filePath) : base(filePath)
    {
        /*   ServiceConfiguration = new ServiceConfiguration(GetBitcoinP2pEndPoint(), DustThreshold);*/
    }
    [JsonProperty(PropertyName = "EnableGpu")]
    public bool EnableGpu { get; internal set; } = true;


    /// <inheritdoc />
    public override void LoadFile()
    {
        base.LoadFile();

        /* ServiceConfiguration = new ServiceConfiguration(GetBitcoinP2pEndPoint(), DustThreshold);*/
    }

    protected override bool TryEnsureBackwardsCompatibility(string jsonString)
    {
        try
        {
            var jsObject = JsonConvert.DeserializeObject<JObject>(jsonString);

            if (jsObject is null)
            {
                Logger.LogWarning("Failed to parse config JSON.");
                return false;
            }

            bool saveIt = false;

            return saveIt;
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Backwards compatibility couldn't be ensured.");
            Logger.LogInfo(ex);
            return false;
        }
    }
}