using Microsoft.AspNetCore.Mvc;
using TimePolling;
using WebServer.Models;

namespace MainServer.Controllers;

[Route("api")]
public class ServerController : Controller
{
    private readonly HttpClient _client;

    public ServerController()
    {
        _client = new HttpClient();
    }

    [HttpGet("poll")]
    public async Task<IActionResult> Poll(string from)
    {
        var uri = new Uri($"https://{from}/api/poll");
        PollData? data;
        try
        {
            data = await _client.GetFromJsonAsync<PollData>(uri);
        }
        catch (Exception e)
        {
            return NotFound();
        }

        return Ok(data);
    }

    [HttpGet("poll/time")]
    public async Task<IActionResult> PollTime(string from)
    {
        var uri = new Uri($"https://{from}/api/poll/time");
        var data = await _client.GetStringAsync(uri);

        return Ok(data);
    }

    [HttpGet("poll/status")]
    public async Task<IActionResult> PollStatus(string from)
    {
        var uri = new Uri($"https://{from}/api/poll/status");
        var data = await _client.GetFromJsonAsync<Status>(uri);

        return Ok(data);
    }
}