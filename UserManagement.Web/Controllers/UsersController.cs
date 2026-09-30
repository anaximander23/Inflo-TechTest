using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data.Entities;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Services.Results;
using UserManagement.Web.Extensions;
using UserManagement.Web.Models;
using UserManagement.Web.Models.Logs;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
[Authorize]
public class UsersController : Controller
{
    private const Int32 PageSize = 10;

    private readonly IUserService _userService;
    private readonly ILogService _logService;

    public UsersController(IUserService userService, ILogService logService)
    {
        _userService = userService;
        _logService = logService;
    }

    [HttpGet]
    public ViewResult List([FromQuery]Boolean? isActive = null)
    {
        IEnumerable<User> userRecords = isActive.HasValue ? _userService.FilterByActive(isActive.Value) : _userService.GetAll();

        IEnumerable<UserViewModel> results = userRecords
            .Select(p => new UserViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                DateOfBirth = p.DateOfBirth,
                IsActive = p.IsActive
            });

        UserListViewModel model = new()
        {
            Items = results.ToList()
        };

        return View(model);
    }

    [HttpGet("{id}")]
    public ViewResult Details([FromRoute] Int64 id, DateTime? before = null, DateTime? after = null)
    {
        var result = _userService.GetById(id);

        switch (result)
        {
            case SuccessResult<User> success:

                UserViewModel model = new()
                {
                    Id = success.Result.Id,
                    Forename = success.Result.Forename,
                    Surname = success.Result.Surname,
                    DateOfBirth = success.Result.DateOfBirth,
                    Email = success.Result.Email,
                    IsActive = success.Result.IsActive,
                    ActivityLogs = BuildActivityLogs(id, before, after)
                };

                return View(model);

            case ErrorResult error:
                return View("Error", new ErrorDetailModel { Message = "Error", Details = error.ErrorMessage });

            default:
                return View("Error", new ErrorDetailModel { Message = "Error", Details = $"An unexpected error occurred." });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("add")]
    public ViewResult Add() => View(new UserEditViewModel());

    [HttpPost("add")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Add([FromForm] UserEditViewModel model)
    {
        if (ModelState.IsValid)
        {
            User newUser = new()
            {
                Forename = model.Forename!,
                Surname = model.Surname!,
                Email = model.Email!,
                DateOfBirth = model.DateOfBirth,
                IsActive = model.IsActive
            };

            var result = await _userService.Add(newUser);

            switch (result)
            {
                case SuccessResult<User> success:
                     return RedirectToAction(nameof(List));

                case ErrorResult error:
                    ModelState.AddModelError(String.Empty, error.ErrorMessage);
                    break;

                default:
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                    break;
            }
        }

        return View(model);
    }

    [HttpGet("{id}/edit")]
    [Authorize(Roles = "Admin")]
    public ViewResult Edit([FromRoute] Int64 id)
    {
        var result = _userService.GetById(id);

        switch (result)
        {
            case SuccessResult<User> success:

                var model = new UserEditViewModel
                {
                    Id = success.Result.Id,
                    Forename = success.Result.Forename,
                    Surname = success.Result.Surname,
                    Email = success.Result.Email,
                    DateOfBirth = success.Result.DateOfBirth,
                    IsActive = success.Result.IsActive
                };
                return View(model);

            case ErrorResult error:
                return View("Error", new ErrorDetailModel { Message = "Error", Details = error.ErrorMessage });

            default:
                return View("Error", new ErrorDetailModel { Message = "Error", Details = $"An unexpected error occurred." });
        }
    }

    [HttpPost("{id}/edit")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([FromRoute] Int64 id, [FromForm] UserEditViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new()
            {
                Id = id,
                Forename = model.Forename!,
                Surname = model.Surname!,
                Email = model.Email!,
                DateOfBirth = model.DateOfBirth,
                IsActive = model.IsActive
            };

            var result = await _userService.Edit(id, user);

            switch (result)
            {
                case SuccessResult<User> success:
                    return RedirectToAction(nameof(List));

                case ErrorResult error:
                    ModelState.AddModelError(String.Empty, error.ErrorMessage);
                    break;

                default:
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                    break;
            }
        }

        return View(model);
    }

    private LogListViewModel BuildActivityLogs(Int64 id, DateTime? before, DateTime? after)
    {
        LogTarget target = new(LogTargets.User, id);

        IEnumerable<LogEntry> results;

        switch (before, after)
        {
            case (not null, null):
                results = _logService.GetBeforeForTarget(target, before.Value.AsUtc(), PageSize);
                break;

            case (null, not null):
                results = _logService.GetAfterForTarget(target, after.Value.AsUtc(), PageSize);
                break;

            default:
                results = _logService.GetBeforeForTarget(target, DateTime.UtcNow, PageSize);
                break;
        }

        return new LogListViewModel
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
                .ToList(),
            PagingAction = nameof(Details),
            RouteId = id
        };
    }
}
