using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Results;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

        /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        return GetAll().Where(u => u.IsActive == isActive);
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public IOperationResult<User> Add(User newUser)
    {
        Boolean emailCollides = _dataAccess
            .GetAll<User>()
            .Any(u => u.Email == newUser.Email);

        if (emailCollides)
        {
            return new ErrorResult<User>("Email address already exists");
        }

        try
        {
            _dataAccess.Create(newUser);

            return new SuccessResult<User>(newUser);
        }
        catch (Exception ex)
        {
            return new ErrorResult<User>(ex, "Failed to add user");
        }
    }

}
