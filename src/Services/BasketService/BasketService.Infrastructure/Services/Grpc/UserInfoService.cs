using AuthenticationUserInfoService;
using BasketService.Application.Abstractions;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace BasketService.Infrastructure.Services.Grpc
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IConfiguration _configuration;

        public UserInfoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BuildingBlock.Base.Models.UserModel> GetUserModel(string token)
        {
            var channel = GrpcChannel.ForAddress(_configuration["GrpcAuthentication"]);
            var client = new GrpcUserInfo.GrpcUserInfoClient(channel);
            var request = new GetUserModelRequest() { Token = token };

            try
            {
                var reply = await client.GetUserInfoAsync(request);
                return new() { Email = reply.UserModel.Email, FullName = reply.UserModel.FullName, Id = Guid.Parse(reply.UserModel.Id) };
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("ERROR MESSAGE FOR GRPC: " + ex.Message);
                return null;
            }
        }
    }
}
