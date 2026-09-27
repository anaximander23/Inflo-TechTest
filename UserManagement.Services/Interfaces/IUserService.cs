using System.Collections.Generic;
using UserManagement.Models;
using UserManagement.Services.Results;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    IOperationResult<User> Add(User newUser);

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    IEnumerable<User> FilterByActive(bool isActive);

    IEnumerable<User> GetAll();
}
