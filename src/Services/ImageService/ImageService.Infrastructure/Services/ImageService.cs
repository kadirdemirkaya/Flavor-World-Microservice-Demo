using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models;
using ImageService.Application.Abstractions;
using ImageService.Application.Exceptions;
using ImageService.Domain.Aggregate;
using ImageService.Domain.Aggregate.Entities;
using ImageService.Domain.Aggregate.Enums;
using ImageService.Domain.Aggregate.ValueObjects;
using Microsoft.AspNetCore.Http;
using static ImageService.Domain.Constants.Constant;

namespace ImageService.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private int saveResult;
        private bool imageResult;
        public ImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            saveResult = 0;
            imageResult = false;
        }

        public async Task<bool> AddImageAsync(FileUpload fileUpload)
        {
            try
            {
                var fileBytes = GetByteToImage(fileUpload.File);

                bool dbResult = await _unitOfWork.GetWriteRepository<Image, ImageId>().CreateAsync(Image.Create(fileUpload.Name, fileUpload.Path, GetByteToImage(fileUpload.File), FileType.image, ContentType.png));

                if (dbResult)
                    return await _unitOfWork.SaveChangesAsync() > 0;

                return false;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("SERVICE ERROR : " + ex.Message);
                throw new ServiceErrorException(nameof(ImageService), ex.Message);
            }
        }

        public async Task<bool> AddImageToUserAsync(BuildingBlock.Base.Models.UserModel user, FileUpload fileUpload)
        {
            try
            {
                bool existsUserImage = await _unitOfWork.GetReadRepository<ImageUser, ImageUserId>().AnyAsync(i => i.UserId == user.Id);

                if (existsUserImage is false)
                {
                    Image? image = Image.Create(fileUpload.Name, fileUpload.Path, GetByteToImage(fileUpload.File), FileType.image, ContentType.png);

                    AddImageUser(user, image);

                    imageResult = await _unitOfWork.GetWriteRepository<Image, ImageId>().CreateAsync(image);

                    if (imageResult is true)
                        return await _unitOfWork.SaveChangesAsync() > 0;
                    return false;
                }
                else if (existsUserImage is true)
                {
                    ImageUser? imageUser = await _unitOfWork.GetReadRepository<ImageUser, ImageUserId>().GetAsync(i => i.UserId == user.Id);
                    return await UpdateImageUserAsync(imageUser, fileUpload);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("SERVICE ERROR : " + ex.Message);
                throw new ServiceErrorException(nameof(ImageService), ex.Message);
            }
            return false;
        }

        public async Task<bool> AddImageToProduct(ProductModel productModel, FileUpload fileUpload)
        {
            try
            {
                bool dbResult = await _unitOfWork.GetReadRepository<ImageProduct, ImageProductId>().AnyAsync(ip => ip.ProductId == productModel.ProductId);
                if (dbResult is false)
                {
                    Image? image = Image.Create(fileUpload.Name, fileUpload.Path, GetByteToImage(fileUpload.File), FileType.image, ContentType.png);

                    AddImageProduct(productModel, image);

                    imageResult = await _unitOfWork.GetWriteRepository<Image, ImageId>().CreateAsync(image);

                    if (imageResult is true)
                        return await _unitOfWork.SaveChangesAsync() > 0;
                    return false;
                }
                else if (dbResult is true)
                {
                    ImageProduct? imageProduct = await _unitOfWork.GetReadRepository<ImageProduct, ImageProductId>().GetAsync(i => i.ProductId == productModel.ProductId);
                    return await UpdateImageProductAsync(imageProduct, fileUpload);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("SERVICE ERROR : " + ex.Message);
                throw new ServiceErrorException(nameof(ImageService), ex.Message);
            }
            return false;
        }

        public async Task<bool> UpdateImageProductAsync(ImageProduct imageProduct, FileUpload fileUpload)
        {
            try
            {
                Image? image = await _unitOfWork.GetReadRepository<Image, ImageId>().GetAsync(i => i.Id == imageProduct.ImageId);
                image.Photo = GetByteToImage(fileUpload.File);
                image.Path = fileUpload.Path;
                image.Name = fileUpload.Name;
                _unitOfWork.GetWriteRepository<Image, ImageId>().UpdateAsync(image);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("SERVICE ERROR : " + ex.Message);
                throw new ServiceErrorException(nameof(ImageService), ex.Message);
            }
        }


        public async Task<bool> UpdateImageUserAsync(ImageUser imageUser, FileUpload fileUpload)
        {
            try
            {
                //imageUser.ImageId
                Image? image = await _unitOfWork.GetReadRepository<Image, ImageId>().GetAsync(i => i.Id == imageUser.ImageId);
                image.Photo = GetByteToImage(fileUpload.File);
                image.Path = fileUpload.Path;
                image.Name = fileUpload.Name;
                _unitOfWork.GetWriteRepository<Image, ImageId>().UpdateAsync(image);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("SERVICE ERROR : " + ex.Message);
                throw new ServiceErrorException(nameof(ImageService), ex.Message);
            }
        }

        private void AddImageUser(BuildingBlock.Base.Models.UserModel userModel, Image image)
        {
            ImageUser? imageUser = ImageUser.Create(userModel.Id, image.Id.Id);
            image.AddImageUser(imageUser);
        }

        private void AddImageProduct(BuildingBlock.Base.Models.ProductModel productModel, Image image)
        {
            ImageProduct imageProduct = ImageProduct.Create(productModel.ProductId, image.Id.Id);
            image.AddImageProduct(imageProduct);
        }

        public async Task<bool> AssignUserDefaultImage(BuildingBlock.Base.Models.UserModel user)
        {
            try
            {
                bool anyImageUserResult = await _unitOfWork.GetReadRepository<Image, ImageId>().AnyAsync(i => i.Name == GetFileName(App.DefaultUserImagePath) && i.Path == App.DefaultUserImagePath);

                if (!anyImageUserResult)
                {
                    await _unitOfWork.GetWriteRepository<Image, ImageId>().CreateAsync(Image.Create(Guid.Parse(App.DefaultUserImageId), GetFileName(App.DefaultUserImagePath), App.DefaultUserImagePath, GetImageBytes(App.DefaultUserImagePath), FileType.image, ContentType.png));

                    await _unitOfWork.GetWriteRepository<ImageUser, ImageUserId>().CreateAsync(ImageUser.Create(user.Id, Guid.Parse(App.DefaultUserImageId)));
                }
                else
                    await _unitOfWork.GetWriteRepository<ImageUser, ImageUserId>().CreateAsync(ImageUser.Create(user.Id, Guid.Parse(App.DefaultUserImageId)));

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("SERVICE ERROR : " + ex.Message);
                throw new ServiceErrorException(nameof(ImageService), ex.Message);
            }
        }

        public async Task<bool> AssignProductDefaultImage(ProductModel productModel)
        {
            try
            {
                bool anyImageProductResult = await _unitOfWork.GetReadRepository<Image, ImageId>().AnyAsync(i => i.Name == GetFileName(App.DefaultProductImagePath) && i.Path == App.DefaultProductImagePath);

                if (!anyImageProductResult)
                {
                    await _unitOfWork.GetWriteRepository<Image, ImageId>().CreateAsync(Image.Create(Guid.Parse(App.DefaultProductImageId), GetFileName(App.DefaultProductImagePath), App.DefaultProductImagePath, GetImageBytes(App.DefaultProductImagePath), FileType.image, ContentType.png));

                    await _unitOfWork.GetWriteRepository<ImageProduct, ImageProductId>().CreateAsync(ImageProduct.Create(productModel.ProductId, Guid.Parse(App.DefaultProductImageId)));
                }
                else
                    await _unitOfWork.GetWriteRepository<ImageProduct, ImageProductId>().CreateAsync(ImageProduct.Create(productModel.ProductId, Guid.Parse(App.DefaultProductImageId)));

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("SERVICE ERROR : " + ex.Message);
                throw new ServiceErrorException(nameof(ImageService), ex.Message);
            }
        }





        public async Task<Image> GetImageAsync(Guid imageId)
            => await _unitOfWork.GetReadRepository<Image, ImageId>().GetAsync(i => i.Id == ImageId.Create(imageId)) ?? default(Image);

        private byte[] GetByteToImage(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private byte[] GetImageBytes(string imagePath)
        {
            try
            {
                return File.ReadAllBytes(imagePath);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("SERVICE ERROR : " + ex.Message);
                return null;
            }
        }

        private string GetFileName(string filePath)
            => Path.GetFileName(filePath);
    }
}
