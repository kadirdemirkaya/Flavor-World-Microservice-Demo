using AuthenticationService.Application.Abstractions;
using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.Entities;
using AuthenticationService.Domain.Aggregate.Enums;
using AuthenticationService.Domain.Aggregate.Events;
using AuthenticationService.Domain.Aggregate.ValueObjects;
using AutoMapper;
using BuildingBlock.Base.Abstractions;
using MediatR;

namespace AuthenticationService.Application.Features.Command.Authentication.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;
        private readonly IWriteRepository<RoleUser, RoleUserId> _writeRoleUserRepository;
        private readonly IReadRepository<Role, RoleId> _readRoleRepository;
        private readonly IUserImageService _userImageService;
        private bool imageRes;

        public CreateUserCommandHandler(BuildingBlock.Base.Abstractions.IUnitOfWork unitOfWork, IMapper mapper, IHashService hashService, IWriteRepository<RoleUser, RoleUserId> writeRoleUserRepository, IReadRepository<Role, RoleId> readRoleRepository, IUserImageService userImageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashService = hashService;
            _writeRoleUserRepository = writeRoleUserRepository;
            _readRoleRepository = readRoleRepository;
            _userImageService = userImageService;
            imageRes = false;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            request.CreateUserDto.Password = _hashService.StringHashingEncrypt(request.CreateUserDto.Password);

            AuthenticationService.Domain.Aggregate.User? user = await _unitOfWork.GetReadRepository<AuthenticationService.Domain.Aggregate.User, UserId>().GetAsync(u => u.Email == request.CreateUserDto.Email);

            if (user is not null)
                return new(false);

            user = User.Create(request.CreateUserDto.FullName, request.CreateUserDto.Email, request.CreateUserDto.Password, DateTime.Now, UserStatus.Active);

            var role = await _readRoleRepository.GetAsync(r => r.RoleEnum == request.CreateUserDto.RoleName);

            user.AddUserRole(role.Id, user.Id, true, UserRoleStatus.Active);

            bool result = await _unitOfWork.GetWriteRepository<AuthenticationService.Domain.Aggregate.User, UserId>().CreateAsync(user);

            user.AddUserDomainEvent(new SendEmailEvent(user.Email));

            user.AddUserDomainEvent(new UserRegisterEvent(user.Email));

            if (result)
            {
                imageRes = await _userImageService.AddImageToUser(new() { Email = user.Email, FullName = user.FullName, Id = user.Id.Id }, new() { File = null, Name = Guid.NewGuid().ToString(), Path = "RightNowNullTest" });

                if (imageRes)
                {
                    int dbResult = await _unitOfWork.SaveChangesAsync();
                    return new(dbResult <= 0 ? false : true);
                }
            }
            return new(false);
        }
    }
}
