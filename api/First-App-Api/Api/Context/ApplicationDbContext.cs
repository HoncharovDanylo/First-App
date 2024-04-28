using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Api.Context;

public class ApplicationDbContext : DbContext
{
    [Inject]
    private IHistoryService HistoryService { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Card> Cards { get; set; }
    public DbSet<TaskList> TaskLists { get; set; }
    public DbSet<History> Histories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskList>().HasIndex(t => t.Name).IsUnique();
        modelBuilder.Entity<Card>().HasOne(x => x.TaskList)
            .WithMany(x => x.Cards).HasForeignKey(x => x.TaskListId).OnDelete(DeleteBehavior.ClientCascade);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity is Card && entry.State == EntityState.Deleted))
        {
            entry.State = EntityState.Modified;
            ((Card)entry.Entity).IsDeleted = true;
            ((Card)entry.Entity).TaskListId = null;
             HistoryService.TrackDeletion((Card)entry.Entity);
        }

         return await base.SaveChangesAsync(cancellationToken);
    }
}