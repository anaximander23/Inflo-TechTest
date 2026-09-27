using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
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
}
