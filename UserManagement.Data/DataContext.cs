using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data.Entities;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, Int64>
{
    private const Int64 AdminUserId = 2;

    public DataContext()
        : this(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("UserManagement.Data.DataContext")
            .Options)
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options) => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (options.IsConfigured)
        {
            return;
        }

        options.UseInMemoryDatabase("UserManagement.Data.DataContext");
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        model.Entity<User>().HasData(
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", DateOfBirth = new DateOnly(2001, 2, 3), IsActive = true },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", DateOfBirth = new DateOnly(2002, 3, 4), IsActive = true },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", DateOfBirth = new DateOnly(2003, 4, 5), IsActive = false },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", DateOfBirth = new DateOnly(2004, 5, 6), IsActive = true },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", DateOfBirth = new DateOnly(2005, 6, 7), IsActive = true },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", DateOfBirth = new DateOnly(2006, 7, 8), IsActive = true },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", DateOfBirth = new DateOnly(2007, 8, 9), IsActive = false },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", DateOfBirth = new DateOnly(2008, 9, 10), IsActive = false },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", DateOfBirth = new DateOnly(2009, 10, 11), IsActive = false },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", DateOfBirth = new DateOnly(2010, 11, 12), IsActive = true },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", DateOfBirth = new DateOnly(2011, 12, 13), IsActive = true });

        model.Entity<LogEntry>()
            .HasOne(log => log.User)
            .WithMany()
            .HasForeignKey(log => log.UserId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        model.Entity<LogEntry>()
            .HasIndex(log => new { log.TargetType, log.TargetId, log.Timestamp });

        model.Entity<LogEntry>().HasData(ExampleLogs());
    }

    public DbSet<User> DomainUsers { get; set; } = null!;

    public DbSet<LogEntry> Logs { get; set; } = null!;

    public virtual IQueryable<TEntity> GetAll<TEntity>() where TEntity : class => Set<TEntity>();

    public virtual void Create<TEntity>(TEntity entity) where TEntity : class
    {
        Add(entity);
        SaveChanges();
    }

    public new virtual void Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        SaveChanges();
    }

    public virtual void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        Remove(entity);
        SaveChanges();
    }

    private static Object[] ExampleLogs()
    {
        (Int64 Id, String Email)[] subjects =
        [
            (1, "ploew@example.com"),
            (2, "bfgates@example.com"),
            (3, "ctroy@example.com"),
            (4, "mraines@example.com"),
            (5, "sgodspeed@example.com"),
            (6, "himcdunnough@example.com"),
            (7, "cpoe@example.com"),
            (8, "emalus@example.com"),
            (9, "dmacready@example.com"),
            (10, "jblaze@example.com"),
            (11, "rfeld@example.com")
        ];

        Random random = Random.Shared;

        Int32 numDays = 10;
        DateTime now = DateTime.UtcNow;

        Int32 firstLogId = 1000;

        return Enumerable.Range(1, numDays)
            .Select(x => now.AddDays(-x))
            .SelectMany(day => Enumerable.Range(0, random.Next(3, 9))
                .Select(count => day.AddHours(count).AddMinutes(random.Next(0, 60)))
            )
            .Select(timestamp => (timestamp, subject: subjects[random.Next(subjects.Length)]))
            .Select((x, i) => new LogEntry
            {
                Id = firstLogId + i,
                Timestamp = x.timestamp,
                Action = "Edited user",
                Description = $"User {x.subject.Id} edited",
                UserId = AdminUserId,
                TargetType = LogTargets.User,
                TargetId = x.subject.Id,
                TargetLabel = x.subject.Email
            })
            .ToArray();
    }
}
