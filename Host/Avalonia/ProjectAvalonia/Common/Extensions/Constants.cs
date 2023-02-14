
using System;

namespace ProjectAvalonia.Common.Extensions;

public static class AppConstants
{
    public const string ClientSupportBackendVersionMin = "4";
    public const string ClientSupportBackendVersionMax = "4";
    public const string BackendMajorVersion = "4";

    /// <summary>
    /// By changing this, we can force to start over the transactions file, so old incorrect transactions would be cleared.
    /// It is also important to force the KeyManagers to be reindexed when this is changed by renaming the BlockState Height related property.
    /// </summary>
    public const string ConfirmedTransactionsVersion = "2";

    public const uint ProtocolVersionWitnessVersion = 70012;

    public const int InputBaseSizeInBytes = 41;

    public const int P2wpkhInputSizeInBytes = 41;
    public const int P2wpkhInputVirtualSize = 69;
    public const int P2pkhInputSizeInBytes = 145;
    public const int P2wpkhOutputVirtualSize = 31;

    public const int P2trInputVirtualSize = 58;
    public const int P2trOutputVirtualSize = 43;

    /// <summary>
    /// OBSOLATED, USE SPECIFIC TYPE
    /// </summary>
    public const int OutputSizeInBytes = 33;

    // https://en.bitcoin.it/wiki/Bitcoin
    // There are a maximum of 2,099,999,997,690,000 Bitcoin elements (called satoshis), which are currently most commonly measured in units of 100,000,000 known as BTC. Stated another way, no more than 21 million BTC can ever be created.
    public const long MaximumNumberOfSatoshis = 2099999997690000;

    public const decimal MaximumNumberOfBitcoins = 20999999.9769m;

    public const int SemiPrivateThreshold = 2;

    public const int FastestConfirmationTarget = 1;
    public const int TwentyMinutesConfirmationTarget = 2;
    public const int OneDayConfirmationTarget = 144;
    public const int SevenDaysConfirmationTarget = 1008;

    public const int BigFileReadWriteBufferSize = 1 * 1024 * 1024;

    public const decimal DefaultDustThreshold = 0.00005m;

    public const string AlphaNumericCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public const string CapitalAlphaNumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static readonly Version ClientVersion = new(2, 0, 2, 1);

    public static readonly Version HwiVersion = new("2.1.1");
    public static readonly Version BitcoinCoreVersion = new("21.2");
    public static readonly Version Ww1LegalDocumentsVersion = new(3, 0);
    public static readonly Version Ww2LegalDocumentsVersion = new(1, 0);

    // Defined in hours. Do not modify these values or the order!
    public static readonly int[] CoinJoinFeeRateMedianTimeFrames = new[] { 24, 168, 720 };

    public static readonly int[] ConfirmationTargets = new[]
    {
        2, // Twenty Minutes
		3, // Thirty Minutes
		6, // One Hour
		18, // Three Hours
		36, // Six Hours
		72, // Twelve Hours
		144, // One Day
		432, // Three Days
		1008, // Seven Days
	};

    public static string ClientSupportBackendVersionText => ClientSupportBackendVersionMin == ClientSupportBackendVersionMax
        ? ClientSupportBackendVersionMin
        : $"{ClientSupportBackendVersionMin} - {ClientSupportBackendVersionMax}";
}
