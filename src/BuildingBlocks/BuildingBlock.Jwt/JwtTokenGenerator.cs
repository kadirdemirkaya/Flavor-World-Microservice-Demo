using AuthenticationService.Domain.Aggregate.Enums;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BuildingBlock.Jwt
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Token GenerateToken(UserModel user)
        {
            var siginingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"])),
            SecurityAlgorithms.HmacSha256
            );
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName,user.FullName),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,GetRoleInEnum(RoleEnum.User))
            };

            var _expries = DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryMinutes"]));

            var securityToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: _expries,
                claims: claims,
                signingCredentials: siginingCredentials
            );

            Token token = new();
            token.Expiration = _expries;
            token.AccessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public Token GenerateTokenWithRole(UserModel user, string role)
        {
            var siginingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"])),
            SecurityAlgorithms.HmacSha256
            );
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName,user.FullName),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,role)
            };

            var _expries = DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryMinutes"]));

            var securityToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: _expries,
                claims: claims,
                signingCredentials: siginingCredentials
            );

            Token token = new();
            token.Expiration = _expries;
            token.AccessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }

        private string GetRoleInEnum(RoleEnum roleEnum)
        {
            RoleEnum role = roleEnum;
            return role.ToString();
        }
    }

}
