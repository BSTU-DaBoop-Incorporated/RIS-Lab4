namespace TimePolling;

public interface ISyncTimeProvider
{
    long SyncTime { get; }
}