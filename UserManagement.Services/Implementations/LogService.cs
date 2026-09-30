using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Implementations;

public sealed class LogService : ILogService
{
    private readonly DataContext _dataContext;

    public LogService(DataContext dataContext) => _dataContext = dataContext;

    public void Log(String action, String description, ApplicationUser actor, LogTarget? target = null)
    {
        _dataContext.Add(new LogEntry
        {
            Action = action,
            Description = description,
            User = actor,
            Timestamp = DateTime.UtcNow,
            TargetType = target?.Type,
            TargetId = target?.Id,
            TargetLabel = target?.Label
        });

        _dataContext.SaveChanges();
    }

    public IQueryable<LogEntry> GetBefore(DateTime time, Int32 count)
    {
        return _dataContext
            .Set<LogEntry>()
            .Include(log => log.User)
            .Where(log => log.Timestamp < time)
            .OrderByDescending(log => log.Timestamp)
            .Take(count)
            .AsNoTracking();
    }

    public IQueryable<LogEntry> GetAfter(DateTime time, Int32 count)
    {
        return _dataContext
            .Set<LogEntry>()
            .Include(log => log.User)
            .Where(log => log.Timestamp > time)
            .OrderBy(log => log.Timestamp)
            .Take(count)
            .AsNoTracking();
    }

    public IQueryable<LogEntry> GetBeforeForTarget(LogTarget target, DateTime time, Int32 count)
    {
        return GetBefore(time, count)
            .Where(log => log.TargetType == target.Type
                && log.TargetId == target.Id);
    }

    public IQueryable<LogEntry> GetAfterForTarget(LogTarget target, DateTime time, Int32 count)
    {
        return GetAfter(time, count)
            .Where(log => log.TargetType == target.Type
                && log.TargetId == target.Id);
    }
}
