using MainServer.Repository;
using Microsoft.AspNetCore.Mvc;
using TimePolling;

namespace MainServer.Controllers;

[Route("api")]
public class ServerController : Controller
{
    private readonly HttpClient _client;
    private readonly MainServerContext _context;

    public ServerController(MainServerContext context)
    {
        _context = context;
        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            return true;
        };
        _client = new HttpClient(clientHandler);
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

    [HttpGet("pull")]
    public async Task<IActionResult> Pull(string from)
    {
        var uri = new Uri($"https://{from}/api/pull");
        var data = await _client.GetFromJsonAsync<List<Model>>(uri);

        if (data != null)
        {
            _context.Models.AddRange(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        return NotFound("No pull data found");
    }

    [HttpGet("push")]
    public async Task<IActionResult> Push(string from)
    {
        var uri = new Uri($"https://{from}/api/push");

        var models = _context.Models.ToList();
        var data = await _client.PostAsJsonAsync(uri, models);

        return Ok(await data.Content.ReadAsStringAsync());
    }
}