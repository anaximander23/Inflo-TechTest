using System;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var user = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == user.Email)
            .Which.Should().BeEquivalentTo(user);
    }

    private User SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var user = new User
        {
            Forename = forename,
            Surname = surname,
            Email = email,
            DateOfBirth = new DateOnly(2007, 10, 20),
            IsActive = isActive
        };

        _dataContext.Create(user);

        return user;
    }

    private readonly DataContext _dataContext = new(
        new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private UserService CreateService() => new(_dataContext);
}
