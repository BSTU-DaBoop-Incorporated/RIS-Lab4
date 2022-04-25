using Microsoft.EntityFrameworkCore;
using TimePolling;

namespace MainServer.Repository;

public class MainServerContext : DbContext
{
    public MainServerContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Model> Models { get; set; }
}