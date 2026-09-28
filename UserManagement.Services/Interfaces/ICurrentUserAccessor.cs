using System.Threading.Tasks;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Interfaces;

public interface ICurrentUserAccessor
{
    Task<ApplicationUser?> GetCurrentActor();
}
