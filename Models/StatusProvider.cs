namespace TimePolling;

public class StatusProvider
{
    public static string WORKING = "Working";
    public static string PENDING = "Pending";
    public static string SUSPENDED = "Suspended";

    private readonly Status _status;

    public StatusProvider()
    {
        _status = new Status
        {
            State = "",
            RequestsServed = 0
        };
    }

    public string State
    {
        get => _status.State;
        set => _status.State = value;
    }

    public void IncrementServedRequests()
    {
        _status.RequestsServed++;
    }

    public Status GetStatus()
    {
        return _status;
    }
}