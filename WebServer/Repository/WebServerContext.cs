using Microsoft.EntityFrameworkCore;
using TimePolling;

namespace WebServer.Repository;

public sealed class WebServerContext : DbContext
{
    public WebServerContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
        Models.AddRange(TimePolling.Model.GenerateRandom());
        SaveChanges();
    }

    public DbSet<Model> Models { get; set; }
}