using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models;
using ImageService.Application.Abstractions;
using ImageService.Domain.Aggregate.Entities;
using MediatR;

namespace ImageService.Application.Features.Queries.Image.UserImageGet
{
    public class UserImageGetQueryHandler : IRequestHandler<UserImageGetQuery, UserImageGetQueryResponse>
    {
        private readonly IImageService _imageService;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public UserImageGetQueryHandler(IImageService imageService, ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserImageGetQueryResponse> Handle(UserImageGetQuery request, CancellationToken cancellationToken)
        {
            UserModel? user = await _tokenService.GetUserWithTokenAsync(request.token);

            List<ImageUser>? imageUser = await _unitOfWork.GetReadRepository<ImageUser>().GetAllAsync(iu => iu.UserId == user.Id);

            ImageService.Domain.Aggregate.Image? image = await _imageService.GetImageAsync(imageUser.FirstOrDefault().ImageId.Id);

            return new(image);
        }
    }
}
