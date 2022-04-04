using Microsoft.AspNetCore.Mvc;
using TimePolling;

namespace WebServer.Controllers;

[Route("api")]
public class WebServerController : Controller
{
    private readonly StatusProvider _status;
    private readonly ISyncTimeProviderFactory _syncTimeProviderFactory;

    public WebServerController(ISyncTimeProviderFactory syncTimeProviderFactory, StatusProvider status)
    {
        _syncTimeProviderFactory = syncTimeProviderFactory;
        _status = status;
        _status.State = StatusProvider.WORKING;
    }

    private static long GetCorrection(long clientTime, long serverTime)
    {
        var correction = serverTime - clientTime;
        return correction;
    }

    private static string RandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [HttpGet("poll/status")]
    public IActionResult Status(int clientTime)
    {
        return Ok(_status.GetStatus());
    }

    [HttpGet("poll/time")]
    public IActionResult Time(int clientTime = 0)
    {
        var ntpTimeProvider = _syncTimeProviderFactory.CreateInstance();
        var startTIme = ntpTimeProvider.SyncTime;
        var correction = GetCorrection(clientTime, startTIme);
        _status.IncrementServedRequests();

        return Ok(correction);
    }
}