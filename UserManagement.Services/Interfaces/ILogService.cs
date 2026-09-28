using System.Linq;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Interfaces;

public interface ILogService
{
    void Log(string action, string description, ApplicationUser actor);

    IQueryable<LogEntry> GetAll();
}
