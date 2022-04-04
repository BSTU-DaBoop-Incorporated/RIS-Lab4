namespace TimePolling;

public interface ISyncTimeProviderFactory
{
    ISyncTimeProvider CreateInstance();
}