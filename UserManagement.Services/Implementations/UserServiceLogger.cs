using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data.Entities;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Services.Results;

namespace UserManagement.Services.Implementations;

public sealed class UserServiceLogger : IUserService
{
    private readonly IUserService _inner;
    private readonly ILogService _log;
    private readonly ICurrentUserAccessor _currentUser;

    public UserServiceLogger(IUserService inner, ILogService log, ICurrentUserAccessor currentUser)
    {
        _inner = inner;
        _log = log;
        _currentUser = currentUser;
    }

    public IEnumerable<User> FilterByActive(bool isActive) => _inner.FilterByActive(isActive);

    public IEnumerable<User> GetAll() => _inner.GetAll();

    public IOperationResult<User> GetById(long id) => _inner.GetById(id);

    public async Task<IOperationResult<User>> Add(User newUser)
    {
        var actor = await _currentUser.GetCurrentActor();
        if (actor is null)
        {
            return new ErrorResult<User>("Unable to resolve current user");
        }

        var result = await _inner.Add(newUser);

        if (result is SuccessResult<User> success)
        {
            _log.Log(
                "Added user",
                $"User {success.Result.Email} added",
                actor,
                new LogTarget(LogTargets.User, success.Result.Id, success.Result.Email));
        }

        return result;
    }

    public async Task<IOperationResult<User>> Edit(long id, User model)
    {
        var actor = await _currentUser.GetCurrentActor();
        if (actor is null)
        {
            return new ErrorResult<User>("Unable to resolve current user");
        }

        var result = await _inner.Edit(id, model);

        if (result is SuccessResult<User> success)
        {
            _log.Log(
                "Edited user",
                $"User {id} edited",
                actor,
                new LogTarget(LogTargets.User, id, success.Result.Email));
        }

        return result;
    }
}
