using System.Net;
using GuerrillaNtp;

namespace TimePolling;

public class NtpSyncTimeProvider : ISyncTimeProvider, IDisposable
{
    public static string[] NtpServerList =
    {
        "pool.ntp.org",
        "asia.pool.ntp.org",
        "europe.pool.ntp.org",
        "north-america.pool.ntp.org",
        "oceania.pool.ntp.org",
        "south-america.pool.ntp.org",
        "time-a.nist.gov"
    };

    private readonly NtpClient _client = new(Dns.GetHostAddresses(NtpServerList[0]).First());

    private readonly object _timeLock = new();

    private readonly Timer _timer;

    private DateTime _dateTime;

    public NtpSyncTimeProvider()
    {
        var startTimeSpan = TimeSpan.Zero;
        var periodTimeSpan = TimeSpan.FromMilliseconds(500);


        _timer = new Timer(_ =>
        {
            lock (_timeLock)
            {
                try
                {
                    _dateTime = DateTime.Now + _client.GetCorrectionOffset();
                }
                catch (Exception)
                {
                }
            }
        }, null, startTimeSpan, periodTimeSpan);
    }

    public void Dispose()
    {
        _client.Dispose();
        _timer.Dispose();
    }


    public long SyncTime
    {
        get
        {
            lock (_timeLock)
            {
                return ((DateTimeOffset)_dateTime).ToUnixTimeMilliseconds() * 1000;
            }
        }
    }
}