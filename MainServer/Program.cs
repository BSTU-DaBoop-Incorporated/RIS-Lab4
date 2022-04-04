using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();
var env = app.Environment;

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "client-app";
    if (env.IsDevelopment()) spa.UseReactDevelopmentServer("start");
});

app.Run();