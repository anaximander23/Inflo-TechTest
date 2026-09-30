using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Results;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly DataContext _dataAccess;

    public UserService(DataContext dataAccess) => _dataAccess = dataAccess;

    public IEnumerable<User> FilterByActive(bool isActive)
        => GetAll().Where(u => u.IsActive == isActive);

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public IOperationResult<User> GetById(Int64 id)
    {
        var user = _dataAccess.GetAll<User>()
            .FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return new ErrorResult<User>("User not found");
        }

        return new SuccessResult<User>(user);
    }

    public async Task<IOperationResult<User>> Add(User newUser)
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

    public async Task<IOperationResult<User>> Edit(Int64 id, User model)
    {
        var userExists = _dataAccess.GetAll<User>()
            .Any(u => u.Id == id);

        // technically a TOCTOU race condition here -
        // in a real application we'd use locking, concurrency tokens, or atomic find-and-update operations where available
        // (depending on requirements regarding throughput and data consistency)
        if (!userExists)
        {
            return new ErrorResult<User>("User not found");
        }

        try
        {
            //NB: safe for this model; may not be appropriate if there are properties that shouldn't be edited.
            // Can also cause issues with navigation properties, depending on how they're mapped
            _dataAccess.Update(model);

            return new SuccessResult<User>(model);
        }
        catch (Exception ex)
        {
            return new ErrorResult<User>(ex, "Failed to edit user");
        }
    }
}
