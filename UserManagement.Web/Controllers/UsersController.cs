using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Results;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List([FromQuery]Boolean? isActive = null)
    {
        IEnumerable<User> userRecords = isActive.HasValue ? _userService.FilterByActive(isActive.Value) : _userService.GetAll();

        IEnumerable<UserListItemViewModel> results = userRecords
            .Select(p => new UserListItemViewModel
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

    [HttpGet("add")]
    public ViewResult Add() => View(new UserAddViewModel());

    [HttpPost("add")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Add([FromForm] UserAddViewModel model)
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

            var result = _userService.Add(newUser);

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
}
