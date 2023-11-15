using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;

using Newtonsoft.Json;

using ProjectAvalonia.Common.Bases;
using ProjectAvalonia.Common.Converters;

using ReactiveUI;

namespace ProjectAvalonia;

[JsonObject(memberSerialization: MemberSerialization.OptIn)]
public class UiConfig : ConfigBase
{
    private bool _autocopy;
    private bool _autoPaste;
    private bool _darkModeEnabled;
    private int _feeDisplayUnit;
    private int _feeTarget;
    private bool _hideOnClose;
    private bool _isCustomChangeAddress;
    private string? _lastSelectedWallet;
    private bool _oobe;
    private bool _privacyMode;
    private bool _runOnSystemStartup;
    private bool _sendAmountConversionReversed;
    private double? _windowHeight;
    private string _windowState = "Normal";
    private string _defaultLawContent = "";
    private double? _windowWidth;

    public UiConfig()
    {
    }

    public UiConfig(
        string filePath
    ) : base(filePath: filePath)
    {
        this.WhenAnyValue(
                property1: x => x.Autocopy,
                property2: x => x.AutoPaste,
                property3: x => x.IsCustomChangeAddress,
                property4: x => x.DarkModeEnabled,
                property5: x => x.FeeDisplayUnit,
                property6: x => x.LastSelectedWallet,
                property7: x => x.WindowState,
                property8: x => x.Oobe,
                property9: x => x.RunOnSystemStartup,
                property10: x => x.PrivacyMode,
                property11: x => x.HideOnClose,
                property12: x => x.FeeTarget,
                selector: (
                    _
                    , _
                    , _
                    , _
                    , _
                    , _
                    , _
                    , _
                    , _
                    , _
                    , _
                    , _
                ) => Unit.Default)
            .Throttle(dueTime: TimeSpan.FromMilliseconds(value: 500))
            .Skip(count: 1) // Won't save on UiConfig creation.
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Subscribe(onNext: _ => ToFile());

        this.WhenAnyValue(property1: x => x.SendAmountConversionReversed)
            .Throttle(dueTime: TimeSpan.FromMilliseconds(value: 500))
            .Skip(count: 1) // Won't save on UiConfig creation.
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Subscribe(onNext: _ => ToFile());

        this.WhenAnyValue(property1: x => x.DefaultLawContent)
           .Throttle(dueTime: TimeSpan.FromMilliseconds(value: 500))
           .Skip(count: 1) // Won't save on UiConfig creation.
           .ObserveOn(scheduler: RxApp.MainThreadScheduler)
           .Subscribe(onNext: _ => ToFile());

        this.WhenAnyValue(
                property1: x => x.WindowWidth,
                property2: x => x.WindowHeight)
            .Throttle(dueTime: TimeSpan.FromMilliseconds(value: 500))
            .Skip(count: 1) // Won't save on UiConfig creation.
            .ObserveOn(scheduler: RxApp.TaskpoolScheduler)
            .Subscribe(onNext: _ => ToFile());
    }

    [JsonProperty(PropertyName = "Oobe", DefaultValueHandling = DefaultValueHandling.Populate)]
    [DefaultValue(value: true)]
    public bool Oobe
    {
        get => _oobe;
        set => RaiseAndSetIfChanged(field: ref _oobe, value: value);
    }

    [JsonProperty(PropertyName = "WindowState")]
    [JsonConverter(converterType: typeof(WindowStateAfterStartJsonConverter))]
    public string WindowState
    {
        get => _windowState;
        internal set => RaiseAndSetIfChanged(field: ref _windowState, value: value);
    }
    [JsonProperty(PropertyName = "DefaultLawContent", DefaultValueHandling = DefaultValueHandling.Populate)]
    [DefaultValue(value: "Legislação Vigente: NBR 9.050/15, NBR 16.537/16, Decreto Nº 5296 de 02.12.2004 e Lei Federal 13.146/16")]
    public string DefaultLawContent
    {
        get => _defaultLawContent;
        internal set => RaiseAndSetIfChanged(field: ref _defaultLawContent, value: value);
    }

