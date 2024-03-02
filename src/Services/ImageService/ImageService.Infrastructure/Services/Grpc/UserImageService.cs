using BuildingBlock.Base.Models;
using Grpc.Core;
using ImageService.Application.Abstractions;

namespace ImageService.Infrastructure.Services.Grpc
{
    public class UserImageService : GrpcImageUser.GrpcImageUserBase
    {
        private readonly IImageService _imageService;
        private readonly IImageTypeService _imageTypeService;
        private bool _dbResult;
        public UserImageService(IImageService imageService, IImageTypeService imageTypeService)
        {
            _imageService = imageService;
            _imageTypeService = imageTypeService;
            _dbResult = false;
        }

        public override async Task<UserImageAddModelResponse> UserImageAdd(UserImageAddModel request, ServerCallContext context)
        {
            if (request.FileUploadModel.Files is not null && request.FileUploadModel.Files.Length > 1)
            {
                _dbResult = await _imageService.AddImageToUserAsync
                 (
                     new BuildingBlock.Base.Models.UserModel()
                     {
                         Email = request.UserModel.Email,
                         FullName = request.UserModel.FullName,
                         Id = Guid.Parse(request.UserModel.Id)
                     },
                     new FileUpload()
                     {
                         File = _imageTypeService.ConvertToIFormFile(request.FileUploadModel.Files.ToArray(), request.FileUploadModel.Name),
                         Name = request.FileUploadModel.Name,
                         Path = request.FileUploadModel.Path
                     }
                 );
            }

            _dbResult = await _imageService.AssignUserDefaultImage(new() { Email = request.UserModel.Email, FullName = request.UserModel.FullName, Id = Guid.Parse(request.UserModel.Id) });

            return new() { Result = _dbResult};
        }
    }
}
