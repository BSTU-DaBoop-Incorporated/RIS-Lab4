using Microsoft.EntityFrameworkCore;
using Serilog;
using TimePolling;
using WebServer.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => { lc.WriteTo.File("log.log"); });
builder.Services.AddDbContext<WebServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        options => { options.EnableRetryOnFailure(); }));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(new NtpSyncTimeProvider());
builder.Services.AddSingleton(new StatusProvider());
builder.Services.AddSingleton<ISyncTimeProviderFactory, SyncTimeProviderFactory>();

var app = builder.Build();

app.MapControllers();

app.Run();