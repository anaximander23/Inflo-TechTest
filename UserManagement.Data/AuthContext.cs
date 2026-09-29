using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data.Entities;

namespace UserManagement.Data;

public sealed class AuthContext : IdentityDbContext<ApplicationUser, ApplicationRole, Int64>
{
    public AuthContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("UserManagement.Data.AuthContext");

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        model.Entity<LogEntry>()
            .HasOne(log => log.User)
            .WithMany()
            .HasForeignKey(log => log.UserId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        model.Entity<LogEntry>().HasData(ExampleLogs());
    }

    public DbSet<LogEntry> Logs { get; set; } = null!;

    private static Object[] ExampleLogs()
    {
        Int64 AdminUserId = 2;

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
                UserId = AdminUserId
            })
            .ToArray();
    }
}
