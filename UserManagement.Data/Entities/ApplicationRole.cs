using System;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.Data.Entities;

public sealed class ApplicationRole : IdentityRole<Int64>
{
    public ApplicationRole()
    {            
    }

    public ApplicationRole(string roleName)
        : base(roleName)
    {
    }
}
