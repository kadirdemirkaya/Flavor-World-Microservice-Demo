using Grpc.Net.Client;
using ImageProductService;
using Microsoft.Extensions.Configuration;
using ProductService.Application.Abstractions;

namespace ProductService.Infrastructure.Services.Grpc
{
    public class ProductImageService : IProductImageService
    {
        private readonly IConfiguration _configuration;
        private readonly IImageTypeService _imageTypeService;
        private byte[] _defaultBytes;
        public ProductImageService(IConfiguration configuration, IImageTypeService imageTypeService)
        {
            _defaultBytes = new byte[] { 1 };
            _configuration = configuration;
            _imageTypeService = imageTypeService;
        }

        public async Task<bool> ProductImageAddAsync(BuildingBlock.Base.Models.ProductModel productModel, BuildingBlock.Base.Models.FileUpload fileUpload)
        {
            var channel = GrpcChannel.ForAddress(_configuration["GrpcImage"]);
            FileUploadModel fileUploadModel = new();
            var client = new GrpcImageProduct.GrpcImageProductClient(channel);

            if (fileUpload.File is null)
            {
                fileUploadModel.Files = _imageTypeService.ConvertByteArrayToByteString(_defaultBytes);
                fileUploadModel.Name = fileUpload.Name;
                fileUploadModel.Path = fileUpload.Path;
            }
            else
            {
                fileUploadModel.Files = _imageTypeService.ConvertIFormFileToByteArray(fileUpload.File);
                fileUploadModel.Name = fileUpload.Name;
                fileUploadModel.Path = fileUpload.Path;
            }

            var request = new ProductImageAddModelRequest()
            {
                FileUploadModel = fileUploadModel,
                ProductModel = new() { ProductId = productModel.ProductId.ToString() }
            };

            try
            {
                var reply = await client.ProductImageAddAsync(request);
                return reply.Result;
            }
            catch (System.Exception ex)
            {
                Serilog.Log.Error("GRPC error : " + ex.Message);
                return false;
            }

            return default;
        }
    }
}
