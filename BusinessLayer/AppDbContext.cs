using BusinessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Miners.Web.BusinessLayer.Entities;
using TSoft.Framework4.EfEntities;
using File = BusinessLayer.Entities.File;

namespace Miners.Web.BusinessLayer;

public class AppDbContext : TsfwDbContext
{
    protected override string DefaultSchema => "sch_miners";
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Scoreboard>().HasNoKey();
    }

    public DbSet<Article> Articles { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Scoreboard> Scores { get; set; }
}