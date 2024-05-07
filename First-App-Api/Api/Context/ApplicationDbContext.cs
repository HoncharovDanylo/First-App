using System.Collections;
using Api.DataInitializers;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Api.Context;

public class ApplicationDbContext : DbContext
{
    private readonly IHistoryService _historyService;

    public ApplicationDbContext()
    {
        
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHistoryService historyService) : base(options)
    {
        _historyService = historyService;
        Database.EnsureCreated();
        
    }

    public virtual DbSet<Card> Cards { get; set; }
    public virtual DbSet<TaskList> TaskLists { get; set; }
    public virtual DbSet<History> Histories { get; set; }
    
    public virtual DbSet<Board> Boards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>().Property(x => x.Name).IsRequired().HasMaxLength(25);
        modelBuilder.Entity<Board>().HasIndex(x => x.Name).IsUnique();
        
        modelBuilder.Entity<TaskList>().Property(x=>x.Name).HasMaxLength(50);
        modelBuilder.Entity<TaskList>().Property(x=>x.BoardId).IsRequired();
        modelBuilder.Entity<TaskList>().HasIndex(x => new { x.BoardId, x.Name }).IsUnique();

        modelBuilder.Entity<History>().Property(x => x.BoardId).IsRequired();
        
        modelBuilder.Entity<Card>().Property(x=>x.Title).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Card>().Property(x=>x.Description).HasMaxLength(1000);
        modelBuilder.Entity<Card>().Property(x=>x.Priority).IsRequired();
        modelBuilder.Entity<Card>().Property(x=>x.DueDate).IsRequired();
        
        modelBuilder.Entity<Card>().HasOne(x => x.TaskList)
            .WithMany(x => x.Cards).HasForeignKey(x => x.TaskListId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskList>().HasOne(x => x.Board).WithMany(x => x.TaskLists).HasForeignKey(x => x.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Board>().HasMany(x=>x.Histories).WithOne(x=>x.Board).HasForeignKey(x=>x.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<History>().HasOne(x => x.Board).WithMany(x => x.Histories).HasForeignKey(x => x.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        var histories = new List<History>();
        var historyId = 1;
        var cards = new CardsDataInitializer().Data;
        var lists = new TaskListsDataInitializer().Data;
        var boards = new BoardDataInitializer().Data;
        foreach (var card in cards)
        {
            var hist = _historyService.TrackCreation(card, lists.FirstOrDefault(x=>x.Id == card.TaskListId).Name, lists.FirstOrDefault(x=>x.Id == card.TaskListId).BoardId);
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
        foreach (var  entry in ChangeTracker.Entries().Where(en=>en.Entity is Board && en.State == EntityState.Deleted))
        {
            var boardId = (entry.Entity as Board).Id;
            histories.RemoveAll(x => x.BoardId == boardId);
        }
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