using System;
using System.Collections.Generic;
using UserManagement.Models;
using UserManagement.Services.Results;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    IEnumerable<User> FilterByActive(bool isActive);

    IEnumerable<User> GetAll();
    IOperationResult<User> GetById(Int64 id);

    IOperationResult<User> Add(User newUser);
    IOperationResult<User> Edit(Int64 id, User model);

}
