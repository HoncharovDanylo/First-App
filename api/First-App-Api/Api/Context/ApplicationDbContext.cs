using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Card> Cards { get; set; }
    public DbSet<TaskList> TaskLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskList>().HasIndex(t=>t.Name).IsUnique();
        modelBuilder.Entity<Card>().HasOne(x => x.TaskList)
            .WithMany(x => x.Cards).HasForeignKey(x => x.TaskListId).OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}