    [DefaultValue(value: 2)]
    [JsonProperty(PropertyName = "FeeTarget", DefaultValueHandling = DefaultValueHandling.Populate)]
    public int FeeTarget
    {
        get => _feeTarget;
        internal set => RaiseAndSetIfChanged(field: ref _feeTarget, value: value);
    }

    [DefaultValue(value: 0)]
    [JsonProperty(PropertyName = "FeeDisplayUnit", DefaultValueHandling = DefaultValueHandling.Populate)]
    public int FeeDisplayUnit
    {
        get => _feeDisplayUnit;
        set => RaiseAndSetIfChanged(field: ref _feeDisplayUnit, value: value);
    }

    [DefaultValue(value: true)]
    [JsonProperty(PropertyName = "Autocopy", DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool Autocopy
    {
        get => _autocopy;
        set => RaiseAndSetIfChanged(field: ref _autocopy, value: value);
    }

    [DefaultValue(value: false)]
    [JsonProperty(PropertyName = nameof(AutoPaste), DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool AutoPaste
    {
        get => _autoPaste;
        set => RaiseAndSetIfChanged(field: ref _autoPaste, value: value);
    }

    [DefaultValue(value: false)]
    [JsonProperty(PropertyName = "IsCustomChangeAddress", DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool IsCustomChangeAddress
    {
        get => _isCustomChangeAddress;
        set => RaiseAndSetIfChanged(field: ref _isCustomChangeAddress, value: value);
    }

    [DefaultValue(value: false)]
    [JsonProperty(PropertyName = "PrivacyMode", DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool PrivacyMode
    {
        get => _privacyMode;
        set => RaiseAndSetIfChanged(field: ref _privacyMode, value: value);
    }

    [DefaultValue(value: true)]
    [JsonProperty(PropertyName = "DarkModeEnabled", DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool DarkModeEnabled
    {
        get => _darkModeEnabled;
        set => RaiseAndSetIfChanged(field: ref _darkModeEnabled, value: value);
    }

    [DefaultValue(value: null)]
    [JsonProperty(PropertyName = "LastSelectedWallet", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string? LastSelectedWallet
    {
        get => _lastSelectedWallet;
        set => RaiseAndSetIfChanged(field: ref _lastSelectedWallet, value: value);
    }

    [DefaultValue(value: false)]
    [JsonProperty(PropertyName = "RunOnSystemStartup", DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool RunOnSystemStartup
    {
        get => _runOnSystemStartup;
        set => RaiseAndSetIfChanged(field: ref _runOnSystemStartup, value: value);
    }

    [DefaultValue(value: false)]
    [JsonProperty(PropertyName = "HideOnClose", DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool HideOnClose
    {
        get => _hideOnClose;
        set => RaiseAndSetIfChanged(field: ref _hideOnClose, value: value);
    }

    [DefaultValue(value: false)]
    [JsonProperty(PropertyName = "SendAmountConversionReversed", DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool SendAmountConversionReversed
    {
        get => _sendAmountConversionReversed;
        internal set => RaiseAndSetIfChanged(field: ref _sendAmountConversionReversed, value: value);
    }

    [JsonProperty(PropertyName = "WindowWidth")]
    public double? WindowWidth
    {
        get => _windowWidth;
        internal set => RaiseAndSetIfChanged(field: ref _windowWidth, value: value);
    }

    [JsonProperty(PropertyName = "WindowHeight")]
    public double? WindowHeight
    {
        get => _windowHeight;
        internal set => RaiseAndSetIfChanged(field: ref _windowHeight, value: value);
    }

    [DefaultValue(value: false)]
    [JsonProperty(PropertyName = "ImageStrecthing", DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool ImageStrecthing
    {
        get;
        internal set;
    }
}