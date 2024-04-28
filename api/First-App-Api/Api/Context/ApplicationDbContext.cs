using System.Collections;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Api.Context;

public class ApplicationDbContext : DbContext
{
    private readonly IHistoryService _historyService;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHistoryService historyService) : base(options)
    {
        _historyService = historyService;
        Database.EnsureCreated();
        
    }

    public DbSet<Card> Cards { get; set; }
    public DbSet<TaskList> TaskLists { get; set; }
    public DbSet<History> Histories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskList>().HasIndex(t => t.Name).IsUnique();
        modelBuilder.Entity<Card>().HasOne(x => x.TaskList)
            .WithMany(x => x.Cards).HasForeignKey(x => x.TaskListId).OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var histories = new List<History>();
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity is TaskList && entry.State == EntityState.Deleted))
        {
            var cards = entry.Navigation("Cards");
            await cards.LoadAsync();
            foreach (var card in (IEnumerable<Card>)cards.CurrentValue)
            { 
                histories.Add(_historyService.TrackDeletion(card));
            }

        }
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity is Card && entry.State == EntityState.Modified))
        {
            var Originalvalues = entry.OriginalValues.Properties.ToDictionary(p => p.Name, p => entry.OriginalValues[p]);
            var CurrentValues = entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p]);
            histories.AddRange( _historyService.TrackUpdate(Originalvalues, CurrentValues));
        }
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity is Card && entry.State == EntityState.Deleted))
        {
            histories.Add(_historyService.TrackDeletion((Card)entry.Entity));
        }

        var addedEntries = ChangeTracker.Entries()
            .Where(entry => entry.Entity is Card && entry.State == EntityState.Added).ToList();
        
        var saveChangesResult = await base.SaveChangesAsync(cancellationToken);
        foreach (var entry in addedEntries)
        {
            histories.Add(_historyService.TrackCreation((Card)entry.Entity));
        }
        await Histories.AddRangeAsync(histories);
        var secondresult = await base.SaveChangesAsync(cancellationToken);
        return saveChangesResult & secondresult;

    }
}