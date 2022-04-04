using Serilog;
using TimePolling;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => { lc.WriteTo.File("log.log"); });

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(new NtpSyncTimeProvider());
builder.Services.AddSingleton(new StatusProvider());
builder.Services.AddSingleton<ISyncTimeProviderFactory, SyncTimeProviderFactory>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();