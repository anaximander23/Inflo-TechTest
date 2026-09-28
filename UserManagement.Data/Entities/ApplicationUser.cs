using System;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.Data.Entities;

public sealed class ApplicationUser : IdentityUser<Int64>
{
    public ApplicationUser()
    {
    }
}
