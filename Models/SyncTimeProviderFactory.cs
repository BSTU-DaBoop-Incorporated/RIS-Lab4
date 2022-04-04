namespace TimePolling;

public class SyncTimeProviderFactory : ISyncTimeProviderFactory
{
    // private readonly SinceStartupSyncTimeProvider _sinceStartupSyncTimeProvider;
    private readonly NtpSyncTimeProvider _ntpSyncTimeProvider;

    public SyncTimeProviderFactory(NtpSyncTimeProvider ntpSyncTimeProvider)
    {
        // _sinceStartupSyncTimeProvider = fromStartupSyncTimeProvider;
        _ntpSyncTimeProvider = ntpSyncTimeProvider;
    }

    public ISyncTimeProvider CreateInstance()
    {
        return _ntpSyncTimeProvider;
    }
    // => mode switch
    // {
    //     SyncMode.SinceStartup => _sinceStartupSyncTimeProvider,
    //     SyncMode.NTP => _ntpSyncTimeProvider,
    //     _ => throw new NotImplementedException()
    // };
}