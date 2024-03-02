﻿using Google.Protobuf;
using Microsoft.AspNetCore.Http;

namespace AuthenticationService.Application.Abstractions
{
    public interface IImageTypeService
    {
        IFormFile ConvertToIFormFile(byte[] fileBytes, string name);

        ByteString ConvertIFormFileToByteArray(IFormFile formFile);

        ByteString ConvertByteArrayToByteString(byte[] bytes);
    }
}
