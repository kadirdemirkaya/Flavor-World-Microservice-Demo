using AuthenticationService.Application.Abstractions;
using AuthenticationService.Domain.Aggregate.Entities;
using AuthenticationService.Domain.Aggregate.ValueObjects;
using BuildingBlock.Base.Abstractions;

namespace AuthenticationService.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        List<Role> Roles;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Roles = new();
        }

        public async Task<Role> GetUserRole(UserId Id)
        {
            var role = await _unitOfWork.GetReadRepository<RoleUser, RoleUserId>().GetAsync(r => r.UserId == Id);
            return await _unitOfWork.GetReadRepository<Role, RoleId>().GetAsync(r => r.Id == role.RoleId);
        }

        public async Task<List<Role>> GetUserRoles(UserId Id)
        {
            var roles = await _unitOfWork.GetReadRepository<RoleUser, RoleUserId>().GetAllAsync(r => r.UserId == Id);
            foreach (var role in roles)
            {
                Roles.Add(await _unitOfWork.GetReadRepository<Role, RoleId>().GetAsync(r => r.Id == role.RoleId));
            }
            return Roles;
        }
    }
}
