using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Implementations;

public sealed class LogService : ILogService
{
    public LogService(AuthContext dataContext)
    {
        _dataContext = dataContext;
    }

    private readonly AuthContext _dataContext;

    public void Log(String action, String description, ApplicationUser actor)
    {
        _dataContext.Add(new LogEntry
        {
            Action = action,
            Description = description,
            User = actor,
            Timestamp = DateTime.UtcNow
        });
        _dataContext.SaveChanges();
    }

    public IQueryable<LogEntry> GetAll()
    {
        return _dataContext.Set<LogEntry>().AsNoTracking();
    }

    public IQueryable<LogEntry> Get(DateTime startTime, DateTime? endTime = null)
    {
        var results = _dataContext
            .Set<LogEntry>()
            .Where(x => x.Timestamp >= startTime);

        if (endTime.HasValue)
        {
            results = results.Where(x => x.Timestamp < endTime);
        }

        return results.AsNoTracking();
    }
}
