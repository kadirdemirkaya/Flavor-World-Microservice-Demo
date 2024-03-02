using Google.Protobuf;
using ImageService.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace ImageService.Infrastructure.Services
{
    public class ImageTypeService : IImageTypeService
    {
        public ByteString ConvertIFormFileToByteArray(IFormFile formFile)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                return ByteString.CopyFrom(fileBytes);
            }
        }

        public IFormFile ConvertToIFormFile(byte[] fileBytes, string name)
        {
            using (MemoryStream memoryStream = new MemoryStream(fileBytes))
            {
                IFormFile formFile = new FormFile(memoryStream, 0, fileBytes.Length, "file", name);
                return formFile;
            }
        }
    }
}
