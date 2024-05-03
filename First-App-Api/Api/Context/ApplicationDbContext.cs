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
    
    public DbSet<Board?> Boards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>().Property(x => x.Name).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Board>().HasIndex(x => x.Name).IsUnique();
        
        modelBuilder.Entity<TaskList>().Property(x=>x.Name).HasMaxLength(50);
        modelBuilder.Entity<TaskList>().Property(x=>x.BoardId).IsRequired();
        modelBuilder.Entity<TaskList>().HasIndex(x => new { x.BoardId, x.Name }).IsUnique();
        
        modelBuilder.Entity<Card>().Property(x=>x.Title).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Card>().Property(x=>x.Description).HasMaxLength(1000);
        modelBuilder.Entity<Card>().Property(x=>x.Priority).IsRequired();
        modelBuilder.Entity<Card>().Property(x=>x.DueDate).IsRequired();
        
        modelBuilder.Entity<Card>().HasOne(x => x.TaskList)
            .WithMany(x => x.Cards).HasForeignKey(x => x.TaskListId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskList>().HasOne(x => x.Board).WithMany(x => x.TaskLists).HasForeignKey(x => x.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<History>().HasOne(x=>x.Board).WithMany(x=>x.Histories).HasForeignKey(x=>x.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        // Data seeding
        var boards = new List<Board>()
        {
            new Board() { Id = 1, Name = "Initial Board" }
        };
        var lists = new List<TaskList>()
        {
            new TaskList() { Id =1, Name = "To Do", BoardId = 1},
            new TaskList() { Id =2, Name = "In progress", BoardId = 1},
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
            var hist = _historyService.TrackCreation(card, lists.Find(x=>x.Id == card.TaskListId).Name, lists.Find(x=>x.Id == card.TaskListId).BoardId);
            hist.Id = historyId++;
            histories.Add(hist);
        }

        modelBuilder.Entity<Board>().HasData(boards);
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
                histories.Add(_historyService.TrackDeletion(card, TaskLists.Find(card.TaskListId).Name, TaskLists.Find(card.TaskListId).BoardId));
            }

        }
        
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity is Card && entry.State == EntityState.Modified))
        {
            var Originalvalues = entry.OriginalValues.Properties.ToDictionary(p => p.Name, p => entry.OriginalValues[p]);
            Originalvalues.Add("oldListName", TaskLists.Find((int)(Originalvalues["TaskListId"])).Name);
            Originalvalues.Add("BoardId", TaskLists.Find((int)(Originalvalues["TaskListId"])).BoardId);
            var CurrentValues = entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p]);
            CurrentValues.Add("newListName", TaskLists.Find((int)(CurrentValues["TaskListId"])).Name);
            CurrentValues.Add("BoardId", TaskLists.Find((int)(CurrentValues["TaskListId"])).BoardId);
            histories.AddRange( _historyService.TrackUpdate(Originalvalues, CurrentValues));
        }
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity is Card && entry.State == EntityState.Deleted))
        {
            histories.Add(_historyService.TrackDeletion((Card)entry.Entity, TaskLists.Find(((Card)entry.Entity).TaskListId).Name,TaskLists.Find(((Card)entry.Entity).TaskListId).BoardId));
        }

        var addedEntries = ChangeTracker.Entries()
            .Where(entry => entry.Entity is Card && entry.State == EntityState.Added).ToList();
        
        var saveChangesResult = await base.SaveChangesAsync(cancellationToken);
        foreach (var entry in addedEntries)
        {
            histories.Add(_historyService.TrackCreation((Card)entry.Entity,TaskLists.Find(((Card)entry.Entity).TaskListId).Name, TaskLists.Find(((Card)entry.Entity).TaskListId).BoardId ));
        }
        await Histories.AddRangeAsync(histories);
        var secondresult = await base.SaveChangesAsync(cancellationToken);
        return saveChangesResult & secondresult;

    }
}