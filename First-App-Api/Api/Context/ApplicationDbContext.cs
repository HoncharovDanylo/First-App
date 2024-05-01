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
        modelBuilder.Entity<TaskList>().Property(x=>x.Name).HasMaxLength(100);
        modelBuilder.Entity<Card>().Property(x=>x.Title).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Card>().Property(x=>x.Description).HasMaxLength(1000);
        modelBuilder.Entity<Card>().Property(x=>x.Priority).IsRequired();
        modelBuilder.Entity<Card>().Property(x=>x.DueDate).IsRequired();
        // modelBuilder.Entity<Card>().HasIndex(x=>x.Title).IsUnique();
        
        modelBuilder.Entity<Card>().HasOne(x => x.TaskList)
            .WithMany(x => x.Cards).HasForeignKey(x => x.TaskListId).OnDelete(DeleteBehavior.Cascade);
        
        // Data seeding
        var lists = new List<TaskList>()
        {
            new TaskList() { Id =1, Name = "To Do" },
            new TaskList() { Id =2, Name = "In progress" },
        };
        var cards = new List<Card>()
        {
            new Card() { Id=1, Title = "Create structure", Description = "This is the first card", Priority = "Low", DueDate = DateOnly.Parse("2024-05-01"),TaskListId = 2 },
            new Card() { Id=2, Title = "Setup Entity Framework", Description = "This is the second card", Priority = "High", DueDate = DateOnly.Parse("2024-05-10"),TaskListId = 2 },
            new Card() { Id=3, Title = "Create models", Description = "This is the third card", Priority = "Medium", DueDate = DateOnly.Parse("2024-05-24"),TaskListId = 2 },
            new Card() { Id=4, Title = "Develop controller for lists", Description = "This is the fourth card", Priority = "Medium", DueDate = DateOnly.Parse("2024-06-05"),TaskListId = 2 },
            new Card() { Id=5, Title = "Implement validation", Description = "This is the fifth card", Priority = "Low", DueDate = DateOnly.Parse("2024-06-12"),TaskListId = 2 },
            new Card() { Id=6, Title = "Create front-end", Description = "This is the sixth card", Priority = "Low", DueDate = DateOnly.Parse("2024-06-14"),TaskListId = 1 },
            new Card() { Id=7, Title = "Test application", Description = "This is the seventh card", Priority = "High", DueDate = DateOnly.Parse("2024-06-19"),TaskListId = 1 },
            new Card() { Id=8, Title = "BugFix", Description = "This is the eight card", Priority = "High", DueDate = DateOnly.Parse("2024-06-19"),TaskListId = 1 },
            new Card() { Id=9, Title = "Containerize application", Description = "This is the ninths card", Priority = "Medium", DueDate = DateOnly.Parse("2024-06-23"),TaskListId = 1 },
            new Card() { Id=10, Title = "Push into master branch", Description = "This is the tenth card", Priority = "Low", DueDate = DateOnly.Parse("2024-06-27"),TaskListId = 1 },
        };
        var histories = new List<History>();
        var historyId = 1;
        foreach (var card in cards)
        {
            var hist = _historyService.TrackCreation(card, lists.Find(x=>x.Id == card.TaskListId).Name);
            hist.Id = historyId++;
            histories.Add(hist);
        }
        modelBuilder.Entity<TaskList>().HasData(lists);
        modelBuilder.Entity<Card>().HasData(cards);
        modelBuilder.Entity<History>().HasData(histories);
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
                histories.Add(_historyService.TrackDeletion(card, TaskLists.Find(card.TaskListId).Name));
            }

        }
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity is Card && entry.State == EntityState.Modified))
        {
            var Originalvalues = entry.OriginalValues.Properties.ToDictionary(p => p.Name, p => entry.OriginalValues[p]);
            Originalvalues.Add("oldListName", TaskLists.Find((int)(Originalvalues["TaskListId"])).Name);
            var CurrentValues = entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p]);
            CurrentValues.Add("newListName", TaskLists.Find((int)(CurrentValues["TaskListId"])).Name);
            histories.AddRange( _historyService.TrackUpdate(Originalvalues, CurrentValues));
        }
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity is Card && entry.State == EntityState.Deleted))
        {
            histories.Add(_historyService.TrackDeletion((Card)entry.Entity, TaskLists.Find(((Card)entry.Entity).TaskListId).Name));
        }

        var addedEntries = ChangeTracker.Entries()
            .Where(entry => entry.Entity is Card && entry.State == EntityState.Added).ToList();
        
        var saveChangesResult = await base.SaveChangesAsync(cancellationToken);
        foreach (var entry in addedEntries)
        {
            histories.Add(_historyService.TrackCreation((Card)entry.Entity,TaskLists.Find(((Card)entry.Entity).TaskListId).Name));
        }
        await Histories.AddRangeAsync(histories);
        var secondresult = await base.SaveChangesAsync(cancellationToken);
        return saveChangesResult & secondresult;

    }
}