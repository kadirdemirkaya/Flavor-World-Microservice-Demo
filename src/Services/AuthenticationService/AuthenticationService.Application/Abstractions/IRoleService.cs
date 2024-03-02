using AuthenticationService.Domain.Aggregate.Entities;
using AuthenticationService.Domain.Aggregate.ValueObjects;

namespace AuthenticationService.Application.Abstractions
{
    public interface IRoleService
    {
        Task<Role> GetUserRole(UserId Id);

        Task<List<Role>> GetUserRoles(UserId Id);
    }
}
