namespace AuthenticationService.Test.Dtos
{
    public class CreateUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public RoleEnum? RoleName { get; set; } = RoleEnum.User;
    }
}
