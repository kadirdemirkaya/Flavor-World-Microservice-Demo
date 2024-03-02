using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using ProductService.Application.Abstractions;

namespace ProductService.Infrastructure.Services
{
    public class ImageTypeService : IImageTypeService
    {
        public ByteString ConvertByteArrayToByteString(byte[] bytes) => ByteString.CopyFrom(bytes);

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
