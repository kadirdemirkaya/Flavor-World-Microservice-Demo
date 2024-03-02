using AuthenticationService.Application.Abstractions;
using BuildingBlock.Base.Abstractions;
using Grpc.Net.Client;
using ImageService;
using Microsoft.Extensions.Configuration;

namespace AuthenticationService.Infrastructure.Services.Grpc
{
    public class UserImageService : IUserImageService
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IImageTypeService _imageTypeService;
        private byte[] _defaultBytes;
        public UserImageService(IConfiguration configuration, ITokenService tokenService, IImageTypeService imageTypeService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _imageTypeService = imageTypeService;
            _defaultBytes = new byte[] { 1 };
        }

        public async Task<bool> AddImageToUser(BuildingBlock.Base.Models.UserModel userModel, BuildingBlock.Base.Models.FileUpload fileUpload)
        {
            var channel = GrpcChannel.ForAddress(_configuration["GrpcImage"]);
            FileUploadModel fileUploadModel = new();
            var client = new GrpcImageUser.GrpcImageUserClient(channel);
            if (fileUpload.File is null)
                fileUploadModel = new()
                {
                    Files = _imageTypeService.ConvertByteArrayToByteString(_defaultBytes),
                    Name = fileUpload.Name,
                    Path = fileUpload.Path
                };
            else
                fileUploadModel = new()
                {
                    Files = _imageTypeService.ConvertIFormFileToByteArray(fileUpload.File),
                    Name = fileUpload.Name,
                    Path = fileUpload.Path
                };

            var request = new UserImageAddModel()
            {
                FileUploadModel = fileUploadModel,
                UserModel = new()
                {
                    Email = userModel.Email,
                    FullName = userModel.FullName,
                    Id = userModel.Id.ToString()
                }
            };

            try
            {
                var reply = await client.UserImageAddAsync(request);
                return reply.Result;
            }
            catch (System.Exception ex)
            {
                Serilog.Log.Error("GRPC error : " + ex.Message);
                return false;
            }
        }
    }
}
