using Microsoft.AspNetCore.Mvc;
using TimePolling;
using WebServer.Repository;

namespace WebServer.Controllers;

[Route("api")]
public class WebServerController : Controller
{
    private readonly WebServerContext _context;
    private readonly StatusProvider _status;
    private readonly ISyncTimeProviderFactory _syncTimeProviderFactory;

    public WebServerController(ISyncTimeProviderFactory syncTimeProviderFactory, StatusProvider status,
        WebServerContext context)
    {
        _syncTimeProviderFactory = syncTimeProviderFactory;
        _status = status;
        _context = context;
        _status.State = StatusProvider.WORKING;
    }

    private static long GetCorrection(long clientTime, long serverTime)
    {
        var correction = serverTime - clientTime;
        return correction;
    }

    [HttpGet("/poll/status")]
    public IActionResult Status(int clientTime)
    {
        return Ok(_status.GetStatus());
    }

    [HttpGet("/poll/time")]
    public IActionResult Time(int clientTime = 0)
    {
        var ntpTimeProvider = _syncTimeProviderFactory.CreateInstance();
        var startTIme = ntpTimeProvider.SyncTime;
        var correction = GetCorrection(clientTime, startTIme);
        _status.IncrementServedRequests();

        return Ok(correction);
    }

    [HttpGet("pull")]
    public IActionResult Pull()
    {
        var models = _context.Models;
        return Ok(models);
    }

    [HttpPost("push")]
    public async Task<IActionResult> Push([FromBody] List<Model> model)
    {
        _context.Models.AddRange(model);
        await _context.SaveChangesAsync();

        return Ok("Data pushed");
    }
}