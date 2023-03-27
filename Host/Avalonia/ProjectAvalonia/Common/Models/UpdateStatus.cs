using System;

namespace ProjectAvalonia.Common.Models;
public class UpdateStatus : IEquatable<UpdateStatus>
{
    public UpdateStatus(bool clientUpToDate, Version clientVersion)
    {
        ClientUpToDate = clientUpToDate;
        ClientVersion = clientVersion;
    }

    public bool ClientUpToDate
    {
        get;
    }
    public bool IsReadyToInstall
    {
        get; set;
    }

    public Version ClientVersion
    {
        get; set;
    }

    #region EqualityAndComparison

    public static bool operator ==(UpdateStatus? x, UpdateStatus? y)
        => (x?.ClientUpToDate, x?.ClientVersion) == (y?.ClientUpToDate, y?.ClientVersion);

    public static bool operator !=(UpdateStatus? x, UpdateStatus? y) => !(x == y);

    public override bool Equals(object? obj) => Equals(obj as UpdateStatus);

    public bool Equals(UpdateStatus? other) => this == other;

    public override int GetHashCode() => (ClientUpToDate, ClientVersion).GetHashCode();

    #endregion EqualityAndComparison
}
