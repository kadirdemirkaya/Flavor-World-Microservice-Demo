using AuthenticationService.Domain.Aggregate.Entities;
using AuthenticationService.Domain.Aggregate.Enums;
using AuthenticationService.Domain.Aggregate.ValueObjects;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models.Base;

namespace AuthenticationService.Domain.Aggregate
{
    public class User : AggregateRoot<UserId>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserStatus UserStatus { get; set; } = UserStatus.Active;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }



        private readonly List<RoleUser> _roleUsers = new();
        public IReadOnlyCollection<RoleUser> RoleUsers => _roleUsers.AsReadOnly();


        

        public User()
        {

        }
        public User(UserId Id) : base(Id)
        {

        }
        public User(UserId id, string fullName, string email, string password, DateTime createdDate, UserStatus userStatus) : base(id)
        {
            FullName = fullName;
            Email = email;
            Password = password;
            CreatedDate = createdDate;
            UserStatus = userStatus;
        }

        public static User Create(string fullName, string email, string password, DateTime createdDate, UserStatus userStatus)
        {
            return new(UserId.CreateUnique(), fullName, email, password, createdDate, userStatus);
        }

        public static User Create(Guid id, string fullName, string email, string password, DateTime createdDate, UserStatus userStatus)
        {
            return new(UserId.Create(id), fullName, email, password, createdDate, userStatus);
        }

        public void AddUserRole(RoleId roleId, UserId userId, bool isActive, UserRoleStatus userRoleStatus)
        {
            _roleUsers.Add(RoleUser.Create(roleId, userId, isActive, userRoleStatus));
        }

        public List<RoleUser> GetUserRoles() => RoleUsers.ToList();

        public void AddUserDomainEvent(IDomainEvent @event)
        {
            AddDomainEvent(@event);
        }

        public void CountDomainEvent() => DomainEvents.Count();
    }
}
