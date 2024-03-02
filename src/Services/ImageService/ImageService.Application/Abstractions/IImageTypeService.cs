using Google.Protobuf;
using Microsoft.AspNetCore.Http;

namespace ImageService.Application.Abstractions
{
    public interface IImageTypeService
    {
        IFormFile ConvertToIFormFile(byte[] fileBytes, string name);

        ByteString ConvertIFormFileToByteArray(IFormFile formFile);
    }
}
