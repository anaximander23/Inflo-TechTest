using System.Linq;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.WebMS.Controllers;

[Route("logs")]
[Authorize]
public class LogsController : Controller
{
    private readonly ILogService _logService;

    public LogsController(ILogService logService) => _logService = logService;

    [HttpGet]
    public ViewResult List()
    {
        DateTime now = DateTime.Now;

        var items = _logService
            .Get(now.Subtract(TimeSpan.FromDays(1)), now)
            .OrderByDescending(e => e.Timestamp)
            .Select(e => new LogEntryViewModel
            {
                Action = e.Action,
                Description = e.Description,
                ActorEmail = e.User == null ? null : e.User.Email,
                Timestamp = e.Timestamp
            })
            .ToList();

        return View(new LogListViewModel { Items = items });
    }
}
