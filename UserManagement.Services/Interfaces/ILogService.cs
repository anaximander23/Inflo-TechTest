using System;
using System.Linq;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Interfaces;

public interface ILogService
{
    void Log(String action, String description, ApplicationUser actor, LogTarget? target = null);

    IQueryable<LogEntry> GetBefore(DateTime time, Int32 count);

    IQueryable<LogEntry> GetAfter(DateTime time, Int32 count);

    IQueryable<LogEntry> GetBeforeForTarget(LogTarget target, DateTime time, Int32 count);

    IQueryable<LogEntry> GetAfterForTarget(LogTarget target, DateTime time, Int32 count);
}
