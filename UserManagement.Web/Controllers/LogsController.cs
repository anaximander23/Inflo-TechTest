using System.Linq;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Extensions;
using UserManagement.Web.Models.Logs;

namespace UserManagement.WebMS.Controllers;

[Route("logs")]
[Authorize]
public class LogsController : Controller
{
    private const Int32 PageSize = 10;

    private readonly ILogService _logService;

    public LogsController(ILogService logService) => _logService = logService;

    [HttpGet]
    public ViewResult List(DateTime? before = null, DateTime? after = null)
    {
        IEnumerable<LogEntry> results;

        switch (before, after)
        {
            case (not null, null):
                results = _logService.GetBefore(before.Value.AsUtc(), PageSize);
                break;

            case (null, not null):
                results = _logService.GetAfter(after.Value.AsUtc(), PageSize);
                break;

            default:
                results = _logService.GetBefore(DateTime.UtcNow, PageSize);
                break;
        }

        var model = new LogListViewModel
        {
            Items = results
                .OrderByDescending(x => x.Timestamp)
                .Select(x => new LogEntryViewModel
                {
                    Action = x.Action,
                    Description = x.Description,
                    ActorEmail = x.User == null ? null : x.User.Email,
                    Timestamp = x.Timestamp
                })
                .ToList()
        };

        return View(model);
    }
}
